using EnglishTrainingCenter.Application.DTOs.Notifications;

namespace EnglishTrainingCenter.Application.Services.Notifications;

/// <summary>
/// Service for managing notifications, emails, SMS, and push notifications
/// </summary>
public interface INotificationService
{
    // Email Notification Methods
    /// <summary>
    /// Send a simple text email
    /// </summary>
    /// <param name="recipientEmail">Email recipient address</param>
    /// <param name="subject">Email subject</param>
    /// <param name="body">Email body content</param>
    /// <returns>Notification ID</returns>
    Task<int> SendEmailAsync(string recipientEmail, string subject, string body);

    /// <summary>
    /// Send email using predefined template
    /// </summary>
    /// <param name="recipientEmail">Email recipient address</param>
    /// <param name="templateId">Email template ID</param>
    /// <param name="templateData">Data to populate template placeholders</param>
    /// <returns>Notification ID</returns>
    Task<int> SendEmailFromTemplateAsync(string recipientEmail, string templateId, Dictionary<string, string> templateData);

    /// <summary>
    /// Send HTML formatted email
    /// </summary>
    /// <param name="recipientEmail">Email recipient address</param>
    /// <param name="subject">Email subject</param>
    /// <param name="htmlBody">HTML content</param>
    /// <param name="attachments">Optional file attachments</param>
    /// <returns>Notification ID</returns>
    Task<int> SendHtmlEmailAsync(string recipientEmail, string subject, string htmlBody, List<string>? attachments = null);

    /// <summary>
    /// Send bulk emails to multiple recipients
    /// </summary>
    /// <param name="recipients">List of recipient emails</param>
    /// <param name="subject">Email subject</param>
    /// <param name="body">Email body</param>
    /// <returns>List of notification IDs</returns>
    Task<List<int>> SendBulkEmailAsync(List<string> recipients, string subject, string body);

    /// <summary>
    /// Send email with custom sender information
    /// </summary>
    /// <param name="recipientEmail">Email recipient address</param>
    /// <param name="subject">Email subject</param>
    /// <param name="body">Email body</param>
    /// <param name="senderName">Custom sender name</param>
    /// <param name="senderEmail">Custom sender email (if allowed)</param>
    /// <returns>Notification ID</returns>
    Task<int> SendEmailWithCustomSenderAsync(string recipientEmail, string subject, string body, string senderName, string? senderEmail = null);

    // SMS Notification Methods
    /// <summary>
    /// Send SMS notification
    /// </summary>
    /// <param name="phoneNumber">Phone number (E.164 format)</param>
    /// <param name="message">SMS message content (max 160 characters)</param>
    /// <returns>Notification ID</returns>
    Task<int> SendSmsAsync(string phoneNumber, string message);

    /// <summary>
    /// Send SMS from template
    /// </summary>
    /// <param name="phoneNumber">Phone number (E.164 format)</param>
    /// <param name="templateId">SMS template ID</param>
    /// <param name="templateData">Data to populate template placeholders</param>
    /// <returns>Notification ID</returns>
    Task<int> SendSmsFromTemplateAsync(string phoneNumber, string templateId, Dictionary<string, string> templateData);

    /// <summary>
    /// Send bulk SMS to multiple recipients
    /// </summary>
    /// <param name="phoneNumbers">List of phone numbers</param>
    /// <param name="message">SMS message content</param>
    /// <returns>List of notification IDs</returns>
    Task<List<int>> SendBulkSmsAsync(List<string> phoneNumbers, string message);

    // Push Notification Methods
    /// <summary>
    /// Send push notification
    /// </summary>
    /// <param name="userId">User ID to send notification to</param>
    /// <param name="title">Notification title</param>
    /// <param name="message">Notification message</param>
    /// <param name="data">Optional additional data payload</param>
    /// <returns>Notification ID</returns>
    Task<int> SendPushNotificationAsync(int userId, string title, string message, Dictionary<string, string>? data = null);

    /// <summary>
    /// Send push notification to multiple users
    /// </summary>
    /// <param name="userIds">List of user IDs</param>
    /// <param name="title">Notification title</param>
    /// <param name="message">Notification message</param>
    /// <param name="data">Optional additional data payload</param>
    /// <returns>List of notification IDs</returns>
    Task<List<int>> SendBulkPushNotificationAsync(List<int> userIds, string title, string message, Dictionary<string, string>? data = null);

    /// <summary>
    /// Send push notification to user group/segment
    /// </summary>
    /// <param name="segment">User segment (e.g., "instructors", "students", "admins")</param>
    /// <param name="title">Notification title</param>
    /// <param name="message">Notification message</param>
    /// <param name="data">Optional additional data payload</param>
    /// <returns>Count of notifications sent</returns>
    Task<int> SendSegmentPushNotificationAsync(string segment, string title, string message, Dictionary<string, string>? data = null);

    // Template Management Methods
    /// <summary>
    /// Create new notification template
    /// </summary>
    /// <param name="request">Template creation request with name, type, and content</param>
    /// <returns>Created template DTO</returns>
    Task<NotificationTemplateDto> CreateTemplateAsync(CreateTemplateRequestDto request);

    /// <summary>
    /// Update existing notification template
    /// </summary>
    /// <param name="templateId">Template ID to update</param>
    /// <param name="request">Updated template content</param>
    /// <returns>Updated template DTO</returns>
    Task<NotificationTemplateDto> UpdateTemplateAsync(string templateId, UpdateTemplateRequestDto request);

    /// <summary>
    /// Delete notification template
    /// </summary>
    /// <param name="templateId">Template ID to delete</param>
    /// <returns>Success flag</returns>
    Task<bool> DeleteTemplateAsync(string templateId);

    /// <summary>
    /// Get all notification templates
    /// </summary>
    /// <returns>List of template DTOs</returns>
    Task<IEnumerable<NotificationTemplateDto>> GetAllTemplatesAsync();

    /// <summary>
    /// Get template by ID
    /// </summary>
    /// <param name="templateId">Template ID</param>
    /// <returns>Template DTO</returns>
    Task<NotificationTemplateDto> GetTemplateAsync(string templateId);

    // Notification History & Status Methods
    /// <summary>
    /// Get notification history for user
    /// </summary>
    /// <param name="userId">User ID</param>
    /// <param name="limit">Maximum number of notifications to return</param>
    /// <param name="offset">Number of notifications to skip</param>
    /// <returns>List of notification DTOs</returns>
    Task<IEnumerable<NotificationDto>> GetUserNotificationsAsync(int userId, int limit = 50, int offset = 0);

    /// <summary>
    /// Get notification status
    /// </summary>
    /// <param name="notificationId">Notification ID</param>
    /// <returns>Notification status DTO</returns>
    Task<NotificationStatusDto> GetNotificationStatusAsync(int notificationId);

    /// <summary>
    /// Mark notification as read
    /// </summary>
    /// <param name="notificationId">Notification ID</param>
    /// <returns>Success flag</returns>
    Task<bool> MarkAsReadAsync(int notificationId);

    /// <summary>
    /// Mark all notifications as read for user
    /// </summary>
    /// <param name="userId">User ID</param>
    /// <returns>Count of notifications marked as read</returns>
    Task<int> MarkAllAsReadAsync(int userId);

    /// <summary>
    /// Delete notification
    /// </summary>
    /// <param name="notificationId">Notification ID</param>
    /// <returns>Success flag</returns>
    Task<bool> DeleteNotificationAsync(int notificationId);

    /// <summary>
    /// Get unread notification count for user
    /// </summary>
    /// <param name="userId">User ID</param>
    /// <returns>Count of unread notifications</returns>
    Task<int> GetUnreadCountAsync(int userId);

    // User Preference Methods
    /// <summary>
    /// Get notification preferences for user
    /// </summary>
    /// <param name="userId">User ID</param>
    /// <returns>User notification preferences DTO</returns>
    Task<NotificationPreferenceDto> GetUserPreferencesAsync(int userId);

    /// <summary>
    /// Update notification preferences for user
    /// </summary>
    /// <param name="userId">User ID</param>
    /// <param name="preferences">Updated preferences DTO</param>
    /// <returns>Updated preferences DTO</returns>
    Task<NotificationPreferenceDto> UpdateUserPreferencesAsync(int userId, NotificationPreferenceDto preferences);

    /// <summary>
    /// Subscribe user to notification channel
    /// </summary>
    /// <param name="userId">User ID</param>
    /// <param name="channel">Channel type (email, sms, push)</param>
    /// <returns>Success flag</returns>
    Task<bool> SubscribeToChannelAsync(int userId, string channel);

    /// <summary>
    /// Unsubscribe user from notification channel
    /// </summary>
    /// <param name="userId">User ID</param>
    /// <param name="channel">Channel type (email, sms, push)</param>
    /// <returns>Success flag</returns>
    Task<bool> UnsubscribeFromChannelAsync(int userId, string channel);

    // Scheduled & Recurring Notifications
    /// <summary>
    /// Schedule notification for specific datetime
    /// </summary>
    /// <param name="request">Scheduled notification request</param>
    /// <returns>Schedule ID</returns>
    Task<int> ScheduleNotificationAsync(ScheduleNotificationRequestDto request);

    /// <summary>
    /// Get scheduled notifications
    /// </summary>
    /// <returns>List of scheduled notifications</returns>
    Task<IEnumerable<ScheduledNotificationDto>> GetScheduledNotificationsAsync();

    /// <summary>
    /// Cancel scheduled notification
    /// </summary>
    /// <param name="scheduleId">Schedule ID</param>
    /// <returns>Success flag</returns>
    Task<bool> CancelScheduledNotificationAsync(int scheduleId);

    // Notification Analytics
    /// <summary>
    /// Get notification delivery statistics
    /// </summary>
    /// <param name="startDate">Start date for analytics</param>
    /// <param name="endDate">End date for analytics</param>
    /// <returns>Notification statistics DTO</returns>
    Task<NotificationStatisticsDto> GetStatisticsAsync(DateTime startDate, DateTime endDate);

    /// <summary>
    /// Get template performance metrics
    /// </summary>
    /// <param name="templateId">Template ID</param>
    /// <returns>Template performance metrics</returns>
    Task<TemplateMetricsDto> GetTemplateMetricsAsync(string templateId);

    // Notification Type Methods
    /// <summary>
    /// Send student enrollment confirmation
    /// </summary>
    /// <param name="studentId">Student ID</param>
    /// <param name="courseId">Course ID</param>
    /// <returns>Notification ID</returns>
    Task<int> SendEnrollmentConfirmationAsync(int studentId, int courseId);

    /// <summary>
    /// Send payment receipt notification
    /// </summary>
    /// <param name="studentId">Student ID</param>
    /// <param name="paymentId">Payment ID</param>
    /// <returns>Notification ID</returns>
    Task<int> SendPaymentReceiptAsync(int studentId, int paymentId);

    /// <summary>
    /// Send grade notification
    /// </summary>
    /// <param name="studentId">Student ID</param>
    /// <param name="gradeId">Grade ID</param>
    /// <returns>Notification ID</returns>
    Task<int> SendGradeNotificationAsync(int studentId, int gradeId);

    /// <summary>
    /// Send course announcement
    /// </summary>
    /// <param name="courseId">Course ID</param>
    /// <param name="announcement">Announcement text</param>
    /// <returns>Count of notifications sent</returns>
    Task<int> SendCourseAnnouncementAsync(int courseId, string announcement);

    /// <summary>
    /// Send class reminder notification
    /// </summary>
    /// <param name="courseId">Course ID</param>
    /// <param name="daysAhead">Number of days ahead to send reminder (1, 3, 7)</param>
    /// <returns>Count of reminders sent</returns>
    Task<int> SendClassReminderAsync(int courseId, int daysAhead);

    /// <summary>
    /// Send password reset link
    /// </summary>
    /// <param name="userId">User ID</param>
    /// <param name="resetLink">Password reset URL</param>
    /// <returns>Notification ID</returns>
    Task<int> SendPasswordResetAsync(int userId, string resetLink);

    /// <summary>
    /// Send account verification email
    /// </summary>
    /// <param name="userId">User ID</param>
    /// <param name="verificationLink">Account verification URL</param>
    /// <returns>Notification ID</returns>
    Task<int> SendAccountVerificationAsync(int userId, string verificationLink);

    /// <summary>
    /// Send system maintenance notification
    /// </summary>
    /// <param name="maintenanceInfo">Maintenance details</param>
    /// <returns>Count of notifications sent</returns>
    Task<int> SendMaintenanceNotificationAsync(SystemMaintenanceDto maintenanceInfo);

    /// <summary>
    /// Send payment reminder notification
    /// </summary>
    /// <param name="studentId">Student ID</param>
    /// <param name="daysOverdue">Number of days overdue</param>
    /// <returns>Notification ID</returns>
    Task<int> SendPaymentReminderAsync(int studentId, int daysOverdue);
}
