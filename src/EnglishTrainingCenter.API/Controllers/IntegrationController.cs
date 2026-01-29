using EnglishTrainingCenter.Application.DTOs.Integration;
using EnglishTrainingCenter.Application.Services.Integration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EnglishTrainingCenter.API.Controllers;

/// <summary>
/// Integration API Controller
/// Handles all integration endpoints for Google Calendar, Payment Gateway, Video Conferencing, OAuth, and Webhooks
/// Base route: /api/v1/integration
/// </summary>
[ApiController]
[Route("api/v1/integration")]
[Authorize]
[Produces("application/json")]
public class IntegrationController : ControllerBase
{
    private readonly IIntegrationService _integrationService;

    public IntegrationController(IIntegrationService integrationService)
    {
        _integrationService = integrationService;
    }

    // ============================================================================
    // GOOGLE CALENDAR ENDPOINTS (5 Endpoints)
    // ============================================================================

    /// <summary>
    /// Authenticates user with Google Calendar
    /// POST /api/v1/integration/google-calendar/authenticate
    /// </summary>
    /// <remarks>
    /// Initiates Google OAuth flow and stores calendar access token
    /// </remarks>
    /// <param name="email">User email address</param>
    /// <param name="authCode">Authorization code from Google OAuth</param>
    /// <returns>Google Calendar authentication details</returns>
    [HttpPost("google-calendar/authenticate")]
    [ProducesResponseType(typeof(GoogleCalendarAuthDto), 200)]
    [ProducesResponseType(400)]
    public async Task<ActionResult<GoogleCalendarAuthDto>> AuthenticateGoogleCalendarAsync(
        [FromQuery] string email,
        [FromQuery] string authCode)
    {
        var result = await _integrationService.AuthenticateGoogleCalendarAsync(email, authCode);
        return Ok(result);
    }

    /// <summary>
    /// Creates calendar event for a student
    /// POST /api/v1/integration/google-calendar/events
    /// </summary>
    /// <remarks>
    /// Adds a new event to the user's Google Calendar
    /// </remarks>
    /// <param name="userId">Student ID</param>
    /// <param name="eventData">Calendar event details</param>
    /// <returns>Created event ID</returns>
    [HttpPost("google-calendar/events")]
    [ProducesResponseType(typeof(string), 201)]
    [ProducesResponseType(400)]
    public async Task<ActionResult<string>> CreateCalendarEventAsync(
        [FromQuery] int userId,
        [FromBody] CreateCalendarEventDto eventData)
    {
        var eventId = await _integrationService.CreateCalendarEventAsync(userId, eventData);
        return CreatedAtAction(nameof(GetUpcomingEventsAsync), new { userId }, eventId);
    }

    /// <summary>
    /// Synchronizes course schedule to Google Calendar
    /// POST /api/v1/integration/google-calendar/sync-course/{courseId}
    /// </summary>
    /// <remarks>
    /// Syncs all sessions of a course to student's calendar
    /// </remarks>
    /// <param name="studentId">Student ID</param>
    /// <param name="courseId">Course ID</param>
    /// <returns>Number of synced events</returns>
    [HttpPost("google-calendar/sync-course/{courseId}")]
    [ProducesResponseType(typeof(int), 200)]
    [ProducesResponseType(404)]
    public async Task<ActionResult<int>> SyncCourseScheduleAsync(
        [FromQuery] int studentId,
        [FromRoute] int courseId)
    {
        var result = await _integrationService.SyncCourseScheduleAsync(studentId, courseId);
        return Ok(result);
    }

    /// <summary>
    /// Retrieves upcoming calendar events
    /// GET /api/v1/integration/google-calendar/events
    /// </summary>
    /// <remarks>
    /// Gets calendar events for next N days
    /// </remarks>
    /// <param name="userId">Student ID</param>
    /// <param name="dayCount">Number of days to look ahead (default: 30)</param>
    /// <returns>List of upcoming calendar events</returns>
    [HttpGet("google-calendar/events")]
    [ProducesResponseType(typeof(IEnumerable<CalendarEventDto>), 200)]
    [ProducesResponseType(404)]
    public async Task<ActionResult<IEnumerable<CalendarEventDto>>> GetUpcomingEventsAsync(
        [FromQuery] int userId,
        [FromQuery] int dayCount = 30)
    {
        var events = await _integrationService.GetUpcomingEventsAsync(userId, dayCount);
        return Ok(events);
    }

    /// <summary>
    /// Revokes Google Calendar access
    /// DELETE /api/v1/integration/google-calendar/authorize
    /// </summary>
    /// <remarks>
    /// Disconnects application from user's Google Calendar
    /// </remarks>
    /// <param name="userId">Student ID</param>
    /// <returns>Success status</returns>
    [HttpDelete("google-calendar/authorize")]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> RevokeGoogleCalendarAccessAsync([FromQuery] int userId)
    {
        await _integrationService.RevokeGoogleCalendarAccessAsync(userId);
        return Ok(new { message = "Google Calendar access revoked" });
    }

    // ============================================================================
    // PAYMENT GATEWAY ENDPOINTS (6 Endpoints)
    // ============================================================================

    /// <summary>
    /// Initializes Stripe payment
    /// POST /api/v1/integration/payments/stripe/initialize
    /// </summary>
    /// <remarks>
    /// Creates Stripe payment intent for course payment
    /// </remarks>
    /// <param name="paymentData">Payment details</param>
    /// <returns>Stripe payment initiation response</returns>
    [HttpPost("payments/stripe/initialize")]
    [ProducesResponseType(typeof(StripeInitiationDto), 201)]
    [ProducesResponseType(400)]
    public async Task<ActionResult<StripeInitiationDto>> InitializeStripePaymentAsync(
        [FromBody] CreatePaymentDto paymentData)
    {
        var result = await _integrationService.InitializeStripePaymentAsync(paymentData);
        return CreatedAtAction(nameof(GetPaymentStatusAsync), new { paymentIntentId = result.PaymentIntentId }, result);
    }

    /// <summary>
    /// Processes PayPal payment
    /// POST /api/v1/integration/payments/paypal/process
    /// </summary>
    /// <remarks>
    /// Processes payment through PayPal gateway
    /// </remarks>
    /// <param name="paymentData">Payment details</param>
    /// <returns>PayPal payment response</returns>
    [HttpPost("payments/paypal/process")]
    [ProducesResponseType(typeof(PayPalPaymentDto), 200)]
    [ProducesResponseType(400)]
    public async Task<ActionResult<PayPalPaymentDto>> ProcessPayPalPaymentAsync(
        [FromBody] CreatePaymentDto paymentData)
    {
        var result = await _integrationService.ProcessPayPalPaymentAsync(paymentData);
        return Ok(result);
    }

    /// <summary>
    /// Gets payment status
    /// GET /api/v1/integration/payments/stripe/status/{paymentIntentId}
    /// </summary>
    /// <remarks>
    /// Retrieves real-time payment status from Stripe
    /// </remarks>
    /// <param name="paymentIntentId">Stripe Payment Intent ID</param>
    /// <returns>Payment status details</returns>
    [HttpGet("payments/stripe/status/{paymentIntentId}")]
    [ProducesResponseType(typeof(PaymentStatusDto), 200)]
    [ProducesResponseType(404)]
    public async Task<ActionResult<PaymentStatusDto>> GetPaymentStatusAsync([FromRoute] string paymentIntentId)
    {
        var status = await _integrationService.GetStripePaymentStatusAsync(paymentIntentId);
        return Ok(status);
    }

    /// <summary>
    /// Processes refund
    /// POST /api/v1/integration/payments/refund
    /// </summary>
    /// <remarks>
    /// Issues refund for a paid transaction
    /// </remarks>
    /// <param name="transactionId">Transaction ID to refund</param>
    /// <param name="provider">Payment provider (stripe/paypal)</param>
    /// <param name="reason">Refund reason</param>
    /// <returns>Refund details</returns>
    [HttpPost("payments/refund")]
    [ProducesResponseType(typeof(RefundDto), 200)]
    [ProducesResponseType(400)]
    public async Task<ActionResult<RefundDto>> ProcessRefundAsync(
        [FromQuery] string transactionId,
        [FromQuery] string provider,
        [FromQuery] string reason)
    {
        var result = await _integrationService.ProcessRefundAsync(transactionId, provider, reason);
        return Ok(result);
    }

    /// <summary>
    /// Sets up recurring payment
    /// POST /api/v1/integration/payments/subscription
    /// </summary>
    /// <remarks>
    /// Creates recurring payment for course subscription
    /// </remarks>
    /// <param name="studentId">Student ID</param>
    /// <param name="planId">Payment plan ID</param>
    /// <param name="provider">Payment provider (stripe/paypal)</param>
    /// <returns>Subscription details</returns>
    [HttpPost("payments/subscription")]
    [ProducesResponseType(typeof(SubscriptionDto), 201)]
    [ProducesResponseType(400)]
    public async Task<ActionResult<SubscriptionDto>> SetupRecurringPaymentAsync(
        [FromQuery] int studentId,
        [FromQuery] string planId,
        [FromQuery] string provider = "stripe")
    {
        var result = await _integrationService.SetupRecurringPaymentAsync(studentId, planId, provider);
        return CreatedAtAction(nameof(GetPaymentHistoryAsync), new { studentId }, result);
    }

    /// <summary>
    /// Gets payment history
    /// GET /api/v1/integration/payments/history/{studentId}
    /// </summary>
    /// <remarks>
    /// Retrieves all payments made by a student
    /// </remarks>
    /// <param name="studentId">Student ID</param>
    /// <param name="provider">Optional payment provider filter</param>
    /// <returns>List of payment records</returns>
    [HttpGet("payments/history/{studentId}")]
    [ProducesResponseType(typeof(IEnumerable<PaymentHistoryDto>), 200)]
    [ProducesResponseType(404)]
    public async Task<ActionResult<IEnumerable<PaymentHistoryDto>>> GetPaymentHistoryAsync(
        [FromRoute] int studentId,
        [FromQuery] string provider = null)
    {
        var history = await _integrationService.GetPaymentHistoryAsync(studentId, provider);
        return Ok(history);
    }

    // ============================================================================
    // VIDEO CONFERENCING ENDPOINTS (4 Endpoints)
    // ============================================================================

    /// <summary>
    /// Creates Zoom meeting
    /// POST /api/v1/integration/video/zoom
    /// </summary>
    /// <remarks>
    /// Creates a new Zoom meeting for a class
    /// </remarks>
    /// <param name="conferenceData">Meeting details</param>
    /// <returns>Zoom meeting information</returns>
    [HttpPost("video/zoom")]
    [ProducesResponseType(typeof(ZoomMeetingDto), 201)]
    [ProducesResponseType(400)]
    public async Task<ActionResult<ZoomMeetingDto>> CreateZoomMeetingAsync(
        [FromBody] CreateVideoConferenceDto conferenceData)
    {
        var result = await _integrationService.CreateZoomMeetingAsync(conferenceData);
        return CreatedAtAction(nameof(GetZoomMeetingDetailsAsync), new { meetingId = result.MeetingId }, result);
    }

    /// <summary>
    /// Creates Microsoft Teams meeting
    /// POST /api/v1/integration/video/teams
    /// </summary>
    /// <remarks>
    /// Creates a new Teams meeting for a class
    /// </remarks>
    /// <param name="conferenceData">Meeting details</param>
    /// <returns>Teams meeting information</returns>
    [HttpPost("video/teams")]
    [ProducesResponseType(typeof(TeamsMeetingDto), 201)]
    [ProducesResponseType(400)]
    public async Task<ActionResult<TeamsMeetingDto>> CreateTeamsMeetingAsync(
        [FromBody] CreateVideoConferenceDto conferenceData)
    {
        var result = await _integrationService.CreateTeamsMeetingAsync(conferenceData);
        return CreatedAtAction(nameof(GetTeamsMeetingDetailsAsync), new { meetingId = result.MeetingId }, result);
    }

    /// <summary>
    /// Gets Zoom meeting details
    /// GET /api/v1/integration/video/zoom/{meetingId}
    /// </summary>
    /// <remarks>
    /// Retrieves detailed information about a Zoom meeting
    /// </remarks>
    /// <param name="meetingId">Zoom Meeting ID</param>
    /// <returns>Meeting details with participants</returns>
    [HttpGet("video/zoom/{meetingId}")]
    [ProducesResponseType(typeof(ZoomMeetingDetailsDto), 200)]
    [ProducesResponseType(404)]
    public async Task<ActionResult<ZoomMeetingDetailsDto>> GetZoomMeetingDetailsAsync([FromRoute] string meetingId)
    {
        var details = await _integrationService.GetZoomMeetingDetailsAsync(meetingId);
        return Ok(details);
    }

    /// <summary>
    /// Gets Teams meeting details
    /// GET /api/v1/integration/video/teams/{meetingId}
    /// </summary>
    /// <remarks>
    /// Retrieves detailed information about a Teams meeting
    /// </remarks>
    /// <param name="meetingId">Teams Meeting ID</param>
    /// <returns>Meeting details with participants</returns>
    [HttpGet("video/teams/{meetingId}")]
    [ProducesResponseType(typeof(TeamsMeetingDetailsDto), 200)]
    [ProducesResponseType(404)]
    public async Task<ActionResult<TeamsMeetingDetailsDto>> GetTeamsMeetingDetailsAsync([FromRoute] string meetingId)
    {
        var details = await _integrationService.GetTeamsMeetingDetailsAsync(meetingId);
        return Ok(details);
    }

    // ============================================================================
    // OAUTH AUTHENTICATION ENDPOINTS (2 Endpoints)
    // ============================================================================

    /// <summary>
    /// Authenticates with OAuth provider
    /// POST /api/v1/integration/oauth/authenticate
    /// </summary>
    /// <remarks>
    /// OAuth callback handler for Google, Microsoft, or GitHub
    /// </remarks>
    /// <param name="code">OAuth authorization code</param>
    /// <param name="provider">OAuth provider (google/microsoft/github)</param>
    /// <param name="redirectUri">OAuth redirect URI</param>
    /// <returns>OAuth user information</returns>
    [HttpPost("oauth/authenticate")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(OAuthUserDto), 200)]
    [ProducesResponseType(400)]
    public async Task<ActionResult<OAuthUserDto>> AuthenticateWithOAuthAsync(
        [FromQuery] string code,
        [FromQuery] string provider,
        [FromQuery] string redirectUri)
    {
        OAuthUserDto result = provider.ToLower() switch
        {
            "google" => await _integrationService.AuthenticateWithGoogleAsync(code, redirectUri),
            "microsoft" => await _integrationService.AuthenticateWithMicrosoftAsync(code, redirectUri),
            "github" => await _integrationService.AuthenticateWithGitHubAsync(code, redirectUri),
            _ => throw new ArgumentException($"Unsupported provider: {provider}")
        };

        return Ok(result);
    }

    /// <summary>
    /// Gets linked OAuth accounts
    /// GET /api/v1/integration/oauth/linked-accounts/{userId}
    /// </summary>
    /// <remarks>
    /// Retrieves all OAuth accounts linked to user profile
    /// </remarks>
    /// <param name="userId">Student ID</param>
    /// <returns>List of linked OAuth accounts</returns>
    [HttpGet("oauth/linked-accounts/{userId}")]
    [ProducesResponseType(typeof(IEnumerable<LinkedOAuthAccountDto>), 200)]
    [ProducesResponseType(404)]
    public async Task<ActionResult<IEnumerable<LinkedOAuthAccountDto>>> GetLinkedOAuthAccountsAsync(
        [FromRoute] int userId)
    {
        var accounts = await _integrationService.GetLinkedOAuthAccountsAsync(userId);
        return Ok(accounts);
    }

    // ============================================================================
    // WEBHOOK MANAGEMENT ENDPOINTS (2 Endpoints)
    // ============================================================================

    /// <summary>
    /// Registers a new webhook
    /// POST /api/v1/integration/webhooks
    /// </summary>
    /// <remarks>
    /// Creates a webhook subscription for real-time event notifications
    /// </remarks>
    /// <param name="webhookData">Webhook configuration</param>
    /// <returns>Webhook registration details</returns>
    [HttpPost("webhooks")]
    [ProducesResponseType(typeof(WebhookRegistrationDto), 201)]
    [ProducesResponseType(400)]
    public async Task<ActionResult<WebhookRegistrationDto>> RegisterWebhookAsync(
        [FromBody] RegisterWebhookDto webhookData)
    {
        var result = await _integrationService.RegisterWebhookAsync(webhookData);
        return CreatedAtAction(nameof(GetWebhooksAsync), new { userId = result.UserId }, result);
    }

    /// <summary>
    /// Gets all webhooks for a user
    /// GET /api/v1/integration/webhooks/{userId}
    /// </summary>
    /// <remarks>
    /// Retrieves all registered webhooks for a user
    /// </remarks>
    /// <param name="userId">Student ID</param>
    /// <returns>List of webhooks</returns>
    [HttpGet("webhooks/{userId}")]
    [ProducesResponseType(typeof(IEnumerable<WebhookDto>), 200)]
    [ProducesResponseType(404)]
    public async Task<ActionResult<IEnumerable<WebhookDto>>> GetWebhooksAsync([FromRoute] int userId)
    {
        var webhooks = await _integrationService.GetWebhooksAsync(userId);
        return Ok(webhooks);
    }

    /// <summary>
    /// Gets supported webhook events
    /// GET /api/v1/integration/webhooks/events
    /// </summary>
    /// <remarks>
    /// Lists all available webhook event types
    /// </remarks>
    /// <returns>List of supported event types</returns>
    [HttpGet("webhooks/events")]
    [ProducesResponseType(typeof(IEnumerable<WebhookEventTypeDto>), 200)]
    public async Task<ActionResult<IEnumerable<WebhookEventTypeDto>>> GetSupportedWebhookEventsAsync()
    {
        var events = await _integrationService.GetSupportedWebhookEventsAsync();
        return Ok(events);
    }

    /// <summary>
    /// Gets webhook statistics
    /// GET /api/v1/integration/webhooks/{webhookId}/statistics
    /// </summary>
    /// <remarks>
    /// Retrieves delivery statistics for a webhook
    /// </remarks>
    /// <param name="webhookId">Webhook ID</param>
    /// <returns>Webhook statistics</returns>
    [HttpGet("webhooks/{webhookId}/statistics")]
    [ProducesResponseType(typeof(WebhookStatisticsDto), 200)]
    [ProducesResponseType(404)]
    public async Task<ActionResult<WebhookStatisticsDto>> GetWebhookStatisticsAsync([FromRoute] string webhookId)
    {
        var stats = await _integrationService.GetWebhookStatisticsAsync(webhookId);
        return Ok(stats);
    }

    /// <summary>
    /// Health check endpoint
    /// GET /api/v1/integration/health
    /// </summary>
    /// <remarks>
    /// Verifies integration service availability
    /// </remarks>
    [HttpGet("health")]
    [AllowAnonymous]
    [ProducesResponseType(200)]
    public IActionResult HealthCheck()
    {
        return Ok(new { status = "healthy", timestamp = DateTime.Now });
    }
}
