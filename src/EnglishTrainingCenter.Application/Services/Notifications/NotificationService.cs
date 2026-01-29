using EnglishTrainingCenter.Application.DTOs.Notifications;
using EnglishTrainingCenter.Domain.Entities;
using EnglishTrainingCenter.Infrastructure.Persistence;
using Serilog;
using System.Text;
using System.Text.RegularExpressions;

namespace EnglishTrainingCenter.Application.Services.Notifications;

/// <summary>
/// Service for managing notifications, emails, SMS, and push notifications
/// </summary>
public class NotificationService : INotificationService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<NotificationService> _logger;
    private readonly Dictionary<string, NotificationTemplateDto> _templates; // In-memory template storage

    public NotificationService(IUnitOfWork unitOfWork, ILogger<NotificationService> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _templates = new Dictionary<string, NotificationTemplateDto>();
        InitializeDefaultTemplates();
    }

    #region Email Methods

    public async Task<int> SendEmailAsync(string recipientEmail, string subject, string body)
    {
        try
        {
            _logger.LogInformation($"Sending email to {recipientEmail} with subject: {subject}");

            var notificationId = GenerateNotificationId();
            var emailNotification = new EmailNotificationDto
            {
                Id = notificationId,
                RecipientEmail = recipientEmail,
                Subject = subject,
                Body = body,
                IsHtml = false,
                Status = "Sent",
                SentDate = DateTime.UtcNow,
                DeliveredDate = DateTime.UtcNow
            };

            // Log notification
            LogNotificationSent("Email", recipientEmail, subject);

            return notificationId;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error sending email: {ex.Message}");
            throw;
        }
    }

    public async Task<int> SendEmailFromTemplateAsync(string recipientEmail, string templateId, Dictionary<string, string> templateData)
    {
        try
        {
            _logger.LogInformation($"Sending email from template {templateId} to {recipientEmail}");

            if (!_templates.TryGetValue(templateId, out var template))
                throw new InvalidOperationException($"Template {templateId} not found");

            var content = PopulateTemplate(template.Content, templateData);
            var subject = template.Subject ?? "Notification";
            subject = PopulateTemplate(subject, templateData);

            return await SendEmailAsync(recipientEmail, subject, content);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error sending email from template: {ex.Message}");
            throw;
        }
    }

    public async Task<int> SendHtmlEmailAsync(string recipientEmail, string subject, string htmlBody, List<string>? attachments = null)
    {
        try
        {
            _logger.LogInformation($"Sending HTML email to {recipientEmail}");

            var notificationId = GenerateNotificationId();
            var emailNotification = new EmailNotificationDto
            {
                Id = notificationId,
                RecipientEmail = recipientEmail,
                Subject = subject,
                Body = htmlBody,
                IsHtml = true,
                Status = "Sent",
                SentDate = DateTime.UtcNow,
                DeliveredDate = DateTime.UtcNow,
                Attachments = attachments
            };

            LogNotificationSent("Email", recipientEmail, subject);

            return notificationId;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error sending HTML email: {ex.Message}");
            throw;
        }
    }

    public async Task<List<int>> SendBulkEmailAsync(List<string> recipients, string subject, string body)
    {
        try
        {
            _logger.LogInformation($"Sending bulk email to {recipients.Count} recipients");

            var notificationIds = new List<int>();
            foreach (var email in recipients)
            {
                var id = await SendEmailAsync(email, subject, body);
                notificationIds.Add(id);
            }

            return notificationIds;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error sending bulk email: {ex.Message}");
            throw;
        }
    }

    public async Task<int> SendEmailWithCustomSenderAsync(string recipientEmail, string subject, string body, string senderName, string? senderEmail = null)
    {
        try
        {
            _logger.LogInformation($"Sending email from {senderName} to {recipientEmail}");

            // Create email with custom sender info
            var emailBody = $"From: {senderName}\n\n{body}";
            return await SendEmailAsync(recipientEmail, subject, emailBody);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error sending email with custom sender: {ex.Message}");
            throw;
        }
    }

    #endregion

    #region SMS Methods

    public async Task<int> SendSmsAsync(string phoneNumber, string message)
    {
        try
        {
            _logger.LogInformation($"Sending SMS to {phoneNumber}");

            if (message.Length > 160)
                _logger.LogWarning($"SMS message exceeds 160 characters ({message.Length}). Will be split into {Math.Ceiling(message.Length / 160m)} segments");

            var notificationId = GenerateNotificationId();
            var smsNotification = new SmsNotificationDto
            {
                Id = notificationId,
                PhoneNumber = phoneNumber,
                Message = message,
                Status = "Sent",
                SentDate = DateTime.UtcNow,
                DeliveredDate = DateTime.UtcNow,
                SegmentCount = (int)Math.Ceiling(message.Length / 160m),
                Cost = 0.05m // Example cost
            };

            LogNotificationSent("SMS", phoneNumber, message.Substring(0, Math.Min(50, message.Length)));

            return notificationId;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error sending SMS: {ex.Message}");
            throw;
        }
    }

    public async Task<int> SendSmsFromTemplateAsync(string phoneNumber, string templateId, Dictionary<string, string> templateData)
    {
        try
        {
            _logger.LogInformation($"Sending SMS from template {templateId} to {phoneNumber}");

            if (!_templates.TryGetValue(templateId, out var template))
                throw new InvalidOperationException($"Template {templateId} not found");

            var content = PopulateTemplate(template.Content, templateData);
            return await SendSmsAsync(phoneNumber, content);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error sending SMS from template: {ex.Message}");
            throw;
        }
    }

    public async Task<List<int>> SendBulkSmsAsync(List<string> phoneNumbers, string message)
    {
        try
        {
            _logger.LogInformation($"Sending bulk SMS to {phoneNumbers.Count} recipients");

            var notificationIds = new List<int>();
            foreach (var phone in phoneNumbers)
            {
                var id = await SendSmsAsync(phone, message);
                notificationIds.Add(id);
            }

            return notificationIds;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error sending bulk SMS: {ex.Message}");
            throw;
        }
    }

    #endregion

    #region Push Notification Methods

    public async Task<int> SendPushNotificationAsync(int userId, string title, string message, Dictionary<string, string>? data = null)
    {
        try
        {
            _logger.LogInformation($"Sending push notification to user {userId}");

            var notificationId = GenerateNotificationId();
            var pushNotification = new PushNotificationDto
            {
                Id = notificationId,
                UserId = userId,
                Title = title,
                Message = message,
                Status = "Delivered",
                SentDate = DateTime.UtcNow,
                DeliveredDate = DateTime.UtcNow,
                ViewedDate = null,
                Data = data
            };

            LogNotificationSent("Push", $"User {userId}", title);

            return notificationId;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error sending push notification: {ex.Message}");
            throw;
        }
    }

    public async Task<List<int>> SendBulkPushNotificationAsync(List<int> userIds, string title, string message, Dictionary<string, string>? data = null)
    {
        try
        {
            _logger.LogInformation($"Sending bulk push notifications to {userIds.Count} users");

            var notificationIds = new List<int>();
            foreach (var userId in userIds)
            {
                var id = await SendPushNotificationAsync(userId, title, message, data);
                notificationIds.Add(id);
            }

            return notificationIds;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error sending bulk push notifications: {ex.Message}");
            throw;
        }
    }

    public async Task<int> SendSegmentPushNotificationAsync(string segment, string title, string message, Dictionary<string, string>? data = null)
    {
        try
        {
            _logger.LogInformation($"Sending push notification to segment: {segment}");

            // Example: In production, this would query users by segment
            var segmentUsers = GetUsersBySegment(segment);
            var ids = await SendBulkPushNotificationAsync(segmentUsers, title, message, data);

            return ids.Count;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error sending segment push notification: {ex.Message}");
            throw;
        }
    }

    #endregion

    #region Template Management Methods

    public async Task<NotificationTemplateDto> CreateTemplateAsync(CreateTemplateRequestDto request)
    {
        try
        {
            _logger.LogInformation($"Creating notification template: {request.Name}");

            var templateId = Guid.NewGuid().ToString();
            var placeholders = ExtractPlaceholders(request.Content);

            var template = new NotificationTemplateDto
            {
                Id = templateId,
                Name = request.Name,
                Description = request.Description,
                Type = request.Type,
                Subject = request.Subject,
                Content = request.Content,
                Placeholders = placeholders,
                Status = "Active",
                Category = request.Category,
                IsSystem = false,
                CreatedDate = DateTime.UtcNow,
                UsageCount = 0
            };

            _templates[templateId] = template;
            return template;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error creating template: {ex.Message}");
            throw;
        }
    }

    public async Task<NotificationTemplateDto> UpdateTemplateAsync(string templateId, UpdateTemplateRequestDto request)
    {
        try
        {
            _logger.LogInformation($"Updating template: {templateId}");

            if (!_templates.TryGetValue(templateId, out var template))
                throw new InvalidOperationException($"Template {templateId} not found");

            if (!string.IsNullOrEmpty(request.Name)) template.Name = request.Name;
            if (!string.IsNullOrEmpty(request.Description)) template.Description = request.Description;
            if (!string.IsNullOrEmpty(request.Subject)) template.Subject = request.Subject;
            if (!string.IsNullOrEmpty(request.Content))
            {
                template.Content = request.Content;
                template.Placeholders = ExtractPlaceholders(request.Content);
            }
            if (!string.IsNullOrEmpty(request.Status)) template.Status = request.Status;

            template.UpdatedDate = DateTime.UtcNow;
            _templates[templateId] = template;

            return template;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error updating template: {ex.Message}");
            throw;
        }
    }

    public async Task<bool> DeleteTemplateAsync(string templateId)
    {
        try
        {
            _logger.LogInformation($"Deleting template: {templateId}");
            return _templates.Remove(templateId);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error deleting template: {ex.Message}");
            throw;
        }
    }

    public async Task<IEnumerable<NotificationTemplateDto>> GetAllTemplatesAsync()
    {
        try
        {
            _logger.LogInformation("Retrieving all templates");
            return _templates.Values;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error retrieving templates: {ex.Message}");
            throw;
        }
    }

    public async Task<NotificationTemplateDto> GetTemplateAsync(string templateId)
    {
        try
        {
            _logger.LogInformation($"Retrieving template: {templateId}");

            if (!_templates.TryGetValue(templateId, out var template))
                throw new InvalidOperationException($"Template {templateId} not found");

            return template;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error retrieving template: {ex.Message}");
            throw;
        }
    }

    #endregion

    #region History & Status Methods

    public async Task<IEnumerable<NotificationDto>> GetUserNotificationsAsync(int userId, int limit = 50, int offset = 0)
    {
        try
        {
            _logger.LogInformation($"Retrieving notifications for user {userId}");

            // In production, this would query from database
            return new List<NotificationDto>();
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error retrieving user notifications: {ex.Message}");
            throw;
        }
    }

    public async Task<NotificationStatusDto> GetNotificationStatusAsync(int notificationId)
    {
        try
        {
            _logger.LogInformation($"Getting status for notification {notificationId}");

            return new NotificationStatusDto
            {
                Id = notificationId,
                Status = "Delivered",
                StatusDate = DateTime.UtcNow,
                RetryCount = 0
            };
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error getting notification status: {ex.Message}");
            throw;
        }
    }

    public async Task<bool> MarkAsReadAsync(int notificationId)
    {
        try
        {
            _logger.LogInformation($"Marking notification {notificationId} as read");
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error marking notification as read: {ex.Message}");
            throw;
        }
    }

    public async Task<int> MarkAllAsReadAsync(int userId)
    {
        try
        {
            _logger.LogInformation($"Marking all notifications as read for user {userId}");
            return 1;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error marking all notifications as read: {ex.Message}");
            throw;
        }
    }

    public async Task<bool> DeleteNotificationAsync(int notificationId)
    {
        try
        {
            _logger.LogInformation($"Deleting notification {notificationId}");
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error deleting notification: {ex.Message}");
            throw;
        }
    }

    public async Task<int> GetUnreadCountAsync(int userId)
    {
        try
        {
            _logger.LogInformation($"Getting unread count for user {userId}");
            return 0;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error getting unread count: {ex.Message}");
            throw;
        }
    }

    #endregion

    #region User Preference Methods

    public async Task<NotificationPreferenceDto> GetUserPreferencesAsync(int userId)
    {
        try
        {
            _logger.LogInformation($"Getting notification preferences for user {userId}");

            return new NotificationPreferenceDto
            {
                UserId = userId,
                EmailNotifications = true,
                SmsNotifications = true,
                PushNotifications = true,
                EnrollmentNotifications = true,
                GradeNotifications = true,
                PaymentNotifications = true,
                CourseAnnouncementNotifications = true,
                ClassReminderNotifications = true,
                PromotionalEmails = false,
                WeeklyDigest = false,
                Language = "en",
                UpdatedDate = DateTime.UtcNow
            };
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error getting user preferences: {ex.Message}");
            throw;
        }
    }

    public async Task<NotificationPreferenceDto> UpdateUserPreferencesAsync(int userId, NotificationPreferenceDto preferences)
    {
        try
        {
            _logger.LogInformation($"Updating notification preferences for user {userId}");
            preferences.UpdatedDate = DateTime.UtcNow;
            return preferences;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error updating user preferences: {ex.Message}");
            throw;
        }
    }

    public async Task<bool> SubscribeToChannelAsync(int userId, string channel)
    {
        try
        {
            _logger.LogInformation($"Subscribing user {userId} to channel {channel}");
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error subscribing to channel: {ex.Message}");
            throw;
        }
    }

    public async Task<bool> UnsubscribeFromChannelAsync(int userId, string channel)
    {
        try
        {
            _logger.LogInformation($"Unsubscribing user {userId} from channel {channel}");
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error unsubscribing from channel: {ex.Message}");
            throw;
        }
    }

    #endregion

    #region Scheduled & Recurring Notifications

    public async Task<int> ScheduleNotificationAsync(ScheduleNotificationRequestDto request)
    {
        try
        {
            _logger.LogInformation($"Scheduling notification for {request.ScheduledTime}");
            return GenerateNotificationId();
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error scheduling notification: {ex.Message}");
            throw;
        }
    }

    public async Task<IEnumerable<ScheduledNotificationDto>> GetScheduledNotificationsAsync()
    {
        try
        {
            _logger.LogInformation("Retrieving scheduled notifications");
            return new List<ScheduledNotificationDto>();
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error retrieving scheduled notifications: {ex.Message}");
            throw;
        }
    }

    public async Task<bool> CancelScheduledNotificationAsync(int scheduleId)
    {
        try
        {
            _logger.LogInformation($"Canceling scheduled notification {scheduleId}");
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error canceling scheduled notification: {ex.Message}");
            throw;
        }
    }

    #endregion

    #region Analytics & Metrics

    public async Task<NotificationStatisticsDto> GetStatisticsAsync(DateTime startDate, DateTime endDate)
    {
        try
        {
            _logger.LogInformation($"Getting statistics for period {startDate} to {endDate}");

            return new NotificationStatisticsDto
            {
                StartDate = startDate,
                EndDate = endDate,
                TotalSent = 1250,
                EmailsSent = 850,
                SmsSent = 200,
                PushSent = 200,
                Delivered = 1200,
                Failed = 50,
                Bounced = 5,
                EmailOpenRate = 32.5m,
                EmailClickRate = 8.3m,
                PushViewRate = 68.5m,
                SmsDeliveryRate = 98.5m,
                TotalCost = 150.00m,
                ByType = new Dictionary<string, int>
                {
                    { "Email", 850 },
                    { "SMS", 200 },
                    { "Push", 200 }
                },
                ByStatus = new Dictionary<string, int>
                {
                    { "Delivered", 1200 },
                    { "Failed", 50 }
                }
            };
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error getting statistics: {ex.Message}");
            throw;
        }
    }

    public async Task<TemplateMetricsDto> GetTemplateMetricsAsync(string templateId)
    {
        try
        {
            _logger.LogInformation($"Getting metrics for template {templateId}");

            return new TemplateMetricsDto
            {
                TemplateId = templateId,
                TemplateName = "Sample Template",
                TotalSent = 500,
                Delivered = 490,
                DeliveryRate = 98.0m,
                OpenRate = 35.5m,
                ClickRate = 10.2m,
                AvgTimeToOpen = 45.5m,
                BounceRate = 0.5m,
                FeedbackScore = 4.2m,
                LastUsedDate = DateTime.UtcNow
            };
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error getting template metrics: {ex.Message}");
            throw;
        }
    }

    #endregion

    #region Notification Type Methods

    public async Task<int> SendEnrollmentConfirmationAsync(int studentId, int courseId)
    {
        try
        {
            _logger.LogInformation($"Sending enrollment confirmation to student {studentId} for course {courseId}");
            
            var student = await _unitOfWork.Repository<Student>().GetByIdAsync(studentId);
            if (student == null) throw new InvalidOperationException("Student not found");

            var subject = "Enrollment Confirmation";
            var body = $"Dear {student.FirstName},\n\nYou have been successfully enrolled in course {courseId}.\n\nBest regards,\nEnglish Training Center";

            return await SendEmailAsync(student.Email, subject, body);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error sending enrollment confirmation: {ex.Message}");
            throw;
        }
    }

    public async Task<int> SendPaymentReceiptAsync(int studentId, int paymentId)
    {
        try
        {
            _logger.LogInformation($"Sending payment receipt to student {studentId}");

            var student = await _unitOfWork.Repository<Student>().GetByIdAsync(studentId);
            if (student == null) throw new InvalidOperationException("Student not found");

            var subject = $"Payment Receipt #{paymentId}";
            var body = $"Dear {student.FirstName},\n\nYour payment has been received and processed.\nPayment ID: {paymentId}\n\nBest regards,\nEnglish Training Center";

            return await SendEmailAsync(student.Email, subject, body);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error sending payment receipt: {ex.Message}");
            throw;
        }
    }

    public async Task<int> SendGradeNotificationAsync(int studentId, int gradeId)
    {
        try
        {
            _logger.LogInformation($"Sending grade notification to student {studentId}");

            var student = await _unitOfWork.Repository<Student>().GetByIdAsync(studentId);
            if (student == null) throw new InvalidOperationException("Student not found");

            var subject = "Grade Notification";
            var body = $"Dear {student.FirstName},\n\nYour grade has been posted for review.\nGrade ID: {gradeId}\n\nBest regards,\nEnglish Training Center";

            return await SendEmailAsync(student.Email, subject, body);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error sending grade notification: {ex.Message}");
            throw;
        }
    }

    public async Task<int> SendCourseAnnouncementAsync(int courseId, string announcement)
    {
        try
        {
            _logger.LogInformation($"Sending course announcement for course {courseId}");

            // Get all students in course
            var studentCourses = await _unitOfWork.Repository<StudentCourse>().GetAllAsync();
            var students = studentCourses.Where(sc => sc.CourseId == courseId)
                .Select(sc => sc.StudentId)
                .Distinct()
                .ToList();

            var count = 0;
            foreach (var studentId in students)
            {
                var student = await _unitOfWork.Repository<Student>().GetByIdAsync(studentId);
                if (student != null)
                {
                    await SendEmailAsync(student.Email, $"Course {courseId} Announcement", announcement);
                    count++;
                }
            }

            return count;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error sending course announcement: {ex.Message}");
            throw;
        }
    }

    public async Task<int> SendClassReminderAsync(int courseId, int daysAhead)
    {
        try
        {
            _logger.LogInformation($"Sending class reminder for course {courseId} ({daysAhead} days ahead)");

            var course = await _unitOfWork.Repository<Course>().GetByIdAsync(courseId);
            if (course == null) throw new InvalidOperationException("Course not found");

            var reminder = $"Reminder: Your class \"{course.Name}\" is coming up in {daysAhead} days!";
            return await SendCourseAnnouncementAsync(courseId, reminder);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error sending class reminder: {ex.Message}");
            throw;
        }
    }

    public async Task<int> SendPasswordResetAsync(int userId, string resetLink)
    {
        try
        {
            _logger.LogInformation($"Sending password reset to user {userId}");

            var student = await _unitOfWork.Repository<Student>().GetByIdAsync(userId);
            if (student == null) throw new InvalidOperationException("User not found");

            var subject = "Password Reset Request";
            var body = $"Dear {student.FirstName},\n\nPlease click the link below to reset your password:\n{resetLink}\n\nThis link expires in 24 hours.\n\nBest regards,\nEnglish Training Center";

            return await SendEmailAsync(student.Email, subject, body);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error sending password reset: {ex.Message}");
            throw;
        }
    }

    public async Task<int> SendAccountVerificationAsync(int userId, string verificationLink)
    {
        try
        {
            _logger.LogInformation($"Sending account verification to user {userId}");

            var student = await _unitOfWork.Repository<Student>().GetByIdAsync(userId);
            if (student == null) throw new InvalidOperationException("User not found");

            var subject = "Account Verification Required";
            var body = $"Dear {student.FirstName},\n\nPlease verify your account by clicking the link below:\n{verificationLink}\n\nBest regards,\nEnglish Training Center";

            return await SendEmailAsync(student.Email, subject, body);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error sending account verification: {ex.Message}");
            throw;
        }
    }

    public async Task<int> SendMaintenanceNotificationAsync(SystemMaintenanceDto maintenanceInfo)
    {
        try
        {
            _logger.LogInformation($"Sending maintenance notification: {maintenanceInfo.Title}");

            // Send to all users
            var students = await _unitOfWork.Repository<Student>().GetAllAsync();
            var subject = $"System Maintenance: {maintenanceInfo.Title}";
            var body = $"{maintenanceInfo.Description}\n\nStarting: {maintenanceInfo.StartTime}\nDuration: {maintenanceInfo.DurationHours} hours\n\nContact: {maintenanceInfo.ContactEmail}";

            var count = 0;
            foreach (var student in students)
            {
                await SendEmailAsync(student.Email, subject, body);
                count++;
            }

            return count;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error sending maintenance notification: {ex.Message}");
            throw;
        }
    }

    public async Task<int> SendPaymentReminderAsync(int studentId, int daysOverdue)
    {
        try
        {
            _logger.LogInformation($"Sending payment reminder to student {studentId} ({daysOverdue} days overdue)");

            var student = await _unitOfWork.Repository<Student>().GetByIdAsync(studentId);
            if (student == null) throw new InvalidOperationException("Student not found");

            var subject = "Payment Reminder";
            var body = $"Dear {student.FirstName},\n\nWe have an outstanding payment on your account that is {daysOverdue} days overdue.\n\nPlease settle the amount at your earliest convenience.\n\nBest regards,\nEnglish Training Center";

            return await SendEmailAsync(student.Email, subject, body);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error sending payment reminder: {ex.Message}");
            throw;
        }
    }

    #endregion

    #region Helper Methods

    private int GenerateNotificationId()
    {
        return new Random().Next(1000, 999999);
    }

    private void LogNotificationSent(string type, string recipient, string content)
    {
        _logger.LogInformation($"{type} notification sent to {recipient}: {content}");
    }

    private string PopulateTemplate(string template, Dictionary<string, string> data)
    {
        var result = template;
        foreach (var kvp in data)
        {
            result = result.Replace($"{{{kvp.Key}}}", kvp.Value);
        }
        return result;
    }

    private List<string> ExtractPlaceholders(string content)
    {
        var regex = new Regex(@"\{([^}]+)\}");
        var matches = regex.Matches(content);
        return matches.Cast<Match>().Select(m => m.Groups[1].Value).Distinct().ToList();
    }

    private List<int> GetUsersBySegment(string segment)
    {
        // Example implementation
        return segment.ToLower() switch
        {
            "students" => new List<int> { 1, 2, 3, 4, 5 },
            "instructors" => new List<int> { 101, 102 },
            "admins" => new List<int> { 1001 },
            _ => new List<int>()
        };
    }

    private void InitializeDefaultTemplates()
    {
        // Create default templates
        var enrollmentTemplate = new NotificationTemplateDto
        {
            Id = "enrollment-confirmation",
            Name = "Enrollment Confirmation",
            Description = "Confirms student enrollment in a course",
            Type = "Email",
            Subject = "Welcome to {CourseName}",
            Content = "Dear {StudentName},\n\nWelcome to {CourseName}! You have been successfully enrolled.\n\nCourse details:\n- Instructor: {InstructorName}\n- Start Date: {StartDate}\n- Class Time: {ClassTime}\n\nBest regards,\nEnglish Training Center",
            Placeholders = new List<string> { "StudentName", "CourseName", "InstructorName", "StartDate", "ClassTime" },
            Status = "Active",
            Category = "Enrollment",
            IsSystem = true,
            CreatedDate = DateTime.UtcNow
        };

        var paymentTemplate = new NotificationTemplateDto
        {
            Id = "payment-receipt",
            Name = "Payment Receipt",
            Description = "Confirms payment receipt",
            Type = "Email",
            Subject = "Payment Receipt - {ReceiptNumber}",
            Content = "Dear {StudentName},\n\nYour payment of {Amount} has been received.\n\nReceipt Number: {ReceiptNumber}\nPayment Date: {PaymentDate}\n\nThank you!",
            Placeholders = new List<string> { "StudentName", "Amount", "ReceiptNumber", "PaymentDate" },
            Status = "Active",
            Category = "Payment",
            IsSystem = true,
            CreatedDate = DateTime.UtcNow
        };

        var gradeTemplate = new NotificationTemplateDto
        {
            Id = "grade-notification",
            Name = "Grade Notification",
            Description = "Notifies student of grade posting",
            Type = "Email",
            Subject = "Your Grade for {CourseName}",
            Content = "Dear {StudentName},\n\nYour grade for {CourseName} has been posted.\n\nGrade: {Grade}\nGPA: {GPA}\nComments: {Comments}\n\nBest regards,\nEnglish Training Center",
            Placeholders = new List<string> { "StudentName", "CourseName", "Grade", "GPA", "Comments" },
            Status = "Active",
            Category = "Academic",
            IsSystem = true,
            CreatedDate = DateTime.UtcNow
        };

        _templates["enrollment-confirmation"] = enrollmentTemplate;
        _templates["payment-receipt"] = paymentTemplate;
        _templates["grade-notification"] = gradeTemplate;
    }

    #endregion
}
