namespace EnglishTrainingCenter.Application.DTOs.Notifications;

/// <summary>
/// Base notification DTO
/// </summary>
public class NotificationDto
{
    /// <summary>
    /// Unique notification identifier
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// User ID who receives the notification
    /// </summary>
    public int UserId { get; set; }

    /// <summary>
    /// Notification title
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Notification message content
    /// </summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// Notification type (Email, SMS, Push)
    /// </summary>
    public string Type { get; set; } = string.Empty;

    /// <summary>
    /// Whether notification has been read
    /// </summary>
    public bool IsRead { get; set; }

    /// <summary>
    /// When notification was created
    /// </summary>
    public DateTime CreatedDate { get; set; }

    /// <summary>
    /// When notification was read (null if unread)
    /// </summary>
    public DateTime? ReadDate { get; set; }

    /// <summary>
    /// Additional data payload
    /// </summary>
    public Dictionary<string, string>? Data { get; set; }

    /// <summary>
    /// Related resource ID (e.g., CourseId, PaymentId)
    /// </summary>
    public int? ResourceId { get; set; }

    /// <summary>
    /// Related resource type
    /// </summary>
    public string? ResourceType { get; set; }
}

/// <summary>
/// Email notification DTO
/// </summary>
public class EmailNotificationDto
{
    /// <summary>
    /// Notification ID
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Recipient email address
    /// </summary>
    public string RecipientEmail { get; set; } = string.Empty;

    /// <summary>
    /// Email subject
    /// </summary>
    public string Subject { get; set; } = string.Empty;

    /// <summary>
    /// Email body (plain text or HTML)
    /// </summary>
    public string Body { get; set; } = string.Empty;

    /// <summary>
    /// Whether body is HTML formatted
    /// </summary>
    public bool IsHtml { get; set; }

    /// <summary>
    /// Email status (Pending, Sent, Failed, Bounced)
    /// </summary>
    public string Status { get; set; } = "Pending";

    /// <summary>
    /// Email sending timestamp
    /// </summary>
    public DateTime? SentDate { get; set; }

    /// <summary>
    /// Email delivery timestamp
    /// </summary>
    public DateTime? DeliveredDate { get; set; }

    /// <summary>
    /// Email provider reference ID
    /// </summary>
    public string? ProviderMessageId { get; set; }

    /// <summary>
    /// Error message if email failed
    /// </summary>
    public string? ErrorMessage { get; set; }

    /// <summary>
    /// List of attachment file paths
    /// </summary>
    public List<string>? Attachments { get; set; }

    /// <summary>
    /// Email open count (if tracking enabled)
    /// </summary>
    public int OpenCount { get; set; }

    /// <summary>
    /// Email click count (if tracking enabled)
    /// </summary>
    public int ClickCount { get; set; }
}

/// <summary>
/// SMS notification DTO
/// </summary>
public class SmsNotificationDto
{
    /// <summary>
    /// Notification ID
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Recipient phone number (E.164 format)
    /// </summary>
    public string PhoneNumber { get; set; } = string.Empty;

    /// <summary>
    /// SMS message content
    /// </summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// SMS status (Pending, Sent, Failed, Delivered)
    /// </summary>
    public string Status { get; set; } = "Pending";

    /// <summary>
    /// SMS sending timestamp
    /// </summary>
    public DateTime? SentDate { get; set; }

    /// <summary>
    /// SMS delivery timestamp
    /// </summary>
    public DateTime? DeliveredDate { get; set; }

    /// <summary>
    /// SMS provider reference ID
    /// </summary>
    public string? ProviderMessageId { get; set; }

    /// <summary>
    /// Error message if SMS failed
    /// </summary>
    public string? ErrorMessage { get; set; }

    /// <summary>
    /// Number of SMS segments required
    /// </summary>
    public int SegmentCount { get; set; }

    /// <summary>
    /// SMS cost in currency units
    /// </summary>
    public decimal Cost { get; set; }
}

/// <summary>
/// Push notification DTO
/// </summary>
public class PushNotificationDto
{
    /// <summary>
    /// Notification ID
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// User ID to send notification to
    /// </summary>
    public int UserId { get; set; }

    /// <summary>
    /// Notification title
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Notification message
    /// </summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// Push notification status (Pending, Sent, Delivered, Viewed)
    /// </summary>
    public string Status { get; set; } = "Pending";

    /// <summary>
    /// Push sending timestamp
    /// </summary>
    public DateTime? SentDate { get; set; }

    /// <summary>
    /// Push delivery timestamp
    /// </summary>
    public DateTime? DeliveredDate { get; set; }

    /// <summary>
    /// Push view/open timestamp
    /// </summary>
    public DateTime? ViewedDate { get; set; }

    /// <summary>
    /// Additional data payload for the app
    /// </summary>
    public Dictionary<string, string>? Data { get; set; }

    /// <summary>
    /// Deep link URL (optional)
    /// </summary>
    public string? DeepLink { get; set; }

    /// <summary>
    /// Platform (iOS, Android, Web)
    /// </summary>
    public string? Platform { get; set; }

    /// <summary>
    /// Device token
    /// </summary>
    public string? DeviceToken { get; set; }
}

/// <summary>
/// Notification template DTO
/// </summary>
public class NotificationTemplateDto
{
    /// <summary>
    /// Template unique identifier
    /// </summary>
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// Template name
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Template description
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Notification type (Email, SMS, Push)
    /// </summary>
    public string Type { get; set; } = string.Empty;

    /// <summary>
    /// Template subject (for email templates)
    /// </summary>
    public string? Subject { get; set; }

    /// <summary>
    /// Template content with {placeholder} syntax
    /// </summary>
    public string Content { get; set; } = string.Empty;

    /// <summary>
    /// List of available placeholders
    /// </summary>
    public List<string>? Placeholders { get; set; }

    /// <summary>
    /// Template status (Active, Inactive, Draft)
    /// </summary>
    public string Status { get; set; } = "Active";

    /// <summary>
    /// Template category
    /// </summary>
    public string Category { get; set; } = string.Empty;

    /// <summary>
    /// Whether template is system-defined
    /// </summary>
    public bool IsSystem { get; set; }

    /// <summary>
    /// Template creation date
    /// </summary>
    public DateTime CreatedDate { get; set; }

    /// <summary>
    /// Template last update date
    /// </summary>
    public DateTime? UpdatedDate { get; set; }

    /// <summary>
    /// Number of times template was used
    /// </summary>
    public int UsageCount { get; set; }
}

/// <summary>
/// Create notification template request
/// </summary>
public class CreateTemplateRequestDto
{
    /// <summary>
    /// Template name
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Template description
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Notification type (Email, SMS, Push)
    /// </summary>
    public string Type { get; set; } = string.Empty;

    /// <summary>
    /// Template subject (for email)
    /// </summary>
    public string? Subject { get; set; }

    /// <summary>
    /// Template content
    /// </summary>
    public string Content { get; set; } = string.Empty;

    /// <summary>
    /// Template category
    /// </summary>
    public string Category { get; set; } = string.Empty;
}

/// <summary>
/// Update notification template request
/// </summary>
public class UpdateTemplateRequestDto
{
    /// <summary>
    /// Template name
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Template description
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Template subject (for email)
    /// </summary>
    public string? Subject { get; set; }

    /// <summary>
    /// Template content
    /// </summary>
    public string? Content { get; set; }

    /// <summary>
    /// Template status
    /// </summary>
    public string? Status { get; set; }
}

/// <summary>
/// Notification preferences DTO
/// </summary>
public class NotificationPreferenceDto
{
    /// <summary>
    /// User ID
    /// </summary>
    public int UserId { get; set; }

    /// <summary>
    /// Whether email notifications are enabled
    /// </summary>
    public bool EmailNotifications { get; set; } = true;

    /// <summary>
    /// Whether SMS notifications are enabled
    /// </summary>
    public bool SmsNotifications { get; set; } = true;

    /// <summary>
    /// Whether push notifications are enabled
    /// </summary>
    public bool PushNotifications { get; set; } = true;

    /// <summary>
    /// Email address for notifications
    /// </summary>
    public string? NotificationEmail { get; set; }

    /// <summary>
    /// Phone number for SMS notifications (E.164 format)
    /// </summary>
    public string? NotificationPhone { get; set; }

    /// <summary>
    /// Whether to receive enrollment notifications
    /// </summary>
    public bool EnrollmentNotifications { get; set; } = true;

    /// <summary>
    /// Whether to receive grade notifications
    /// </summary>
    public bool GradeNotifications { get; set; } = true;

    /// <summary>
    /// Whether to receive payment notifications
    /// </summary>
    public bool PaymentNotifications { get; set; } = true;

    /// <summary>
    /// Whether to receive course announcements
    /// </summary>
    public bool CourseAnnouncementNotifications { get; set; } = true;

    /// <summary>
    /// Whether to receive class reminders
    /// </summary>
    public bool ClassReminderNotifications { get; set; } = true;

    /// <summary>
    /// Whether to receive promotional emails
    /// </summary>
    public bool PromotionalEmails { get; set; } = false;

    /// <summary>
    /// Whether to receive weekly digest
    /// </summary>
    public bool WeeklyDigest { get; set; } = false;

    /// <summary>
    /// Quiet hours start time (HH:mm format)
    /// </summary>
    public string? QuietHoursStart { get; set; }

    /// <summary>
    /// Quiet hours end time (HH:mm format)
    /// </summary>
    public string? QuietHoursEnd { get; set; }

    /// <summary>
    /// Timezone for quiet hours
    /// </summary>
    public string? TimeZone { get; set; }

    /// <summary>
    /// Preferred notification language
    /// </summary>
    public string Language { get; set; } = "en";

    /// <summary>
    /// Last preference update date
    /// </summary>
    public DateTime UpdatedDate { get; set; }
}

/// <summary>
/// Scheduled notification request
/// </summary>
public class ScheduleNotificationRequestDto
{
    /// <summary>
    /// Recipient user ID or email/phone for bulk
    /// </summary>
    public string Recipient { get; set; } = string.Empty;

    /// <summary>
    /// Notification type (Email, SMS, Push)
    /// </summary>
    public string Type { get; set; } = string.Empty;

    /// <summary>
    /// Notification title/subject
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Notification message
    /// </summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// Scheduled send date/time (UTC)
    /// </summary>
    public DateTime ScheduledTime { get; set; }

    /// <summary>
    /// Recurrence pattern (Once, Daily, Weekly, Monthly)
    /// </summary>
    public string Recurrence { get; set; } = "Once";

    /// <summary>
    /// Recurrence end date (if recurring)
    /// </summary>
    public DateTime? RecurrenceEndDate { get; set; }

    /// <summary>
    /// Template ID if using template
    /// </summary>
    public string? TemplateId { get; set; }

    /// <summary>
    /// Template data for placeholders
    /// </summary>
    public Dictionary<string, string>? TemplateData { get; set; }

    /// <summary>
    /// Additional data payload
    /// </summary>
    public Dictionary<string, string>? Data { get; set; }
}

/// <summary>
/// Scheduled notification DTO
/// </summary>
public class ScheduledNotificationDto
{
    /// <summary>
    /// Schedule unique identifier
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Recipient user ID
    /// </summary>
    public int? UserId { get; set; }

    /// <summary>
    /// Recipient email or phone
    /// </summary>
    public string? Recipient { get; set; }

    /// <summary>
    /// Notification type
    /// </summary>
    public string Type { get; set; } = string.Empty;

    /// <summary>
    /// Notification title
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Notification message
    /// </summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// Next scheduled send time
    /// </summary>
    public DateTime NextScheduledTime { get; set; }

    /// <summary>
    /// Last sent timestamp (if recurring)
    /// </summary>
    public DateTime? LastSentTime { get; set; }

    /// <summary>
    /// Recurrence pattern
    /// </summary>
    public string Recurrence { get; set; } = "Once";

    /// <summary>
    /// Recurrence end date
    /// </summary>
    public DateTime? RecurrenceEndDate { get; set; }

    /// <summary>
    /// Whether schedule is active
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// Number of times notification has been sent
    /// </summary>
    public int SendCount { get; set; }

    /// <summary>
    /// Schedule creation date
    /// </summary>
    public DateTime CreatedDate { get; set; }
}

/// <summary>
/// Notification status DTO
/// </summary>
public class NotificationStatusDto
{
    /// <summary>
    /// Notification ID
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Current status (Pending, Sent, Delivered, Failed, Bounced)
    /// </summary>
    public string Status { get; set; } = string.Empty;

    /// <summary>
    /// Current status timestamp
    /// </summary>
    public DateTime StatusDate { get; set; }

    /// <summary>
    /// Status history with timestamps
    /// </summary>
    public List<StatusHistoryDto>? History { get; set; }

    /// <summary>
    /// Last status update message
    /// </summary>
    public string? StatusMessage { get; set; }

    /// <summary>
    /// Delivery attempts count
    /// </summary>
    public int RetryCount { get; set; }

    /// <summary>
    /// Next retry time (if applicable)
    /// </summary>
    public DateTime? NextRetryTime { get; set; }
}

/// <summary>
/// Status history entry for notifications
/// </summary>
public class StatusHistoryDto
{
    /// <summary>
    /// Status at this point in time
    /// </summary>
    public string Status { get; set; } = string.Empty;

    /// <summary>
    /// When status changed
    /// </summary>
    public DateTime Timestamp { get; set; }

    /// <summary>
    /// Additional status details
    /// </summary>
    public string? Details { get; set; }
}

/// <summary>
/// Notification statistics DTO
/// </summary>
public class NotificationStatisticsDto
{
    /// <summary>
    /// Report period start
    /// </summary>
    public DateTime StartDate { get; set; }

    /// <summary>
    /// Report period end
    /// </summary>
    public DateTime EndDate { get; set; }

    /// <summary>
    /// Total notifications sent
    /// </summary>
    public int TotalSent { get; set; }

    /// <summary>
    /// Total emails sent
    /// </summary>
    public int EmailsSent { get; set; }

    /// <summary>
    /// Total SMS sent
    /// </summary>
    public int SmsSent { get; set; }

    /// <summary>
    /// Total push notifications sent
    /// </summary>
    public int PushSent { get; set; }

    /// <summary>
    /// Total successfully delivered
    /// </summary>
    public int Delivered { get; set; }

    /// <summary>
    /// Total failed notifications
    /// </summary>
    public int Failed { get; set; }

    /// <summary>
    /// Total bounced emails
    /// </summary>
    public int Bounced { get; set; }

    /// <summary>
    /// Email open rate percentage
    /// </summary>
    public decimal EmailOpenRate { get; set; }

    /// <summary>
    /// Email click rate percentage
    /// </summary>
    public decimal EmailClickRate { get; set; }

    /// <summary>
    /// Push notification view rate percentage
    /// </summary>
    public decimal PushViewRate { get; set; }

    /// <summary>
    /// SMS delivery rate percentage
    /// </summary>
    public decimal SmsDeliveryRate { get; set; }

    /// <summary>
    /// Total cost (if SMS used)
    /// </summary>
    public decimal TotalCost { get; set; }

    /// <summary>
    /// Breakdown by type
    /// </summary>
    public Dictionary<string, int>? ByType { get; set; }

    /// <summary>
    /// Breakdown by status
    /// </summary>
    public Dictionary<string, int>? ByStatus { get; set; }
}

/// <summary>
/// Template performance metrics DTO
/// </summary>
public class TemplateMetricsDto
{
    /// <summary>
    /// Template ID
    /// </summary>
    public string TemplateId { get; set; } = string.Empty;

    /// <summary>
    /// Template name
    /// </summary>
    public string TemplateName { get; set; } = string.Empty;

    /// <summary>
    /// Total notifications sent using template
    /// </summary>
    public int TotalSent { get; set; }

    /// <summary>
    /// Total successfully delivered
    /// </summary>
    public int Delivered { get; set; }

    /// <summary>
    /// Delivery rate percentage
    /// </summary>
    public decimal DeliveryRate { get; set; }

    /// <summary>
    /// Email open rate (if email template)
    /// </summary>
    public decimal? OpenRate { get; set; }

    /// <summary>
    /// Email click rate (if email template)
    /// </summary>
    public decimal? ClickRate { get; set; }

    /// <summary>
    /// Average time to open (minutes)
    /// </summary>
    public decimal? AvgTimeToOpen { get; set; }

    /// <summary>
    /// Bounce rate
    /// </summary>
    public decimal? BounceRate { get; set; }

    /// <summary>
    /// User feedback score (0-5)
    /// </summary>
    public decimal? FeedbackScore { get; set; }

    /// <summary>
    /// Number of conversions (if tracked)
    /// </summary>
    public int? Conversions { get; set; }

    /// <summary>
    /// Last used date
    /// </summary>
    public DateTime? LastUsedDate { get; set; }
}

/// <summary>
/// System maintenance notification DTO
/// </summary>
public class SystemMaintenanceDto
{
    /// <summary>
    /// Maintenance title
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Maintenance description
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Maintenance start time
    /// </summary>
    public DateTime StartTime { get; set; }

    /// <summary>
    /// Expected maintenance duration (hours)
    /// </summary>
    public decimal DurationHours { get; set; }

    /// <summary>
    /// Affected services/modules
    /// </summary>
    public List<string>? AffectedServices { get; set; }

    /// <summary>
    /// Maintenance priority (Low, Medium, High, Critical)
    /// </summary>
    public string Priority { get; set; } = "Medium";

    /// <summary>
    /// Contact email for maintenance issues
    /// </summary>
    public string? ContactEmail { get; set; }
}

/// <summary>
/// Send notification request DTO
/// </summary>
public class SendNotificationRequestDto
{
    /// <summary>
    /// Recipient user ID or email/phone
    /// </summary>
    public string Recipient { get; set; } = string.Empty;

    /// <summary>
    /// Notification type (Email, SMS, Push)
    /// </summary>
    public string Type { get; set; } = string.Empty;

    /// <summary>
    /// Notification title/subject
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Notification message
    /// </summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// Template ID (if using template instead of message)
    /// </summary>
    public string? TemplateId { get; set; }

    /// <summary>
    /// Template data for placeholders
    /// </summary>
    public Dictionary<string, string>? TemplateData { get; set; }

    /// <summary>
    /// Whether to send immediately or schedule
    /// </summary>
    public bool SendImmediately { get; set; } = true;

    /// <summary>
    /// Scheduled time if not sending immediately
    /// </summary>
    public DateTime? ScheduledTime { get; set; }

    /// <summary>
    /// Additional data payload
    /// </summary>
    public Dictionary<string, string>? Data { get; set; }

    /// <summary>
    /// List of attachments (email only)
    /// </summary>
    public List<string>? Attachments { get; set; }

    /// <summary>
    /// Priority level (Normal, High, Critical)
    /// </summary>
    public string Priority { get; set; } = "Normal";
}
