# CRM Module - Quick Reference

## 🎯 Quick Start

### 1. Register Services (in Program.cs or ApplicationDependencyInjection.cs)

```csharp
// CRM Services
services.AddScoped<ICustomerService, CustomerService>();
services.AddScoped<ILeadService, LeadService>();
services.AddScoped<IOpportunityService, OpportunityService>();
services.AddScoped<ICrmInteractionService, CrmInteractionService>();
services.AddScoped<ICrmNoteService, CrmNoteService>();

// CRM Validators - add all validators
services.AddScoped<IValidator<CreateCustomerDto>, CreateCustomerValidator>();
services.AddScoped<IValidator<UpdateCustomerDto>, UpdateCustomerValidator>();
// ... add remaining validators
```

### 2. Add AutoMapper Profile

```csharp
// In Mappers/CrmProfile.cs
public class CrmProfile : Profile
{
    public CrmProfile()
    {
        CreateMap<Customer, CustomerListDto>();
        CreateMap<Customer, CustomerDetailDto>();
        CreateMap<CreateCustomerDto, Customer>();
        CreateMap<UpdateCustomerDto, Customer>();
        // ... add other mappings
    }
}
```

### 3. Inject Services in Controller or Service

```csharp
public class MyService
{
    private readonly ICustomerService _customerService;
    
    public MyService(ICustomerService customerService)
    {
        _customerService = customerService;
    }
}
```

---

## 📋 API Endpoint Summary

### Customers
```
GET    /api/v1/crm/customers                    - List all
GET    /api/v1/crm/customers/{id}               - Get by ID
GET    /api/v1/crm/customers/search/{term}      - Search
GET    /api/v1/crm/customers/active/list        - Get active only
GET    /api/v1/crm/customers/{id}/revenue       - Get revenue
POST   /api/v1/crm/customers                    - Create
PUT    /api/v1/crm/customers/{id}               - Update
DELETE /api/v1/crm/customers/{id}               - Delete
```

### Leads
```
GET    /api/v1/crm/leads                        - List all
GET    /api/v1/crm/leads/{id}                   - Get by ID
GET    /api/v1/crm/leads/status/{status}        - By status
GET    /api/v1/crm/leads/interest/hot           - Hot leads
GET    /api/v1/crm/leads/search/{term}          - Search
GET    /api/v1/crm/leads/analytics/total-value  - Total value
POST   /api/v1/crm/leads                        - Create
POST   /api/v1/crm/leads/convert                - Convert to customer
PUT    /api/v1/crm/leads/{id}                   - Update
DELETE /api/v1/crm/leads/{id}                   - Delete
```

### Opportunities
```
GET    /api/v1/crm/opportunities                        - List all
GET    /api/v1/crm/opportunities/{id}                   - Get by ID
GET    /api/v1/crm/opportunities/stage/{stage}          - By stage
GET    /api/v1/crm/opportunities/customer/{customerId}  - By customer
GET    /api/v1/crm/opportunities/search/{term}          - Search
GET    /api/v1/crm/opportunities/analytics/pipeline-value - Pipeline value
GET    /api/v1/crm/opportunities/overdue/list           - Overdue
POST   /api/v1/crm/opportunities                        - Create
PUT    /api/v1/crm/opportunities/{id}                   - Update
PATCH  /api/v1/crm/opportunities/{id}/stage             - Update stage
DELETE /api/v1/crm/opportunities/{id}                   - Delete
```

---

## 📊 Common Use Cases

### Create a Customer

```bash
POST /api/v1/crm/customers
{
  "companyName": "Tech Corp",
  "contactName": "John Doe",
  "email": "john@techcorp.com",
  "phone": "+1 (555) 123-4567",
  "status": "Active",
  "customerSegment": "Enterprise"
}
```

### Create a Lead

```bash
POST /api/v1/crm/leads
{
  "firstName": "Jane",
  "lastName": "Smith",
  "email": "jane@example.com",
  "companyName": "ABC Corp",
  "source": "Website",
  "interestLevel": "Hot",
  "estimatedValue": 50000
}
```

### Create an Opportunity

```bash
POST /api/v1/crm/opportunities
{
  "customerId": 1,
  "opportunityName": "Training Program 2026",
  "description": "Annual training for 200 employees",
  "value": 100000,
  "currency": "USD",
  "stage": "Proposal",
  "probability": 60,
  "estimatedCloseDate": "2026-03-31"
}
```

### Convert Lead to Customer

```bash
POST /api/v1/crm/leads/convert
{
  "leadId": 5,
  "city": "San Francisco",
  "country": "USA",
  "industryType": "Technology",
  "employeeCount": 500
}
```

### Update Opportunity Stage

```bash
PATCH /api/v1/crm/opportunities/1/stage
{
  "stage": "Closed Won",
  "probability": 100
}
```

---

## 🔍 Entity Relationships

```
Customer (1) ─────────────── (Many) Lead
           │
           ├─────────────── (Many) Opportunity
           │
           └─────────────── (Many) CrmInteraction
                                   │
                                   └─────────────── (Many) CrmNote

Lead ─────────────── (Many) CrmInteraction
   │
   └─────────────── (Many) CrmNote

Opportunity ─────────────── (Many) CrmInteraction
           │
           └─────────────── (Many) CrmNote
```

---

## 📝 Entity Fields Reference

### Customer Fields
- `CompanyName` (required) - Company/organization name
- `ContactName` (required) - Primary contact person
- `Email` - Company email
- `Phone` - Company phone
- `Website` - Company website URL
- `Address` - Street address
- `City` - City name
- `Country` - Country name
- `IndustryType` - Industry classification
- `EmployeeCount` - Number of employees
- `AnnualRevenue` - Annual revenue
- `Status` - Active | Inactive | Prospect
- `CustomerSegment` - Enterprise | Mid-Market | SMB
- `AssignedAccountManagerId` - User ID of account manager
- `LastInteractionDate` - Last recorded interaction
- `Notes` - General notes
- `IsActive` - Active status flag

### Lead Fields
- `FirstName` (required) - First name
- `LastName` (required) - Last name
- `Email` - Email address
- `Phone` - Phone number
- `CompanyName` - Lead's company
- `JobTitle` - Job position
- `Source` (required) - Website | Email | Phone | Referral | Event
- `Status` - New | Qualified | Contacted | Unqualified | Converted
- `EstimatedValue` - Potential deal value
- `InterestLevel` - Hot | Warm | Cold
- `PreferredCourse` - Course of interest
- `CustomerId` - Associated customer (if any)
- `AssignedSalesRepId` - Sales rep user ID
- `Notes` - Lead notes

### Opportunity Fields
- `CustomerId` (required) - Associated customer
- `OpportunityName` (required) - Opportunity name
- `Description` - Detailed description
- `Value` (required) - Deal value
- `Currency` - Currency code (USD, EUR, etc.)
- `Stage` - Sales stage
- `Probability` - Win probability (0-100)
- `EstimatedCloseDate` - Expected close date
- `ActualCloseDate` - When actually closed
- `AssignedOwnerId` - Owner user ID
- `CompetitorInfo` - Competitor information
- `WinLossReason` - If closed, why won/lost

### CrmInteraction Fields
- `InteractionType` - Call | Email | Meeting | Demo
- `Subject` (required) - Interaction subject
- `Description` - Details
- `InteractionDate` - When it occurred
- `DurationMinutes` - For calls/meetings
- `Outcome` - Positive | Negative | Neutral | Follow-up Required
- `NextFollowUpDate` - When to follow up
- `CreatedByUserId` - User who logged it
- `CustomerId/LeadId/OpportunityId` - What it relates to

### CrmNote Fields
- `Title` (required) - Note title
- `Content` (required) - Note content
- `Tags` - Comma-separated tags
- `IsPrivate` - Private to creator only
- `CreatedByUserId` - User who created it
- `CustomerId/LeadId/OpportunityId` - What it relates to

---

## ✅ Validation Rules at a Glance

| Field | Rule |
|-------|------|
| CompanyName | Required, max 200 chars |
| ContactName | Required, max 150 chars |
| Email | Valid email format |
| Phone | Valid phone format |
| Status | Active \| Inactive \| Prospect |
| EmployeeCount | Must be > 0 |
| AnnualRevenue | Must be > 0 |
| FirstName/LastName | Required, max 100 chars |
| Source | Website \| Email \| Phone \| Referral \| Event |
| InterestLevel | Hot \| Warm \| Cold |
| Stage | Prospect \| Qualification \| Proposal \| Negotiation \| Closed Won \| Closed Lost |
| Probability | 0-100 |
| InteractionType | Call \| Email \| Meeting \| Demo |

---

## 🗂️ File Locations

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
    └── Entities/DomainEntities.cs (contains all CRM entities)
```

---

## 🔧 Next Steps

1. ✅ **Entities Created** - All CRM domain entities added
2. ✅ **DTOs Created** - All data transfer objects defined
3. ✅ **Validators Created** - FluentValidation rules configured
4. ✅ **Services Created** - Business logic implemented
5. ✅ **Controllers Created** - REST endpoints defined
6. 📋 **Testing** - Write unit tests for CRM module
7. 📋 **Integration** - Register services in Program.cs
8. 📋 **Database Migration** - Run EF Core migrations
9. 📋 **Documentation** - Complete API documentation

---

## 🚀 Testing Endpoints

Use this curl command to test endpoints:

```bash
# Create customer
curl -X POST http://localhost:5000/api/v1/crm/customers \
  -H "Content-Type: application/json" \
  -d '{
    "companyName": "Test Corp",
    "contactName": "Test User",
    "email": "test@test.com",
    "status": "Active"
  }'

# Get all customers
curl http://localhost:5000/api/v1/crm/customers

# Get customer by ID
curl http://localhost:5000/api/v1/crm/customers/1

# Search customers
curl http://localhost:5000/api/v1/crm/customers/search/test
```

---

**Last Updated**: January 2026  
**Version**: 1.0.0  
**Status**: ✅ Ready for Integration
