using EnglishTrainingCenter.Application.DTOs.Notifications;
using EnglishTrainingCenter.Application.Services.Notifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EnglishTrainingCenter.API.Controllers;

/// <summary>
/// Controller for managing notifications, emails, SMS, and push notifications
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class NotificationsController : ControllerBase
{
    private readonly INotificationService _notificationService;
    private readonly ILogger<NotificationsController> _logger;

    public NotificationsController(INotificationService notificationService, ILogger<NotificationsController> logger)
    {
        _notificationService = notificationService;
        _logger = logger;
    }

    #region Email Endpoints

    /// <summary>
    /// Send email notification
    /// </summary>
    [HttpPost("email/send")]
    [ProduceResponseType(typeof(int), StatusCodes.Status201Created)]
    public async Task<IActionResult> SendEmail([FromBody] SendNotificationRequestDto request)
    {
        try
        {
            _logger.LogInformation($"Sending email to {request.Recipient}");

            int notificationId;
            if (!string.IsNullOrEmpty(request.TemplateId))
            {
                notificationId = await _notificationService.SendEmailFromTemplateAsync(
                    request.Recipient,
                    request.TemplateId,
                    request.TemplateData ?? new Dictionary<string, string>()
                );
            }
            else
            {
                notificationId = await _notificationService.SendEmailAsync(
                    request.Recipient,
                    request.Title,
                    request.Message
                );
            }

            return CreatedAtAction(nameof(GetNotificationStatus), new { id = notificationId }, new { notificationId });
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error sending email: {ex.Message}");
            return BadRequest(new { message = "Error sending email", error = ex.Message });
        }
    }

    /// <summary>
    /// Send HTML email
    /// </summary>
    [HttpPost("email/send-html")]
    [ProduceResponseType(typeof(int), StatusCodes.Status201Created)]
    public async Task<IActionResult> SendHtmlEmail([FromBody] SendNotificationRequestDto request)
    {
        try
        {
            _logger.LogInformation($"Sending HTML email to {request.Recipient}");

            var notificationId = await _notificationService.SendHtmlEmailAsync(
                request.Recipient,
                request.Title,
                request.Message,
                request.Attachments
            );

            return CreatedAtAction(nameof(GetNotificationStatus), new { id = notificationId }, new { notificationId });
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error sending HTML email: {ex.Message}");
            return BadRequest(new { message = "Error sending HTML email", error = ex.Message });
        }
    }

    /// <summary>
    /// Send bulk email
    /// </summary>
    [HttpPost("email/send-bulk")]
    [ProduceResponseType(typeof(List<int>), StatusCodes.Status201Created)]
    public async Task<IActionResult> SendBulkEmail([FromBody] BulkEmailRequestDto request)
    {
        try
        {
            _logger.LogInformation($"Sending bulk email to {request.Recipients.Count} recipients");

            var notificationIds = await _notificationService.SendBulkEmailAsync(
                request.Recipients,
                request.Title,
                request.Message
            );

            return CreatedAtAction(nameof(GetNotificationStatus), new { }, new { count = notificationIds.Count, notificationIds });
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error sending bulk email: {ex.Message}");
            return BadRequest(new { message = "Error sending bulk email", error = ex.Message });
        }
    }

    #endregion

    #region SMS Endpoints

    /// <summary>
    /// Send SMS notification
    /// </summary>
    [HttpPost("sms/send")]
    [ProduceResponseType(typeof(int), StatusCodes.Status201Created)]
    public async Task<IActionResult> SendSms([FromBody] SendSmsRequestDto request)
    {
        try
        {
            _logger.LogInformation($"Sending SMS to {request.PhoneNumber}");

            int notificationId;
            if (!string.IsNullOrEmpty(request.TemplateId))
            {
                notificationId = await _notificationService.SendSmsFromTemplateAsync(
                    request.PhoneNumber,
                    request.TemplateId,
                    request.TemplateData ?? new Dictionary<string, string>()
                );
            }
            else
            {
                notificationId = await _notificationService.SendSmsAsync(request.PhoneNumber, request.Message);
            }

            return CreatedAtAction(nameof(GetNotificationStatus), new { id = notificationId }, new { notificationId });
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error sending SMS: {ex.Message}");
            return BadRequest(new { message = "Error sending SMS", error = ex.Message });
        }
    }

    /// <summary>
    /// Send bulk SMS
    /// </summary>
    [HttpPost("sms/send-bulk")]
    [ProduceResponseType(typeof(List<int>), StatusCodes.Status201Created)]
    public async Task<IActionResult> SendBulkSms([FromBody] BulkSmsRequestDto request)
    {
        try
        {
            _logger.LogInformation($"Sending bulk SMS to {request.PhoneNumbers.Count} recipients");

            var notificationIds = await _notificationService.SendBulkSmsAsync(request.PhoneNumbers, request.Message);

            return CreatedAtAction(nameof(GetNotificationStatus), new { }, new { count = notificationIds.Count, notificationIds });
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error sending bulk SMS: {ex.Message}");
            return BadRequest(new { message = "Error sending bulk SMS", error = ex.Message });
        }
    }

    #endregion

    #region Push Notification Endpoints

    /// <summary>
    /// Send push notification
    /// </summary>
    [HttpPost("push/send")]
    [ProduceResponseType(typeof(int), StatusCodes.Status201Created)]
    public async Task<IActionResult> SendPush([FromBody] SendPushNotificationRequestDto request)
    {
        try
        {
            _logger.LogInformation($"Sending push notification to user {request.UserId}");

            var notificationId = await _notificationService.SendPushNotificationAsync(
                request.UserId,
                request.Title,
                request.Message,
                request.Data
            );

            return CreatedAtAction(nameof(GetNotificationStatus), new { id = notificationId }, new { notificationId });
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error sending push notification: {ex.Message}");
            return BadRequest(new { message = "Error sending push notification", error = ex.Message });
        }
    }

    /// <summary>
    /// Send push notification to segment
    /// </summary>
    [HttpPost("push/send-segment")]
    [ProduceResponseType(typeof(int), StatusCodes.Status201Created)]
    public async Task<IActionResult> SendSegmentPush([FromBody] SendSegmentPushRequestDto request)
    {
        try
        {
            _logger.LogInformation($"Sending push notification to segment: {request.Segment}");

            var count = await _notificationService.SendSegmentPushNotificationAsync(
                request.Segment,
                request.Title,
                request.Message,
                request.Data
            );

            return CreatedAtAction(nameof(GetNotificationStatus), new { }, new { count });
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error sending segment push notification: {ex.Message}");
            return BadRequest(new { message = "Error sending push notification", error = ex.Message });
        }
    }

    #endregion

    #region Template Management Endpoints

    /// <summary>
    /// Create notification template
    /// </summary>
    [HttpPost("templates")]
    [ProduceResponseType(typeof(NotificationTemplateDto), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateTemplate([FromBody] CreateTemplateRequestDto request)
    {
        try
        {
            _logger.LogInformation($"Creating template: {request.Name}");

            var template = await _notificationService.CreateTemplateAsync(request);
            return CreatedAtAction(nameof(GetTemplate), new { templateId = template.Id }, template);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error creating template: {ex.Message}");
            return BadRequest(new { message = "Error creating template", error = ex.Message });
        }
    }

    /// <summary>
    /// Get all templates
    /// </summary>
    [HttpGet("templates")]
    [ProduceResponseType(typeof(IEnumerable<NotificationTemplateDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetTemplates()
    {
        try
        {
            _logger.LogInformation("Retrieving all templates");

            var templates = await _notificationService.GetAllTemplatesAsync();
            return Ok(templates);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error retrieving templates: {ex.Message}");
            return BadRequest(new { message = "Error retrieving templates", error = ex.Message });
        }
    }

    /// <summary>
    /// Get template by ID
    /// </summary>
    [HttpGet("templates/{templateId}")]
    [ProduceResponseType(typeof(NotificationTemplateDto), StatusCodes.Status200OK)]
    [ProduceResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetTemplate([FromRoute] string templateId)
    {
        try
        {
            _logger.LogInformation($"Retrieving template {templateId}");

            var template = await _notificationService.GetTemplateAsync(templateId);
            return Ok(template);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error retrieving template: {ex.Message}");
            return NotFound(new { message = "Template not found" });
        }
    }

    /// <summary>
    /// Update template
    /// </summary>
    [HttpPut("templates/{templateId}")]
    [ProduceResponseType(typeof(NotificationTemplateDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateTemplate([FromRoute] string templateId, [FromBody] UpdateTemplateRequestDto request)
    {
        try
        {
            _logger.LogInformation($"Updating template {templateId}");

            var template = await _notificationService.UpdateTemplateAsync(templateId, request);
            return Ok(template);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error updating template: {ex.Message}");
            return BadRequest(new { message = "Error updating template", error = ex.Message });
        }
    }

    /// <summary>
    /// Delete template
    /// </summary>
    [HttpDelete("templates/{templateId}")]
    [ProduceResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteTemplate([FromRoute] string templateId)
    {
        try
        {
            _logger.LogInformation($"Deleting template {templateId}");

            var success = await _notificationService.DeleteTemplateAsync(templateId);
            if (!success)
                return NotFound(new { message = "Template not found" });

            return Ok(new { message = "Template deleted successfully" });
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error deleting template: {ex.Message}");
            return BadRequest(new { message = "Error deleting template", error = ex.Message });
        }
    }

    #endregion

    #region Notification Status & History Endpoints

    /// <summary>
    /// Get notification status
    /// </summary>
    [HttpGet("status/{notificationId}")]
    [ProduceResponseType(typeof(NotificationStatusDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetNotificationStatus([FromRoute] int notificationId)
    {
        try
        {
            _logger.LogInformation($"Getting status for notification {notificationId}");

            var status = await _notificationService.GetNotificationStatusAsync(notificationId);
            return Ok(status);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error getting notification status: {ex.Message}");
            return BadRequest(new { message = "Error getting notification status", error = ex.Message });
        }
    }

    /// <summary>
    /// Mark notification as read
    /// </summary>
    [HttpPut("{notificationId}/mark-read")]
    [ProduceResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> MarkAsRead([FromRoute] int notificationId)
    {
        try
        {
            _logger.LogInformation($"Marking notification {notificationId} as read");

            var success = await _notificationService.MarkAsReadAsync(notificationId);
            return Ok(new { message = success ? "Notification marked as read" : "Failed to mark notification" });
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error marking notification as read: {ex.Message}");
            return BadRequest(new { message = "Error updating notification", error = ex.Message });
        }
    }

    /// <summary>
    /// Get user notifications
    /// </summary>
    [HttpGet("user/{userId}")]
    [ProduceResponseType(typeof(IEnumerable<NotificationDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetUserNotifications([FromRoute] int userId, [FromQuery] int limit = 50, [FromQuery] int offset = 0)
    {
        try
        {
            _logger.LogInformation($"Getting notifications for user {userId}");

            var notifications = await _notificationService.GetUserNotificationsAsync(userId, limit, offset);
            return Ok(notifications);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error getting user notifications: {ex.Message}");
            return BadRequest(new { message = "Error retrieving notifications", error = ex.Message });
        }
    }

    /// <summary>
    /// Get unread notification count
    /// </summary>
    [HttpGet("user/{userId}/unread-count")]
    [ProduceResponseType(typeof(int), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetUnreadCount([FromRoute] int userId)
    {
        try
        {
            _logger.LogInformation($"Getting unread count for user {userId}");

            var count = await _notificationService.GetUnreadCountAsync(userId);
            return Ok(new { unreadCount = count });
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error getting unread count: {ex.Message}");
            return BadRequest(new { message = "Error getting unread count", error = ex.Message });
        }
    }

    #endregion

    #region User Preference Endpoints

    /// <summary>
    /// Get notification preferences
    /// </summary>
    [HttpGet("preferences/{userId}")]
    [ProduceResponseType(typeof(NotificationPreferenceDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPreferences([FromRoute] int userId)
    {
        try
        {
            _logger.LogInformation($"Getting preferences for user {userId}");

            var preferences = await _notificationService.GetUserPreferencesAsync(userId);
            return Ok(preferences);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error getting preferences: {ex.Message}");
            return BadRequest(new { message = "Error retrieving preferences", error = ex.Message });
        }
    }

    /// <summary>
    /// Update notification preferences
    /// </summary>
    [HttpPut("preferences/{userId}")]
    [ProduceResponseType(typeof(NotificationPreferenceDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdatePreferences([FromRoute] int userId, [FromBody] NotificationPreferenceDto preferences)
    {
        try
        {
            _logger.LogInformation($"Updating preferences for user {userId}");

            preferences.UserId = userId;
            var updated = await _notificationService.UpdateUserPreferencesAsync(userId, preferences);
            return Ok(updated);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error updating preferences: {ex.Message}");
            return BadRequest(new { message = "Error updating preferences", error = ex.Message });
        }
    }

    #endregion

    #region Scheduling Endpoints

    /// <summary>
    /// Schedule notification
    /// </summary>
    [HttpPost("schedule")]
    [ProduceResponseType(typeof(int), StatusCodes.Status201Created)]
    public async Task<IActionResult> ScheduleNotification([FromBody] ScheduleNotificationRequestDto request)
    {
        try
        {
            _logger.LogInformation($"Scheduling notification for {request.ScheduledTime}");

            var scheduleId = await _notificationService.ScheduleNotificationAsync(request);
            return CreatedAtAction(nameof(GetScheduledNotifications), new { id = scheduleId }, new { scheduleId });
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error scheduling notification: {ex.Message}");
            return BadRequest(new { message = "Error scheduling notification", error = ex.Message });
        }
    }

    /// <summary>
    /// Get scheduled notifications
    /// </summary>
    [HttpGet("schedule")]
    [ProduceResponseType(typeof(IEnumerable<ScheduledNotificationDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetScheduledNotifications()
    {
        try
        {
            _logger.LogInformation("Retrieving scheduled notifications");

            var scheduled = await _notificationService.GetScheduledNotificationsAsync();
            return Ok(scheduled);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error retrieving scheduled notifications: {ex.Message}");
            return BadRequest(new { message = "Error retrieving scheduled notifications", error = ex.Message });
        }
    }

    /// <summary>
    /// Cancel scheduled notification
    /// </summary>
    [HttpDelete("schedule/{scheduleId}")]
    [ProduceResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> CancelScheduledNotification([FromRoute] int scheduleId)
    {
        try
        {
            _logger.LogInformation($"Canceling scheduled notification {scheduleId}");

            var success = await _notificationService.CancelScheduledNotificationAsync(scheduleId);
            if (!success)
                return NotFound(new { message = "Scheduled notification not found" });

            return Ok(new { message = "Scheduled notification canceled successfully" });
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error canceling scheduled notification: {ex.Message}");
            return BadRequest(new { message = "Error canceling notification", error = ex.Message });
        }
    }

    #endregion

    #region Analytics Endpoints

    /// <summary>
    /// Get notification statistics
    /// </summary>
    [HttpGet("analytics/statistics")]
    [ProduceResponseType(typeof(NotificationStatisticsDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetStatistics([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
    {
        try
        {
            _logger.LogInformation($"Getting statistics for {startDate} to {endDate}");

            var statistics = await _notificationService.GetStatisticsAsync(startDate, endDate);
            return Ok(statistics);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error getting statistics: {ex.Message}");
            return BadRequest(new { message = "Error getting statistics", error = ex.Message });
        }
    }

    /// <summary>
    /// Get template metrics
    /// </summary>
    [HttpGet("analytics/templates/{templateId}")]
    [ProduceResponseType(typeof(TemplateMetricsDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetTemplateMetrics([FromRoute] string templateId)
    {
        try
        {
            _logger.LogInformation($"Getting metrics for template {templateId}");

            var metrics = await _notificationService.GetTemplateMetricsAsync(templateId);
            return Ok(metrics);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error getting template metrics: {ex.Message}");
            return BadRequest(new { message = "Error getting metrics", error = ex.Message });
        }
    }

    #endregion
}

// Supporting DTOs for controller requests
public class BulkEmailRequestDto
{
    public List<string> Recipients { get; set; } = new();
    public string Title { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
}

public class SendSmsRequestDto
{
    public string PhoneNumber { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public string? TemplateId { get; set; }
    public Dictionary<string, string>? TemplateData { get; set; }
}

public class BulkSmsRequestDto
{
    public List<string> PhoneNumbers { get; set; } = new();
    public string Message { get; set; } = string.Empty;
}

public class SendPushNotificationRequestDto
{
    public int UserId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public Dictionary<string, string>? Data { get; set; }
}

public class SendSegmentPushRequestDto
{
    public string Segment { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public Dictionary<string, string>? Data { get; set; }
}
