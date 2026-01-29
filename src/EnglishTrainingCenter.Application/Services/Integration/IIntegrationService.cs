using EnglishTrainingCenter.Application.DTOs.Integration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EnglishTrainingCenter.Application.Services.Integration;

/// <summary>
/// Integration Service Interface
/// Provides methods for Google Calendar, Payment Gateway, Video Conferencing, OAuth, and Webhooks
/// </summary>
public interface IIntegrationService
{
    // ============================================================================
    // GOOGLE CALENDAR INTEGRATION METHODS (8 Methods)
    // ============================================================================

    /// <summary>
    /// Authenticate with Google Calendar and store credentials
    /// </summary>
    Task<GoogleCalendarAuthDto> AuthenticateGoogleCalendarAsync(string email, string authCode);

    /// <summary>
    /// Create event in Google Calendar
    /// </summary>
    Task<string> CreateCalendarEventAsync(int userId, CreateCalendarEventDto eventData);

    /// <summary>
    /// Sync course schedule to user's Google Calendar
    /// </summary>
    Task<int> SyncCourseScheduleAsync(int studentId, int courseId);

    /// <summary>
    /// Get upcoming calendar events for user
    /// </summary>
    Task<IEnumerable<CalendarEventDto>> GetUpcomingEventsAsync(int userId, int dayCount = 30);

    /// <summary>
    /// Sync class reminders to Google Calendar
    /// </summary>
    Task<bool> SyncClassRemindersAsync(int courseId);

    /// <summary>
    /// Get calendar sync status
    /// </summary>
    Task<GoogleCalendarSyncStatusDto> GetCalendarSyncStatusAsync(int userId);

    /// <summary>
    /// Revoke Google Calendar access
    /// </summary>
    Task<bool> RevokeGoogleCalendarAccessAsync(int userId);

    /// <summary>
    /// Update calendar event
    /// </summary>
    Task<bool> UpdateCalendarEventAsync(int userId, string eventId, UpdateCalendarEventDto eventData);

    // ============================================================================
    // PAYMENT GATEWAY INTEGRATION METHODS (10 Methods)
    // ============================================================================

    /// <summary>
    /// Initialize payment with Stripe
    /// </summary>
    Task<StripeInitiationDto> InitializeStripePaymentAsync(CreatePaymentDto paymentData);

    /// <summary>
    /// Process payment through PayPal
    /// </summary>
    Task<PayPalPaymentDto> ProcessPayPalPaymentAsync(CreatePaymentDto paymentData);

    /// <summary>
    /// Create payment intent for Stripe
    /// </summary>
    Task<string> CreateStripePaymentIntentAsync(int studentId, decimal amount, string courseName);

    /// <summary>
    /// Confirm payment and update student records
    /// </summary>
    Task<PaymentConfirmationDto> ConfirmPaymentAsync(string transactionId, string provider);

    /// <summary>
    /// Get payment status from Stripe
    /// </summary>
    Task<PaymentStatusDto> GetStripePaymentStatusAsync(string paymentIntentId);

    /// <summary>
    /// Process refund through payment gateway
    /// </summary>
    Task<RefundDto> ProcessRefundAsync(string transactionId, string provider, string reason);

    /// <summary>
    /// Get PayPal transaction details
    /// </summary>
    Task<PayPalTransactionDto> GetPayPalTransactionAsync(string transactionId);

    /// <summary>
    /// Setup recurring payment (subscription)
    /// </summary>
    Task<SubscriptionDto> SetupRecurringPaymentAsync(int studentId, string planId, string provider);

    /// <summary>
    /// Cancel subscription
    /// </summary>
    Task<bool> CancelSubscriptionAsync(string subscriptionId, string provider);

    /// <summary>
    /// Get payment history for student
    /// </summary>
    Task<IEnumerable<PaymentHistoryDto>> GetPaymentHistoryAsync(int studentId, string provider = null);

    // ============================================================================
    // VIDEO CONFERENCING INTEGRATION METHODS (9 Methods)
    // ============================================================================

    /// <summary>
    /// Create Zoom meeting for course
    /// </summary>
    Task<ZoomMeetingDto> CreateZoomMeetingAsync(CreateVideoConferenceDto conferenceData);

    /// <summary>
    /// Create Microsoft Teams meeting for course
    /// </summary>
    Task<TeamsMeetingDto> CreateTeamsMeetingAsync(CreateVideoConferenceDto conferenceData);

    /// <summary>
    /// Get Zoom meeting details
    /// </summary>
    Task<ZoomMeetingDetailsDto> GetZoomMeetingDetailsAsync(string meetingId);

    /// <summary>
    /// Get Teams meeting details
    /// </summary>
    Task<TeamsMeetingDetailsDto> GetTeamsMeetingDetailsAsync(string meetingId);

    /// <summary>
    /// End video conference session
    /// </summary>
    Task<bool> EndVideoConferenceAsync(string conferenceId, string provider);

    /// <summary>
    /// Get recording link for completed session
    /// </summary>
    Task<RecordingDto> GetRecordingAsync(string conferenceId, string provider);

    /// <summary>
    /// Add participant to video conference
    /// </summary>
    Task<bool> AddParticipantAsync(string conferenceId, string participantEmail, string provider);

    /// <summary>
    /// Remove participant from video conference
    /// </summary>
    Task<bool> RemoveParticipantAsync(string conferenceId, string participantEmail, string provider);

    /// <summary>
    /// Get video conference statistics and analytics
    /// </summary>
    Task<VideoConferenceAnalyticsDto> GetVideoConferenceAnalyticsAsync(string conferenceId, string provider);

    // ============================================================================
    // OAUTH AUTHENTICATION METHODS (8 Methods)
    // ============================================================================

    /// <summary>
    /// Authenticate user with Google OAuth
    /// </summary>
    Task<OAuthUserDto> AuthenticateWithGoogleAsync(string code, string redirectUri);

    /// <summary>
    /// Authenticate user with Microsoft OAuth
    /// </summary>
    Task<OAuthUserDto> AuthenticateWithMicrosoftAsync(string code, string redirectUri);

    /// <summary>
    /// Authenticate user with GitHub OAuth
    /// </summary>
    Task<OAuthUserDto> AuthenticateWithGitHubAsync(string code, string redirectUri);

    /// <summary>
    /// Link OAuth account to existing user
    /// </summary>
    Task<bool> LinkOAuthAccountAsync(int userId, string provider, string externalId);

    /// <summary>
    /// Unlink OAuth account from user
    /// </summary>
    Task<bool> UnlinkOAuthAccountAsync(int userId, string provider);

    /// <summary>
    /// Get linked OAuth accounts for user
    /// </summary>
    Task<IEnumerable<LinkedOAuthAccountDto>> GetLinkedOAuthAccountsAsync(int userId);

    /// <summary>
    /// Refresh OAuth token
    /// </summary>
    Task<OAuthTokenDto> RefreshOAuthTokenAsync(int userId, string provider);

    /// <summary>
    /// Revoke OAuth authorization
    /// </summary>
    Task<bool> RevokeOAuthAuthorizationAsync(int userId, string provider);

    // ============================================================================
    // WEBHOOK MANAGEMENT METHODS (10 Methods)
    // ============================================================================

    /// <summary>
    /// Register webhook endpoint for events
    /// </summary>
    Task<WebhookRegistrationDto> RegisterWebhookAsync(RegisterWebhookDto webhookData);

    /// <summary>
    /// Get list of registered webhooks
    /// </summary>
    Task<IEnumerable<WebhookDto>> GetWebhooksAsync(int userId);

    /// <summary>
    /// Update webhook configuration
    /// </summary>
    Task<bool> UpdateWebhookAsync(string webhookId, UpdateWebhookDto webhookData);

    /// <summary>
    /// Delete webhook registration
    /// </summary>
    Task<bool> DeleteWebhookAsync(string webhookId);

    /// <summary>
    /// Trigger webhook manually for testing
    /// </summary>
    Task<WebhookTestResultDto> TestWebhookAsync(string webhookId);

    /// <summary>
    /// Get webhook delivery history
    /// </summary>
    Task<IEnumerable<WebhookDeliveryDto>> GetWebhookDeliveryHistoryAsync(string webhookId);

    /// <summary>
    /// Retry failed webhook delivery
    /// </summary>
    Task<bool> RetryWebhookDeliveryAsync(string deliveryId);

    /// <summary>
    /// Get webhook event types
    /// </summary>
    Task<IEnumerable<WebhookEventTypeDto>> GetSupportedWebhookEventsAsync();

    /// <summary>
    /// Get webhook statistics
    /// </summary>
    Task<WebhookStatisticsDto> GetWebhookStatisticsAsync(string webhookId);

    /// <summary>
    /// Verify webhook signature
    /// </summary>
    Task<bool> VerifyWebhookSignatureAsync(string payload, string signature);
}
