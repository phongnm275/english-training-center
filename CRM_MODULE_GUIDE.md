# CRM Module - Complete Guide

## Overview

The CRM (Customer Relationship Management) module has been successfully integrated into the English Training Center LMS project. This module provides comprehensive customer management, lead tracking, opportunity management, and interaction logging capabilities.

## Table of Contents

1. [Architecture](#architecture)
2. [Domain Entities](#domain-entities)
3. [API Endpoints](#api-endpoints)
4. [DTOs and Validation](#dtos-and-validation)
5. [Service Layer](#service-layer)
6. [Usage Examples](#usage-examples)
7. [Integration](#integration)
8. [Database Setup](#database-setup)

---

## Architecture

The CRM module follows the **5-layer clean architecture**:

```
┌─────────────────────────────────────┐
│       API Layer (Controllers)        │
├─────────────────────────────────────┤
│     Application Layer (Services)     │
├─────────────────────────────────────┤
│     Infrastructure Layer (DB)        │
├─────────────────────────────────────┤
│       Domain Layer (Entities)        │
├─────────────────────────────────────┤
│        Common Layer (Utilities)      │
└─────────────────────────────────────┘
```

### CRM Module Structure

```
src/
├── EnglishTrainingCenter.API/
│   └── Controllers/CRM/
│       ├── CustomersController.cs
│       ├── LeadsController.cs
│       └── OpportunitiesController.cs
│
├── EnglishTrainingCenter.Application/
│   ├── DTOs/CRM/
│   │   ├── CustomerDto.cs
│   │   ├── LeadDto.cs
│   │   ├── OpportunityDto.cs
│   │   ├── CrmInteractionDto.cs
│   │   └── CrmNoteDto.cs
│   │
│   ├── Services/CRM/
│   │   ├── CustomerService.cs
│   │   ├── LeadService.cs
│   │   ├── OpportunityService.cs
│   │   ├── CrmInteractionService.cs
│   │   └── CrmNoteService.cs
│   │
│   └── Validators/CRM/
│       ├── CustomerValidator.cs
│       ├── LeadValidator.cs
│       ├── OpportunityValidator.cs
│       ├── CrmInteractionValidator.cs
│       └── CrmNoteValidator.cs
│
└── EnglishTrainingCenter.Domain/
    └── Entities/
        ├── Customer.cs (in DomainEntities.cs)
        ├── Lead.cs (in DomainEntities.cs)
        ├── Opportunity.cs (in DomainEntities.cs)
        ├── CrmInteraction.cs (in DomainEntities.cs)
        └── CrmNote.cs (in DomainEntities.cs)
```

---

## Domain Entities

### 1. Customer Entity

```csharp
public class Customer : BaseEntity
{
    public string CompanyName { get; set; }
    public string ContactName { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? Website { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? Country { get; set; }
    public string? IndustryType { get; set; }
    public int? EmployeeCount { get; set; }
    public decimal? AnnualRevenue { get; set; }
    public string Status { get; set; } = "Active";
    public string? CustomerSegment { get; set; }
    public int? AssignedAccountManagerId { get; set; }
    public DateTime? LastInteractionDate { get; set; }
    public string? Notes { get; set; }
    public bool IsActive { get; set; }
}
```

**Status Values**: Active, Inactive, Prospect  
**Customer Segments**: Enterprise, Mid-Market, SMB

---

### 2. Lead Entity

```csharp
public class Lead : BaseEntity
{
    public int? CustomerId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? CompanyName { get; set; }
    public string? JobTitle { get; set; }
    public string Source { get; set; }
    public string Status { get; set; }
    public decimal? EstimatedValue { get; set; }
    public int? AssignedSalesRepId { get; set; }
    public DateTime? LastContactDate { get; set; }
    public int? FollowUpDaysRemaining { get; set; }
    public string? InterestLevel { get; set; }
    public string? PreferredCourse { get; set; }
    public string? Notes { get; set; }
}
```

**Lead Sources**: Website, Email, Phone, Referral, Event  
**Lead Status**: New, Qualified, Contacted, Unqualified, Converted  
**Interest Levels**: Hot, Warm, Cold

---

### 3. Opportunity Entity

```csharp
public class Opportunity : BaseEntity
{
    public int CustomerId { get; set; }
    public string OpportunityName { get; set; }
    public string Description { get; set; }
    public decimal Value { get; set; }
    public string Currency { get; set; }
    public string Stage { get; set; }
    public decimal? Probability { get; set; }
    public DateTime? EstimatedCloseDate { get; set; }
    public DateTime? ActualCloseDate { get; set; }
    public int? AssignedOwnerId { get; set; }
    public string? CompetitorInfo { get; set; }
    public string? WinLossReason { get; set; }
    public string? Notes { get; set; }
}
```

**Stages**: Prospect, Qualification, Proposal, Negotiation, Closed Won, Closed Lost

---

### 4. CrmInteraction Entity

```csharp
public class CrmInteraction : BaseEntity
{
    public int? CustomerId { get; set; }
    public int? LeadId { get; set; }
    public int? OpportunityId { get; set; }
    public string InteractionType { get; set; }
    public string Subject { get; set; }
    public string? Description { get; set; }
    public DateTime InteractionDate { get; set; }
    public int? DurationMinutes { get; set; }
    public int? CreatedByUserId { get; set; }
    public string? Outcome { get; set; }
    public DateTime? NextFollowUpDate { get; set; }
    public string? Attachments { get; set; }
}
```

**Interaction Types**: Call, Email, Meeting, Demo  
**Outcomes**: Positive, Negative, Neutral, Follow-up Required

---

### 5. CrmNote Entity

```csharp
public class CrmNote : BaseEntity
{
    public int? CustomerId { get; set; }
    public int? LeadId { get; set; }
    public int? OpportunityId { get; set; }
    public string? InteractionId { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public int CreatedByUserId { get; set; }
    public bool IsPrivate { get; set; }
    public string? Tags { get; set; }
}
```

---

## API Endpoints

### Customer Endpoints

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/v1/crm/customers` | Get all customers (paginated) |
| GET | `/api/v1/crm/customers/{id}` | Get customer by ID |
| GET | `/api/v1/crm/customers/search/{searchTerm}` | Search customers |
| GET | `/api/v1/crm/customers/active/list` | Get active customers |
| GET | `/api/v1/crm/customers/{id}/revenue` | Get total revenue for customer |
| POST | `/api/v1/crm/customers` | Create new customer |
| PUT | `/api/v1/crm/customers/{id}` | Update customer |
| DELETE | `/api/v1/crm/customers/{id}` | Delete customer (soft delete) |

#### Example: Create Customer

```bash
POST /api/v1/crm/customers HTTP/1.1
Content-Type: application/json

{
  "companyName": "TechCorp Solutions",
  "contactName": "John Smith",
  "email": "john@techcorp.com",
  "phone": "+1 (555) 123-4567",
  "website": "https://techcorp.com",
  "address": "123 Tech Street",
  "city": "San Francisco",
  "country": "USA",
  "industryType": "Technology",
  "employeeCount": 250,
  "annualRevenue": 5000000,
  "status": "Active",
  "customerSegment": "Enterprise",
  "notes": "Key account, high priority"
}
```

**Response** (201 Created):
```json
123
```

---

### Lead Endpoints

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/v1/crm/leads` | Get all leads (paginated) |
| GET | `/api/v1/crm/leads/{id}` | Get lead by ID |
| GET | `/api/v1/crm/leads/status/{status}` | Get leads by status |
| GET | `/api/v1/crm/leads/interest/hot` | Get hot leads |
| GET | `/api/v1/crm/leads/search/{searchTerm}` | Search leads |
| GET | `/api/v1/crm/leads/analytics/total-value` | Get total estimated value |
| POST | `/api/v1/crm/leads` | Create new lead |
| POST | `/api/v1/crm/leads/convert` | Convert lead to customer |
| PUT | `/api/v1/crm/leads/{id}` | Update lead |
| DELETE | `/api/v1/crm/leads/{id}` | Delete lead (soft delete) |

#### Example: Create Lead

```bash
POST /api/v1/crm/leads HTTP/1.1
Content-Type: application/json

{
  "firstName": "Jane",
  "lastName": "Doe",
  "email": "jane.doe@example.com",
  "phone": "+1 (555) 987-6543",
  "companyName": "Acme Inc",
  "jobTitle": "Training Director",
  "source": "Website",
  "status": "New",
  "estimatedValue": 15000,
  "interestLevel": "Hot",
  "preferredCourse": "Business English"
}
```

**Response** (201 Created):
```json
456
```

#### Example: Convert Lead to Customer

```bash
POST /api/v1/crm/leads/convert HTTP/1.1
Content-Type: application/json

{
  "leadId": 456,
  "address": "456 Business Ave",
  "city": "New York",
  "country": "USA",
  "industryType": "Manufacturing",
  "employeeCount": 500,
  "annualRevenue": 50000000,
  "customerSegment": "Enterprise"
}
```

**Response** (200 OK):
```json
124
```

---

### Opportunity Endpoints

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/v1/crm/opportunities` | Get all opportunities (paginated) |
| GET | `/api/v1/crm/opportunities/{id}` | Get opportunity by ID |
| GET | `/api/v1/crm/opportunities/stage/{stage}` | Get opportunities by stage |
| GET | `/api/v1/crm/opportunities/customer/{customerId}` | Get opportunities by customer |
| GET | `/api/v1/crm/opportunities/search/{searchTerm}` | Search opportunities |
| GET | `/api/v1/crm/opportunities/analytics/pipeline-value` | Get total pipeline value |
| GET | `/api/v1/crm/opportunities/overdue/list` | Get overdue opportunities |
| POST | `/api/v1/crm/opportunities` | Create new opportunity |
| PUT | `/api/v1/crm/opportunities/{id}` | Update opportunity |
| PATCH | `/api/v1/crm/opportunities/{id}/stage` | Update opportunity stage |
| DELETE | `/api/v1/crm/opportunities/{id}` | Delete opportunity (soft delete) |

#### Example: Create Opportunity

```bash
POST /api/v1/crm/opportunities HTTP/1.1
Content-Type: application/json

{
  "customerId": 123,
  "opportunityName": "Enterprise Training Program",
  "description": "6-month training program for 100+ employees",
  "value": 150000,
  "currency": "USD",
  "stage": "Proposal",
  "probability": 70,
  "estimatedCloseDate": "2026-03-31",
  "assignedOwnerId": 5
}
```

**Response** (201 Created):
```json
789
```

#### Example: Update Opportunity Stage

```bash
PATCH /api/v1/crm/opportunities/789/stage HTTP/1.1
Content-Type: application/json

{
  "stage": "Closed Won",
  "probability": 100
}
```

**Response** (204 No Content)

---

## DTOs and Validation

### Customer DTOs

- **CreateCustomerDto**: Used for creating new customers
- **UpdateCustomerDto**: Used for updating customers
- **CustomerListDto**: Used for listing customers
- **CustomerDetailDto**: Used for retrieving customer details

### Lead DTOs

- **CreateLeadDto**: Used for creating new leads
- **UpdateLeadDto**: Used for updating leads
- **LeadListDto**: Used for listing leads
- **LeadDetailDto**: Used for retrieving lead details
- **ConvertLeadToCustomerDto**: Used for converting leads to customers

### Opportunity DTOs

- **CreateOpportunityDto**: Used for creating new opportunities
- **UpdateOpportunityDto**: Used for updating opportunities
- **UpdateOpportunityStageDto**: Used for updating opportunity stage
- **OpportunityListDto**: Used for listing opportunities
- **OpportunityDetailDto**: Used for retrieving opportunity details

### Validation Rules

**Customers**:
- CompanyName: Required, max 200 chars
- ContactName: Required, max 150 chars
- Email: Valid email format (optional)
- Phone: Valid phone format (optional)
- Status: Must be Active, Inactive, or Prospect
- EmployeeCount: Must be > 0
- AnnualRevenue: Must be > 0

**Leads**:
- FirstName & LastName: Required, max 100 chars
- Email: Valid email format (optional)
- Phone: Valid phone format (optional)
- Source: Required, must be Website/Email/Phone/Referral/Event
- Status: Must be New/Qualified/Contacted/Unqualified/Converted
- EstimatedValue: Must be > 0
- InterestLevel: Hot, Warm, or Cold

**Opportunities**:
- OpportunityName: Required, max 200 chars
- Value: Required, must be > 0
- Currency: Required, 3-character code
- Stage: Required, must be valid stage
- Probability: 0-100

---

## Service Layer

### ICustomerService

```csharp
public interface ICustomerService
{
    Task<CustomerDetailDto?> GetCustomerByIdAsync(int id);
    Task<List<CustomerListDto>> GetAllCustomersAsync(int pageNumber, int pageSize);
    Task<List<CustomerListDto>> SearchCustomersAsync(string searchTerm, int pageNumber, int pageSize);
    Task<int> CreateCustomerAsync(CreateCustomerDto createDto);
    Task<bool> UpdateCustomerAsync(int id, UpdateCustomerDto updateDto);
    Task<bool> DeleteCustomerAsync(int id);
    Task<int> GetTotalCustomersAsync();
    Task<decimal> GetTotalRevenueByCustomerAsync(int customerId);
    Task<List<CustomerListDto>> GetActiveCustomersAsync();
}
```

### ILeadService

```csharp
public interface ILeadService
{
    Task<LeadDetailDto?> GetLeadByIdAsync(int id);
    Task<List<LeadListDto>> GetAllLeadsAsync(int pageNumber, int pageSize);
    Task<List<LeadListDto>> GetLeadsByStatusAsync(string status, int pageNumber, int pageSize);
    Task<List<LeadListDto>> GetLeadsByInterestLevelAsync(string interestLevel);
    Task<List<LeadListDto>> SearchLeadsAsync(string searchTerm, int pageNumber, int pageSize);
    Task<int> CreateLeadAsync(CreateLeadDto createDto);
    Task<bool> UpdateLeadAsync(int id, UpdateLeadDto updateDto);
    Task<bool> DeleteLeadAsync(int id);
    Task<int> ConvertLeadToCustomerAsync(ConvertLeadToCustomerDto convertDto);
    Task<int> GetTotalLeadsAsync();
    Task<int> GetHotLeadsCountAsync();
    Task<decimal> GetTotalEstimatedValueAsync();
}
```

### IOpportunityService

```csharp
public interface IOpportunityService
{
    Task<OpportunityDetailDto?> GetOpportunityByIdAsync(int id);
    Task<List<OpportunityListDto>> GetAllOpportunitiesAsync(int pageNumber, int pageSize);
    Task<List<OpportunityListDto>> GetOpportunitiesByStageAsync(string stage);
    Task<List<OpportunityListDto>> GetOpportunitiesByCustomerAsync(int customerId);
    Task<List<OpportunityListDto>> SearchOpportunitiesAsync(string searchTerm, int pageNumber, int pageSize);
    Task<int> CreateOpportunityAsync(CreateOpportunityDto createDto);
    Task<bool> UpdateOpportunityAsync(int id, UpdateOpportunityDto updateDto);
    Task<bool> UpdateOpportunityStageAsync(int id, UpdateOpportunityStageDto stageDto);
    Task<bool> DeleteOpportunityAsync(int id);
    Task<decimal> GetTotalPipelineValueAsync();
    Task<decimal> GetPipelineValueByStageAsync(string stage);
    Task<int> GetWonOpportunitiesCountAsync();
    Task<int> GetLostOpportunitiesCountAsync();
    Task<List<OpportunityListDto>> GetOverdueOpportunitiesAsync();
}
```

### ICrmInteractionService

```csharp
public interface ICrmInteractionService
{
    Task<CrmInteractionDetailDto?> GetInteractionByIdAsync(int id);
    Task<List<CrmInteractionListDto>> GetAllInteractionsAsync(int pageNumber, int pageSize);
    Task<List<CrmInteractionListDto>> GetInteractionsByCustomerAsync(int customerId);
    Task<List<CrmInteractionListDto>> GetInteractionsByLeadAsync(int leadId);
    Task<List<CrmInteractionListDto>> GetInteractionsByOpportunityAsync(int opportunityId);
    Task<List<CrmInteractionListDto>> GetInteractionsByTypeAsync(string type);
    Task<int> CreateInteractionAsync(CreateCrmInteractionDto createDto);
    Task<bool> UpdateInteractionAsync(int id, UpdateCrmInteractionDto updateDto);
    Task<bool> DeleteInteractionAsync(int id);
    Task<int> GetTotalInteractionsAsync();
    Task<List<CrmInteractionListDto>> GetPendingFollowUpsAsync();
}
```

### ICrmNoteService

```csharp
public interface ICrmNoteService
{
    Task<CrmNoteDetailDto?> GetNoteByIdAsync(int id);
    Task<List<CrmNoteListDto>> GetAllNotesAsync(int pageNumber, int pageSize);
    Task<List<CrmNoteListDto>> GetNotesByCustomerAsync(int customerId);
    Task<List<CrmNoteListDto>> GetNotesByLeadAsync(int leadId);
    Task<List<CrmNoteListDto>> GetNotesByOpportunityAsync(int opportunityId);
    Task<List<CrmNoteListDto>> GetNotesByTagAsync(string tag);
    Task<List<CrmNoteListDto>> SearchNotesAsync(string searchTerm);
    Task<int> CreateNoteAsync(CreateCrmNoteDto createDto, int userId);
    Task<bool> UpdateNoteAsync(int id, UpdateCrmNoteDto updateDto);
    Task<bool> DeleteNoteAsync(int id);
    Task<int> GetTotalNotesAsync();
}
```

---

## Usage Examples

### Example 1: Creating a Complete CRM Pipeline

```csharp
// 1. Create a customer
var customerRequest = new CreateCustomerDto
{
    CompanyName = "Global Solutions Inc",
    ContactName = "Alice Johnson",
    Email = "alice@globalsolutions.com",
    Phone = "+1 (555) 100-2000",
    City = "Los Angeles",
    Country = "USA",
    IndustryType = "Consulting",
    EmployeeCount = 1000,
    Status = "Active",
    CustomerSegment = "Enterprise"
};
var customerId = await _customerService.CreateCustomerAsync(customerRequest);

// 2. Create a lead
var leadRequest = new CreateLeadDto
{
    FirstName = "Bob",
    LastName = "Williams",
    Email = "bob@globalsolutions.com",
    CompanyName = "Global Solutions Inc",
    JobTitle = "HR Manager",
    Source = "Email",
    InterestLevel = "Hot",
    EstimatedValue = 250000,
    PreferredCourse = "English for Business"
};
var leadId = await _leadService.CreateLeadAsync(leadRequest);

// 3. Convert lead to existing customer
var convertRequest = new ConvertLeadToCustomerDto
{
    LeadId = leadId,
    City = "Los Angeles",
    Country = "USA"
};
await _leadService.ConvertLeadToCustomerAsync(convertRequest);

// 4. Create an opportunity
var opportunityRequest = new CreateOpportunityDto
{
    CustomerId = customerId,
    OpportunityName = "Annual Training Program 2026",
    Description = "12-month comprehensive English training for 500 employees",
    Value = 250000,
    Currency = "USD",
    Stage = "Proposal",
    Probability = 60,
    EstimatedCloseDate = DateTime.UtcNow.AddMonths(3)
};
var opportunityId = await _opportunityService.CreateOpportunityAsync(opportunityRequest);

// 5. Create interactions
var interactionRequest = new CreateCrmInteractionDto
{
    CustomerId = customerId,
    InteractionType = "Meeting",
    Subject = "Initial Training Needs Assessment",
    Description = "Discussed company requirements and training goals",
    DurationMinutes = 60,
    Outcome = "Positive",
    NextFollowUpDate = DateTime.UtcNow.AddDays(7)
};
var interactionId = await _crmInteractionService.CreateInteractionAsync(interactionRequest);

// 6. Add notes
var noteRequest = new CreateCrmNoteDto
{
    CustomerId = customerId,
    Title = "Key Contacts and Decision Makers",
    Content = "CEO: John Smith, HR Director: Jane Lee, Training Budget: $300K",
    Tags = "[\"executive\", \"budget\", \"decision-maker\"]"
};
await _crmNoteService.CreateNoteAsync(noteRequest, userId: 1);
```

### Example 2: Sales Pipeline Analysis

```csharp
// Get all opportunities
var allOpportunities = await _opportunityService.GetAllOpportunitiesAsync(1, 100);

// Get opportunities by stage
var proposals = await _opportunityService.GetOpportunitiesByStageAsync("Proposal");
var negotiations = await _opportunityService.GetOpportunitiesByStageAsync("Negotiation");

// Get total pipeline value
var pipelineValue = await _opportunityService.GetTotalPipelineValueAsync();
Console.WriteLine($"Total Pipeline Value: ${pipelineValue:N2}");

// Get won opportunities count
var wonCount = await _opportunityService.GetWonOpportunitiesCountAsync();
Console.WriteLine($"Won Opportunities: {wonCount}");

// Get overdue opportunities
var overdueOpps = await _opportunityService.GetOverdueOpportunitiesAsync();
Console.WriteLine($"Overdue Opportunities: {overdueOpps.Count}");
```

### Example 3: Lead Management

```csharp
// Get hot leads
var hotLeads = await _leadService.GetLeadsByInterestLevelAsync("Hot");

// Get leads by status
var newLeads = await _leadService.GetLeadsByStatusAsync("New");

// Search for leads
var searchResults = await _leadService.SearchLeadsAsync("john", 1, 10);

// Get total estimated value
var totalValue = await _leadService.GetTotalEstimatedValueAsync();
Console.WriteLine($"Total Estimated Value: ${totalValue:N2}");

// Get hot leads count
var hotCount = await _leadService.GetHotLeadsCountAsync();
Console.WriteLine($"Hot Leads: {hotCount}");
```

---

## Integration

### Dependency Injection Setup

Add these registrations to your `ApplicationDependencyInjection.cs`:

```csharp
public static IServiceCollection AddApplicationDependencies(
    this IServiceCollection services)
{
    // ... existing registrations ...
    
    // CRM Services
    services.AddScoped<ICustomerService, CustomerService>();
    services.AddScoped<ILeadService, LeadService>();
    services.AddScoped<IOpportunityService, OpportunityService>();
    services.AddScoped<ICrmInteractionService, CrmInteractionService>();
    services.AddScoped<ICrmNoteService, CrmNoteService>();
    
    // CRM Validators
    services.AddScoped<IValidator<CreateCustomerDto>, CreateCustomerValidator>();
    services.AddScoped<IValidator<UpdateCustomerDto>, UpdateCustomerValidator>();
    services.AddScoped<IValidator<CreateLeadDto>, CreateLeadValidator>();
    services.AddScoped<IValidator<UpdateLeadDto>, UpdateLeadValidator>();
    services.AddScoped<IValidator<ConvertLeadToCustomerDto>, ConvertLeadToCustomerValidator>();
    services.AddScoped<IValidator<CreateOpportunityDto>, CreateOpportunityValidator>();
    services.AddScoped<IValidator<UpdateOpportunityDto>, UpdateOpportunityValidator>();
    services.AddScoped<IValidator<UpdateOpportunityStageDto>, UpdateOpportunityStageValidator>();
    services.AddScoped<IValidator<CreateCrmInteractionDto>, CreateCrmInteractionValidator>();
    services.AddScoped<IValidator<UpdateCrmInteractionDto>, UpdateCrmInteractionValidator>();
    services.AddScoped<IValidator<CreateCrmNoteDto>, CreateCrmNoteValidator>();
    services.AddScoped<IValidator<UpdateCrmNoteDto>, UpdateCrmNoteValidator>();
    
    // AutoMapper profiles (add after other mappings)
    services.AddAutoMapper(typeof(ApplicationDependencyInjection).Assembly);
    
    return services;
}
```

### AutoMapper Profile

Create `src/EnglishTrainingCenter.Application/Mappers/CrmProfile.cs`:

```csharp
using AutoMapper;
using EnglishTrainingCenter.Application.DTOs.CRM;
using EnglishTrainingCenter.Domain.Entities;

namespace EnglishTrainingCenter.Application.Mappers;

public class CrmProfile : Profile
{
    public CrmProfile()
    {
        // Customer mappings
        CreateMap<Customer, CustomerListDto>();
        CreateMap<Customer, CustomerDetailDto>();
        CreateMap<CreateCustomerDto, Customer>();
        CreateMap<UpdateCustomerDto, Customer>();

        // Lead mappings
        CreateMap<Lead, LeadListDto>();
        CreateMap<Lead, LeadDetailDto>();
        CreateMap<CreateLeadDto, Lead>();
        CreateMap<UpdateLeadDto, Lead>();

        // Opportunity mappings
        CreateMap<Opportunity, OpportunityListDto>();
        CreateMap<Opportunity, OpportunityDetailDto>();
        CreateMap<CreateOpportunityDto, Opportunity>();
        CreateMap<UpdateOpportunityDto, Opportunity>();

        // Interaction mappings
        CreateMap<CrmInteraction, CrmInteractionListDto>();
        CreateMap<CrmInteraction, CrmInteractionDetailDto>();
        CreateMap<CreateCrmInteractionDto, CrmInteraction>();
        CreateMap<UpdateCrmInteractionDto, CrmInteraction>();

        // Note mappings
        CreateMap<CrmNote, CrmNoteListDto>();
        CreateMap<CrmNote, CrmNoteDetailDto>();
        CreateMap<CreateCrmNoteDto, CrmNote>();
        CreateMap<UpdateCrmNoteDto, CrmNote>();
    }
}
```

---

## Database Setup

### Running Migrations

The CRM entities will be automatically created when you run the Entity Framework Core migrations:

```powershell
# In the project root directory

# Add migration for CRM entities
dotnet ef migrations add AddCrmModule --project src/EnglishTrainingCenter.Infrastructure --startup-project src/EnglishTrainingCenter.API

# Update database
dotnet ef database update --project src/EnglishTrainingCenter.Infrastructure --startup-project src/EnglishTrainingCenter.API
```

Or use the database setup script:

```powershell
.\scripts\setup-database.ps1 -Environment Development -CreateIfNotExists -SeedData
```

---

## Best Practices

1. **Always validate input** using FluentValidation before processing
2. **Use soft deletes** by setting `IsActive = false` instead of hard deleting
3. **Track interactions** for all customer communications
4. **Keep notes organized** with tags for easy searching
5. **Update last interaction date** when creating new interactions
6. **Assign ownership** for proper accountability and tracking
7. **Monitor overdue opportunities** regularly
8. **Review hot leads** frequently for timely follow-up
9. **Document customer communication** in notes and interactions
10. **Segment customers** appropriately for targeted engagement

---

## Support & Documentation

For more information:
- See [BUILD_AND_TEST_GUIDE.md](BUILD_AND_TEST_GUIDE.md) for testing the CRM module
- See [PROJECT_OPTIMIZATION_ASSESSMENT.md](PROJECT_OPTIMIZATION_ASSESSMENT.md) for optimization tips
- Check API controllers for detailed endpoint documentation with XML comments

---

**Module Status**: ✅ Complete  
**Last Updated**: January 2026  
**Version**: 1.0
