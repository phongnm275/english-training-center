using System;
using System.Collections.Generic;

namespace EnglishTrainingCenter.Application.DTOs.Integration;

// ============================================================================
// GOOGLE CALENDAR INTEGRATION DTOS (12 Classes)
// ============================================================================

/// <summary>Google Calendar authentication response</summary>
public class GoogleCalendarAuthDto
{
    public int UserId { get; set; }
    public string Email { get; set; }
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
    public DateTime ExpiresAt { get; set; }
    public bool IsAuthenticated { get; set; }
    public DateTime AuthenticationDate { get; set; }
}

/// <summary>Create calendar event request</summary>
public class CreateCalendarEventDto
{
    public int UserId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public string Location { get; set; }
    public List<string> AttendeeEmails { get; set; }
    public bool SendNotifications { get; set; }
    public int ReminderMinutes { get; set; }
}

/// <summary>Update calendar event request</summary>
public class UpdateCalendarEventDto
{
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime? StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public string Location { get; set; }
    public List<string> AttendeeEmails { get; set; }
    public int? ReminderMinutes { get; set; }
}

/// <summary>Calendar event details</summary>
public class CalendarEventDto
{
    public string EventId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public string Location { get; set; }
    public string CreatorEmail { get; set; }
    public List<string> AttendeeEmails { get; set; }
    public int AttendeeCount { get; set; }
    public string Status { get; set; } // confirmed, tentative, cancelled
    public bool IsMuted { get; set; }
    public DateTime CreatedDate { get; set; }
}

/// <summary>Google Calendar sync status</summary>
public class GoogleCalendarSyncStatusDto
{
    public int UserId { get; set; }
    public bool IsSynced { get; set; }
    public DateTime LastSyncDate { get; set; }
    public int SyncedEventsCount { get; set; }
    public int TotalCalendars { get; set; }
    public string SyncStatus { get; set; } // synced, syncing, failed
    public string ErrorMessage { get; set; }
    public List<string> SyncedCalendars { get; set; }
}

// ============================================================================
// PAYMENT GATEWAY INTEGRATION DTOS (18 Classes)
// ============================================================================

/// <summary>Create payment request</summary>
public class CreatePaymentDto
{
    public int StudentId { get; set; }
    public int CourseId { get; set; }
    public decimal Amount { get; set; }
    public string CourseName { get; set; }
    public string Description { get; set; }
    public string Currency { get; set; } // USD, VND, etc.
    public string PaymentProvider { get; set; } // stripe, paypal
    public Dictionary<string, string> Metadata { get; set; }
}

/// <summary>Stripe payment initialization</summary>
public class StripeInitiationDto
{
    public string PaymentIntentId { get; set; }
    public string ClientSecret { get; set; }
    public decimal Amount { get; set; }
    public string Currency { get; set; }
    public string Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public Dictionary<string, object> Metadata { get; set; }
}

/// <summary>PayPal payment response</summary>
public class PayPalPaymentDto
{
    public string TransactionId { get; set; }
    public string Status { get; set; } // completed, pending, failed
    public decimal Amount { get; set; }
    public string Currency { get; set; }
    public string PayerEmail { get; set; }
    public DateTime TransactionDate { get; set; }
    public string ApprovalUrl { get; set; }
    public Dictionary<string, object> PayerInfo { get; set; }
}

/// <summary>Payment confirmation</summary>
public class PaymentConfirmationDto
{
    public string TransactionId { get; set; }
    public int StudentId { get; set; }
    public int CourseId { get; set; }
    public decimal Amount { get; set; }
    public string Status { get; set; } // completed, failed, cancelled
    public string Provider { get; set; }
    public DateTime ConfirmationDate { get; set; }
    public string ReceiptUrl { get; set; }
    public Dictionary<string, string> StudentInfo { get; set; }
}

/// <summary>Payment status</summary>
public class PaymentStatusDto
{
    public string PaymentIntentId { get; set; }
    public string Status { get; set; } // requires_payment_method, requires_confirmation, requires_action, processing, succeeded, cancelled
    public decimal Amount { get; set; }
    public decimal AmountReceived { get; set; }
    public string Currency { get; set; }
    public string LastPaymentError { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ConfirmedAt { get; set; }
}

/// <summary>Refund request and result</summary>
public class RefundDto
{
    public string RefundId { get; set; }
    public string TransactionId { get; set; }
    public decimal Amount { get; set; }
    public string Status { get; set; } // succeeded, pending, failed
    public string Reason { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public string Provider { get; set; }
}

/// <summary>PayPal transaction details</summary>
public class PayPalTransactionDto
{
    public string TransactionId { get; set; }
    public string Status { get; set; }
    public decimal Amount { get; set; }
    public string Currency { get; set; }
    public string PayerEmail { get; set; }
    public string PayerName { get; set; }
    public DateTime TransactionDate { get; set; }
    public string ItemName { get; set; }
    public Dictionary<string, object> TransactionDetails { get; set; }
}

/// <summary>Subscription setup</summary>
public class SubscriptionDto
{
    public string SubscriptionId { get; set; }
    public int StudentId { get; set; }
    public string PlanId { get; set; }
    public string Status { get; set; } // active, cancelled, past_due
    public decimal Amount { get; set; }
    public string Currency { get; set; }
    public string BillingCycle { get; set; } // monthly, yearly
    public DateTime StartDate { get; set; }
    public DateTime? NextBillingDate { get; set; }
    public DateTime? CancelledAt { get; set; }
    public string Provider { get; set; }
}

/// <summary>Payment history item</summary>
public class PaymentHistoryDto
{
    public string TransactionId { get; set; }
    public int StudentId { get; set; }
    public int CourseId { get; set; }
    public string CourseName { get; set; }
    public decimal Amount { get; set; }
    public string Status { get; set; }
    public string Provider { get; set; }
    public DateTime PaymentDate { get; set; }
    public DateTime? CompletedDate { get; set; }
    public string PaymentMethod { get; set; }
}

// ============================================================================
// VIDEO CONFERENCING INTEGRATION DTOS (12 Classes)
// ============================================================================

/// <summary>Create video conference request</summary>
public class CreateVideoConferenceDto
{
    public int CourseId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public int MaxParticipants { get; set; }
    public string Provider { get; set; } // zoom, teams
    public bool EnableRecording { get; set; }
    public bool RequirePassword { get; set; }
    public List<string> InvitedEmails { get; set; }
}

/// <summary>Zoom meeting details</summary>
public class ZoomMeetingDto
{
    public string MeetingId { get; set; }
    public string MeetingNumber { get; set; }
    public string Topic { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public string JoinUrl { get; set; }
    public string StartUrl { get; set; }
    public string Password { get; set; }
    public int Duration { get; set; }
    public bool RecordingEnabled { get; set; }
}

/// <summary>Zoom meeting detailed info</summary>
public class ZoomMeetingDetailsDto
{
    public string MeetingId { get; set; }
    public string Topic { get; set; }
    public DateTime StartTime { get; set; }
    public int Duration { get; set; }
    public int ParticipantCount { get; set; }
    public List<ParticipantDto> Participants { get; set; }
    public string Status { get; set; } // not_started, live, ended
    public bool RecordingEnabled { get; set; }
    public string RecordingStatus { get; set; }
}

/// <summary>Microsoft Teams meeting details</summary>
public class TeamsMeetingDto
{
    public string MeetingId { get; set; }
    public string Topic { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public string JoinUrl { get; set; }
    public string OnlineMeetingUrl { get; set; }
    public int Duration { get; set; }
    public bool RecordingEnabled { get; set; }
}

/// <summary>Teams meeting detailed info</summary>
public class TeamsMeetingDetailsDto
{
    public string MeetingId { get; set; }
    public string Topic { get; set; }
    public DateTime StartTime { get; set; }
    public int Duration { get; set; }
    public int ParticipantCount { get; set; }
    public List<ParticipantDto> Participants { get; set; }
    public string Status { get; set; } // scheduled, inprogress, ended
    public string RecordingStatus { get; set; }
}

/// <summary>Participant in video conference</summary>
public class ParticipantDto
{
    public string ParticipantId { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public DateTime JoinTime { get; set; }
    public DateTime? LeaveTime { get; set; }
    public int DurationMinutes { get; set; }
    public bool IsHost { get; set; }
}

/// <summary>Recording information</summary>
public class RecordingDto
{
    public string RecordingId { get; set; }
    public string ConferenceId { get; set; }
    public DateTime RecordingDate { get; set; }
    public int DurationMinutes { get; set; }
    public string DownloadUrl { get; set; }
    public string StreamUrl { get; set; }
    public long FileSize { get; set; }
    public string Provider { get; set; }
    public string Status { get; set; } // processing, ready, expired
}

/// <summary>Video conference analytics</summary>
public class VideoConferenceAnalyticsDto
{
    public string ConferenceId { get; set; }
    public int TotalParticipants { get; set; }
    public int PeakParticipants { get; set; }
    public int DurationMinutes { get; set; }
    public string Provider { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public double AverageConnectionQuality { get; set; }
    public int DroppedParticipants { get; set; }
    public List<ParticipantAnalyticsDto> ParticipantStats { get; set; }
}

/// <summary>Participant analytics</summary>
public class ParticipantAnalyticsDto
{
    public string ParticipantId { get; set; }
    public string Name { get; set; }
    public int DurationMinutes { get; set; }
    public double AverageAudioQuality { get; set; }
    public double AverageVideoQuality { get; set; }
    public int AudioDrops { get; set; }
    public int VideoDrops { get; set; }
}

// ============================================================================
// OAUTH AUTHENTICATION DTOS (8 Classes)
// ============================================================================

/// <summary>OAuth user information</summary>
public class OAuthUserDto
{
    public string ExternalId { get; set; }
    public string Email { get; set; }
    public string Name { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string ProfilePictureUrl { get; set; }
    public string Provider { get; set; } // google, microsoft, github
    public bool IsNewUser { get; set; }
    public int UserId { get; set; }
    public DateTime CreatedAt { get; set; }
}

/// <summary>OAuth token information</summary>
public class OAuthTokenDto
{
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
    public string TokenType { get; set; }
    public int ExpiresIn { get; set; }
    public DateTime IssuedAt { get; set; }
    public DateTime ExpiresAt { get; set; }
    public string Provider { get; set; }
}

/// <summary>Linked OAuth account</summary>
public class LinkedOAuthAccountDto
{
    public string Provider { get; set; }
    public string ExternalId { get; set; }
    public string Email { get; set; }
    public string Name { get; set; }
    public DateTime LinkedDate { get; set; }
    public bool IsPrimary { get; set; }
}

// ============================================================================
// WEBHOOK MANAGEMENT DTOS (12 Classes)
// ============================================================================

/// <summary>Register webhook request</summary>
public class RegisterWebhookDto
{
    public int UserId { get; set; }
    public string Url { get; set; }
    public List<string> Events { get; set; } // payment_completed, course_enrolled, etc.
    public string Secret { get; set; }
    public bool IsActive { get; set; }
    public Dictionary<string, string> Headers { get; set; }
}

/// <summary>Webhook registration response</summary>
public class WebhookRegistrationDto
{
    public string WebhookId { get; set; }
    public int UserId { get; set; }
    public string Url { get; set; }
    public List<string> Events { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public string Secret { get; set; }
}

/// <summary>Webhook details</summary>
public class WebhookDto
{
    public string WebhookId { get; set; }
    public string Url { get; set; }
    public List<string> Events { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime LastTriggeredAt { get; set; }
    public int SuccessCount { get; set; }
    public int FailureCount { get; set; }
    public string LastStatus { get; set; }
}

/// <summary>Update webhook request</summary>
public class UpdateWebhookDto
{
    public string Url { get; set; }
    public List<string> Events { get; set; }
    public bool? IsActive { get; set; }
    public Dictionary<string, string> Headers { get; set; }
}

/// <summary>Webhook test result</summary>
public class WebhookTestResultDto
{
    public string WebhookId { get; set; }
    public bool Success { get; set; }
    public int HttpStatusCode { get; set; }
    public string ResponseTime { get; set; }
    public string ResponseBody { get; set; }
    public string ErrorMessage { get; set; }
    public DateTime TestedAt { get; set; }
}

/// <summary>Webhook delivery history</summary>
public class WebhookDeliveryDto
{
    public string DeliveryId { get; set; }
    public string WebhookId { get; set; }
    public string EventType { get; set; }
    public DateTime DeliveredAt { get; set; }
    public int HttpStatusCode { get; set; }
    public bool Success { get; set; }
    public string ResponseTime { get; set; }
    public int RetryCount { get; set; }
    public string Payload { get; set; }
}

/// <summary>Webhook event type</summary>
public class WebhookEventTypeDto
{
    public string EventType { get; set; }
    public string Description { get; set; }
    public List<string> PayloadExample { get; set; }
}

/// <summary>Webhook statistics</summary>
public class WebhookStatisticsDto
{
    public string WebhookId { get; set; }
    public int TotalDeliveries { get; set; }
    public int SuccessfulDeliveries { get; set; }
    public int FailedDeliveries { get; set; }
    public double SuccessRate { get; set; }
    public double AverageResponseTime { get; set; }
    public DateTime LastDeliveryAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public int DaysActive { get; set; }
}
