# Phase 5C Option 3: Integration Services Documentation

## Overview

Phase 5C Option 3 provides comprehensive integration services for the English Training Center platform, enabling seamless connectivity with external APIs and services. This phase implements:

- **Google Calendar Integration** - Sync course schedules and manage class reminders
- **Payment Gateway Integration** - Stripe and PayPal payment processing
- **Video Conferencing** - Zoom and Microsoft Teams meeting management
- **OAuth Authentication** - Multi-provider login support
- **Webhook Management** - Event-driven notifications and integrations

---

## Table of Contents

1. [Architecture Overview](#architecture-overview)
2. [Service Interface](#service-interface)
3. [Data Transfer Objects](#data-transfer-objects)
4. [REST API Endpoints](#rest-api-endpoints)
5. [Integration Setup Guides](#integration-setup-guides)
6. [OAuth Configuration](#oauth-configuration)
7. [Webhook Management](#webhook-management)
8. [Usage Examples](#usage-examples)
9. [Best Practices](#best-practices)
10. [Troubleshooting](#troubleshooting)

---

## Architecture Overview

### Service Design Pattern

The integration service follows a **provider-agnostic design pattern**:

```
IIntegrationService (Interface)
    ↓
IntegrationService (Implementation)
    ↓
External API Providers (Google, Stripe, PayPal, Zoom, Teams, OAuth)
```

### Components

| Component | Location | Purpose |
|-----------|----------|---------|
| IIntegrationService | Services/Integration/ | Service contract with 45 methods |
| IntegrationService | Services/Integration/ | Implementation with business logic |
| IntegrationDTOs | DTOs/Integration/ | 52 data transfer objects |
| IntegrationController | Controllers/ | 20 REST API endpoints |
| IntegrationMappingProfile | Mappings/ | AutoMapper configuration |

### Method Organization

All 45 service methods are organized into 5 functional categories:

```
1. Google Calendar Integration (8 methods)
   - Authentication
   - Event management
   - Schedule synchronization

2. Payment Gateway Integration (10 methods)
   - Stripe payment processing
   - PayPal payment processing
   - Refund handling
   - Subscription management

3. Video Conferencing (9 methods)
   - Zoom meeting creation
   - Teams meeting creation
   - Recording management
   - Participant management

4. OAuth Authentication (8 methods)
   - Multi-provider login
   - Account linking
   - Token management

5. Webhook Management (10 methods)
   - Webhook registration
   - Event delivery management
   - Statistics tracking
```

---

## Service Interface

### IIntegrationService

Complete service contract with 45 async methods.

#### Google Calendar Methods

```csharp
Task<GoogleCalendarAuthDto> AuthenticateGoogleCalendarAsync(string email, string authCode);
Task<string> CreateCalendarEventAsync(int userId, CreateCalendarEventDto eventData);
Task<int> SyncCourseScheduleAsync(int studentId, int courseId);
Task<IEnumerable<CalendarEventDto>> GetUpcomingEventsAsync(int userId, int dayCount = 30);
Task<bool> SyncClassRemindersAsync(int courseId);
Task<GoogleCalendarSyncStatusDto> GetCalendarSyncStatusAsync(int userId);
Task<bool> RevokeGoogleCalendarAccessAsync(int userId);
Task<bool> UpdateCalendarEventAsync(int userId, string eventId, UpdateCalendarEventDto eventData);
```

#### Payment Gateway Methods

```csharp
Task<StripeInitiationDto> InitializeStripePaymentAsync(CreatePaymentDto paymentData);
Task<PayPalPaymentDto> ProcessPayPalPaymentAsync(CreatePaymentDto paymentData);
Task<string> CreateStripePaymentIntentAsync(int studentId, decimal amount, string courseName);
Task<PaymentConfirmationDto> ConfirmPaymentAsync(string transactionId, string provider);
Task<PaymentStatusDto> GetStripePaymentStatusAsync(string paymentIntentId);
Task<RefundDto> ProcessRefundAsync(string transactionId, string provider, string reason);
Task<PayPalTransactionDto> GetPayPalTransactionAsync(string transactionId);
Task<SubscriptionDto> SetupRecurringPaymentAsync(int studentId, string planId, string provider);
Task<bool> CancelSubscriptionAsync(string subscriptionId, string provider);
Task<IEnumerable<PaymentHistoryDto>> GetPaymentHistoryAsync(int studentId, string provider = null);
```

#### Video Conferencing Methods

```csharp
Task<ZoomMeetingDto> CreateZoomMeetingAsync(CreateVideoConferenceDto conferenceData);
Task<TeamsMeetingDto> CreateTeamsMeetingAsync(CreateVideoConferenceDto conferenceData);
Task<ZoomMeetingDetailsDto> GetZoomMeetingDetailsAsync(string meetingId);
Task<TeamsMeetingDetailsDto> GetTeamsMeetingDetailsAsync(string meetingId);
Task<bool> EndVideoConferenceAsync(string conferenceId, string provider);
Task<RecordingDto> GetRecordingAsync(string conferenceId, string provider);
Task<bool> AddParticipantAsync(string conferenceId, string participantEmail, string provider);
Task<bool> RemoveParticipantAsync(string conferenceId, string participantEmail, string provider);
Task<VideoConferenceAnalyticsDto> GetVideoConferenceAnalyticsAsync(string conferenceId, string provider);
```

#### OAuth Authentication Methods

```csharp
Task<OAuthUserDto> AuthenticateWithGoogleAsync(string code, string redirectUri);
Task<OAuthUserDto> AuthenticateWithMicrosoftAsync(string code, string redirectUri);
Task<OAuthUserDto> AuthenticateWithGitHubAsync(string code, string redirectUri);
Task<bool> LinkOAuthAccountAsync(int userId, string provider, string externalId);
Task<bool> UnlinkOAuthAccountAsync(int userId, string provider);
Task<IEnumerable<LinkedOAuthAccountDto>> GetLinkedOAuthAccountsAsync(int userId);
Task<OAuthTokenDto> RefreshOAuthTokenAsync(int userId, string provider);
Task<bool> RevokeOAuthAuthorizationAsync(int userId, string provider);
```

#### Webhook Management Methods

```csharp
Task<WebhookRegistrationDto> RegisterWebhookAsync(RegisterWebhookDto webhookData);
Task<IEnumerable<WebhookDto>> GetWebhooksAsync(int userId);
Task<bool> UpdateWebhookAsync(string webhookId, UpdateWebhookDto webhookData);
Task<bool> DeleteWebhookAsync(string webhookId);
Task<WebhookTestResultDto> TestWebhookAsync(string webhookId);
Task<IEnumerable<WebhookDeliveryDto>> GetWebhookDeliveryHistoryAsync(string webhookId);
Task<bool> RetryWebhookDeliveryAsync(string deliveryId);
Task<IEnumerable<WebhookEventTypeDto>> GetSupportedWebhookEventsAsync();
Task<WebhookStatisticsDto> GetWebhookStatisticsAsync(string webhookId);
Task<bool> VerifyWebhookSignatureAsync(string payload, string signature);
```

---

## Data Transfer Objects

### Google Calendar DTOs (6 classes)

#### GoogleCalendarAuthDto
```csharp
public class GoogleCalendarAuthDto
{
    public string Email { get; set; }
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
    public DateTime ExpiresAt { get; set; }
    public bool IsAuthenticated { get; set; }
    public DateTime AuthenticationDate { get; set; }
}
```

#### CalendarEventDto
```csharp
public class CalendarEventDto
{
    public string EventId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public string Location { get; set; }
    public string Status { get; set; }
    public List<string> Attendees { get; set; }
}
```

### Payment Gateway DTOs (9 classes)

#### StripeInitiationDto
```csharp
public class StripeInitiationDto
{
    public string PaymentIntentId { get; set; }
    public string ClientSecret { get; set; }
    public decimal Amount { get; set; }
    public string Currency { get; set; }
    public string Status { get; set; }
    public DateTime CreatedAt { get; set; }
}
```

#### PaymentStatusDto
```csharp
public class PaymentStatusDto
{
    public string PaymentIntentId { get; set; }
    public string Status { get; set; }
    public decimal Amount { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ConfirmedAt { get; set; }
}
```

#### SubscriptionDto
```csharp
public class SubscriptionDto
{
    public string SubscriptionId { get; set; }
    public int StudentId { get; set; }
    public string PlanId { get; set; }
    public string Status { get; set; }
    public string Provider { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime NextBillingDate { get; set; }
    public decimal Amount { get; set; }
}
```

### Video Conferencing DTOs (7 classes)

#### ZoomMeetingDto
```csharp
public class ZoomMeetingDto
{
    public string MeetingId { get; set; }
    public string Topic { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public string JoinUrl { get; set; }
    public int Duration { get; set; }
    public bool RecordingEnabled { get; set; }
    public int MaxParticipants { get; set; }
}
```

#### TeamsMeetingDto
```csharp
public class TeamsMeetingDto
{
    public string MeetingId { get; set; }
    public string Topic { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public string OnlineMeetingUrl { get; set; }
    public int Duration { get; set; }
    public bool RecordingEnabled { get; set; }
}
```

### OAuth DTOs (3 classes)

#### OAuthUserDto
```csharp
public class OAuthUserDto
{
    public string ExternalId { get; set; }
    public string Provider { get; set; }
    public string Email { get; set; }
    public string Name { get; set; }
    public string Picture { get; set; }
    public string AccessToken { get; set; }
    public bool IsNewUser { get; set; }
    public DateTime CreatedAt { get; set; }
}
```

### Webhook DTOs (8 classes)

#### WebhookRegistrationDto
```csharp
public class WebhookRegistrationDto
{
    public string WebhookId { get; set; }
    public int UserId { get; set; }
    public string Url { get; set; }
    public List<string> Events { get; set; }
    public bool IsActive { get; set; }
    public string Secret { get; set; }
    public DateTime CreatedAt { get; set; }
}
```

---

## REST API Endpoints

### Base URL
```
/api/v1/integration
```

### Google Calendar Endpoints

| Method | Endpoint | Description |
|--------|----------|-------------|
| POST | `/google-calendar/authenticate` | Authenticate with Google Calendar |
| POST | `/google-calendar/events` | Create calendar event |
| POST | `/google-calendar/sync-course/{courseId}` | Sync course schedule |
| GET | `/google-calendar/events` | Get upcoming events |
| DELETE | `/google-calendar/authorize` | Revoke calendar access |

### Payment Gateway Endpoints

| Method | Endpoint | Description |
|--------|----------|-------------|
| POST | `/payments/stripe/initialize` | Initialize Stripe payment |
| POST | `/payments/paypal/process` | Process PayPal payment |
| GET | `/payments/stripe/status/{paymentIntentId}` | Get payment status |
| POST | `/payments/refund` | Process refund |
| POST | `/payments/subscription` | Setup recurring payment |
| GET | `/payments/history/{studentId}` | Get payment history |

### Video Conferencing Endpoints

| Method | Endpoint | Description |
|--------|----------|-------------|
| POST | `/video/zoom` | Create Zoom meeting |
| POST | `/video/teams` | Create Teams meeting |
| GET | `/video/zoom/{meetingId}` | Get Zoom details |
| GET | `/video/teams/{meetingId}` | Get Teams details |

### OAuth Endpoints

| Method | Endpoint | Description |
|--------|----------|-------------|
| POST | `/oauth/authenticate` | OAuth authentication callback |
| GET | `/oauth/linked-accounts/{userId}` | Get linked accounts |

### Webhook Endpoints

| Method | Endpoint | Description |
|--------|----------|-------------|
| POST | `/webhooks` | Register webhook |
| GET | `/webhooks/{userId}` | Get user webhooks |
| GET | `/webhooks/events` | Get supported events |
| GET | `/webhooks/{webhookId}/statistics` | Get webhook statistics |

### Health Check

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/health` | Service health check |

---

## Integration Setup Guides

### Stripe Setup

1. **Obtain API Keys**
   - Visit https://dashboard.stripe.com
   - Navigate to Developers → API Keys
   - Copy "Secret Key" and "Publishable Key"

2. **Environment Configuration**
   ```json
   {
     "Stripe": {
       "SecretKey": "sk_live_...",
       "PublishableKey": "pk_live_..."
     }
   }
   ```

3. **Usage Example**
   ```csharp
   var paymentData = new CreatePaymentDto
   {
       StudentId = 1,
       Amount = 99.99m,
       Currency = "USD",
       CourseName = "Advanced English"
   };
   
   var result = await integrationService.InitializeStripePaymentAsync(paymentData);
   ```

### PayPal Setup

1. **Create Business Account**
   - Visit https://www.paypal.com/business
   - Sign up for business account

2. **Obtain Credentials**
   - Go to Settings → API Signature
   - Copy API Username, API Password, Signature

3. **Environment Configuration**
   ```json
   {
     "PayPal": {
       "ApiUsername": "...",
       "ApiPassword": "...",
       "Signature": "...",
       "Environment": "production"
     }
   }
   ```

### Zoom Setup

1. **Create Zoom App**
   - Visit https://marketplace.zoom.us
   - Create new JWT app
   - Generate JWT token

2. **Environment Configuration**
   ```json
   {
     "Zoom": {
       "ApiKey": "...",
       "ApiSecret": "..."
     }
   }
   ```

3. **Create Meeting**
   ```csharp
   var conferenceData = new CreateVideoConferenceDto
   {
       Title = "English Class",
       CourseId = 1,
       StartTime = DateTime.Now.AddDays(1),
       EndTime = DateTime.Now.AddDays(1).AddHours(1),
       EnableRecording = true
   };
   
   var meeting = await integrationService.CreateZoomMeetingAsync(conferenceData);
   ```

### Microsoft Teams Setup

1. **Register Application**
   - Visit https://dev.teams.microsoft.com
   - Create new app

2. **Configure Microsoft Graph**
   - Enable Calendar API
   - Enable Meetings API

3. **Generate Token**
   - Use OAuth 2.0 flow
   - Request scopes: `Calendars.ReadWrite`, `OnlineMeetings.ReadWrite`

---

## OAuth Configuration

### Supported Providers

- Google
- Microsoft (Office 365)
- GitHub

### OAuth Flow

```
1. User clicks "Login with [Provider]"
2. Redirect to provider's authorization endpoint
3. User authorizes application
4. Provider redirects back with authorization code
5. Exchange code for access token
6. Retrieve user information
7. Create/update user account
8. Return authentication token
```

### Configuration

```json
{
  "OAuth": {
    "Google": {
      "ClientId": "...",
      "ClientSecret": "...",
      "RedirectUri": "https://yourdomain.com/api/v1/integration/oauth/callback"
    },
    "Microsoft": {
      "ClientId": "...",
      "ClientSecret": "...",
      "Authority": "https://login.microsoftonline.com/common"
    },
    "GitHub": {
      "ClientId": "...",
      "ClientSecret": "...",
      "RedirectUri": "https://yourdomain.com/api/v1/integration/oauth/callback"
    }
  }
}
```

### Usage Example

```csharp
// Frontend redirects to:
// https://yourdomain.com/api/v1/integration/oauth/authenticate?
//   code=auth_code&provider=google&redirectUri=...

// Backend processes:
var user = await integrationService.AuthenticateWithGoogleAsync(code, redirectUri);

// Response:
{
  "externalId": "goog_12345678",
  "provider": "google",
  "email": "student@example.com",
  "name": "John Doe",
  "isNewUser": true,
  "accessToken": "..."
}
```

---

## Webhook Management

### Supported Events

| Event Type | Trigger | Description |
|------------|---------|-------------|
| `payment_completed` | Payment successful | Triggered when course payment is completed |
| `student_enrolled` | Student enrollment | Triggered when student enrolls in course |
| `grade_updated` | Grade change | Triggered when grade is updated |
| `course_started` | Course start | Triggered when course session starts |
| `course_ended` | Course end | Triggered when course session ends |

### Webhook Payload

```json
{
  "webhookId": "whk_...",
  "eventType": "payment_completed",
  "timestamp": "2024-01-15T10:30:00Z",
  "data": {
    "transactionId": "txn_...",
    "studentId": 1,
    "amount": 99.99,
    "currency": "USD",
    "courseId": 5
  },
  "signature": "sha256=..."
}
```

### Webhook Registration

```csharp
var webhookData = new RegisterWebhookDto
{
    UserId = 1,
    Url = "https://yourdomain.com/webhooks/payment",
    Events = new List<string> { "payment_completed", "student_enrolled" },
    IsActive = true
};

var registration = await integrationService.RegisterWebhookAsync(webhookData);
```

### Signature Verification

```csharp
// Webhook receiver code
[HttpPost("/webhooks/payment")]
public async Task<IActionResult> HandleWebhook([FromBody] object payload, [FromHeader(Name = "X-Webhook-Signature")] string signature)
{
    var payloadJson = JsonConvert.SerializeObject(payload);
    var isValid = await integrationService.VerifyWebhookSignatureAsync(payloadJson, signature);
    
    if (!isValid)
        return Unauthorized();
    
    // Process webhook...
    return Ok();
}
```

---

## Usage Examples

### Example 1: Complete Payment Flow

```csharp
// 1. Initialize payment
var paymentData = new CreatePaymentDto
{
    StudentId = 1,
    Amount = 99.99m,
    Currency = "USD",
    CourseName = "Advanced English"
};

var stripePayment = await integrationService.InitializeStripePaymentAsync(paymentData);
// Response: PaymentIntentId, ClientSecret for frontend

// 2. Frontend processes payment with Stripe.js

// 3. Confirm payment
var confirmation = await integrationService.ConfirmPaymentAsync(
    stripePayment.PaymentIntentId, 
    "stripe"
);

// 4. Get payment history
var history = await integrationService.GetPaymentHistoryAsync(studentId: 1);
```

### Example 2: Schedule Google Calendar Sync

```csharp
// 1. Authenticate
var auth = await integrationService.AuthenticateGoogleCalendarAsync(
    "student@example.com",
    authCode
);

// 2. Sync course schedule
var syncedCount = await integrationService.SyncCourseScheduleAsync(
    studentId: 1,
    courseId: 5
);

// 3. Get upcoming events
var events = await integrationService.GetUpcomingEventsAsync(
    userId: 1,
    dayCount: 30
);
```

### Example 3: Create Video Conference

```csharp
// 1. Create Zoom meeting
var conferenceData = new CreateVideoConferenceDto
{
    Title = "English Class - Module 5",
    CourseId = 5,
    StartTime = DateTime.Now.AddDays(1).At(10, 0),
    EndTime = DateTime.Now.AddDays(1).At(11, 0),
    EnableRecording = true
};

var meeting = await integrationService.CreateZoomMeetingAsync(conferenceData);
// Response: MeetingId, JoinUrl, etc.

// 2. Add participants
await integrationService.AddParticipantAsync(
    meeting.MeetingId,
    "participant@example.com",
    "zoom"
);

// 3. Get meeting details during session
var details = await integrationService.GetZoomMeetingDetailsAsync(meeting.MeetingId);

// 4. Get recording after session ends
var recording = await integrationService.GetRecordingAsync(
    meeting.MeetingId,
    "zoom"
);
```

### Example 4: OAuth Login

```csharp
// Frontend redirects to OAuth provider
// After authorization, callback to:
// /api/v1/integration/oauth/authenticate?code=...&provider=google

// Backend handles:
var user = await integrationService.AuthenticateWithGoogleAsync(code, redirectUri);

// Create/update student account
// Generate JWT token
// Return to frontend
```

---

## Best Practices

### 1. Error Handling

```csharp
try
{
    var result = await integrationService.InitializeStripePaymentAsync(paymentData);
}
catch (ArgumentException ex)
{
    // Student not found or invalid data
    return BadRequest(ex.Message);
}
catch (Exception ex)
{
    _logger.LogError($"Payment error: {ex.Message}");
    return StatusCode(500, "Payment processing failed");
}
```

### 2. Rate Limiting

- Stripe: 100 requests/second
- PayPal: 150 requests/minute
- Zoom: 10 requests/minute per meeting
- Teams: Standard Graph API limits

### 3. Token Refresh

```csharp
// Automatically refresh OAuth tokens before expiry
var token = await integrationService.RefreshOAuthTokenAsync(userId: 1, "google");

// Store new token
student.GoogleAccessToken = token.AccessToken;
await _studentRepository.UpdateAsync(student);
```

### 4. Webhook Retry Logic

- Initial delivery attempt
- Retry after 5 seconds
- Retry after 30 seconds
- Retry after 5 minutes
- After 5 failed attempts, mark as failed

### 5. Idempotency

Always use idempotency keys for payment operations:

```csharp
var paymentData = new CreatePaymentDto
{
    StudentId = 1,
    Amount = 99.99m,
    Currency = "USD",
    IdempotencyKey = Guid.NewGuid().ToString() // Unique per payment
};
```

### 6. Security

- Store API keys in environment variables, never in code
- Use HTTPS for all external API calls
- Validate webhook signatures
- Implement rate limiting on endpoints
- Use OAuth tokens with shortest practical expiry time

### 7. Logging

```csharp
_logger.LogInformation($"Payment initiated for student {studentId}");
_logger.LogWarning($"Payment retry #{retryCount} for {transactionId}");
_logger.LogError($"Payment failed: {ex.Message}");
```

---

## Troubleshooting

### Common Issues

#### 1. Stripe Payment Fails

**Issue**: "Invalid API Key"
- **Solution**: Verify Stripe API key in environment configuration
- Check key is not expired or revoked
- Ensure using correct key (test vs production)

#### 2. Google Calendar Sync Not Working

**Issue**: "Access Denied"
- **Solution**: Re-authenticate with Google
- Check OAuth scopes include `calendar.readonly` and `calendar`
- Verify refresh token is valid

#### 3. Zoom Meeting Creation Fails

**Issue**: "Unauthorized"
- **Solution**: Verify JWT token is valid and not expired
- Check Zoom API key and secret are correct
- Ensure meeting host is valid user

#### 4. OAuth Login Loop

**Issue**: User keeps redirected to login
- **Solution**: Check redirect URI matches OAuth configuration
- Verify authorization code is not expired
- Clear browser cookies and cache

#### 5. Webhook Not Delivering

**Issue**: Webhook events not received
- **Solution**: Verify webhook URL is publicly accessible
- Check webhook is marked as active
- Review delivery history for errors
- Verify signature validation code

### Debug Mode

Enable debug logging:

```json
{
  "Logging": {
    "LogLevel": {
      "EnglishTrainingCenter.Application.Services.Integration": "Debug"
    }
  }
}
```

### Testing

Use health check endpoint:

```bash
curl https://yourdomain.com/api/v1/integration/health
```

Test webhook delivery:

```csharp
var testResult = await integrationService.TestWebhookAsync(webhookId);
// Response: Success, HttpStatusCode, ResponseTime, etc.
```

---

## Support

For issues or questions:
- Review this documentation
- Check service logs
- Contact integration support team
- Review external provider documentation:
  - Stripe: https://stripe.com/docs
  - PayPal: https://developer.paypal.com
  - Zoom: https://marketplace.zoom.us
  - Teams: https://docs.microsoft.com/teams

---

## Version History

| Version | Date | Changes |
|---------|------|---------|
| 1.0 | 2024-01-15 | Initial release with 45 methods, 52 DTOs, 20 endpoints |

---

## Glossary

- **OAuth**: Open authorization protocol for secure delegation
- **API Key**: Unique identifier for API access
- **Webhook**: Event-triggered HTTP callback
- **Idempotency**: Property of operation producing same result regardless of number of executions
- **Rate Limiting**: Restricting number of requests per time period
- **DTO**: Data Transfer Object for API communication
- **Signature Verification**: Confirming message authenticity using cryptographic signatures

