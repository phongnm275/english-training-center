namespace EnglishTrainingCenter.Application.DTOs.CRM;

/// <summary>
/// Lead DTO for listing
/// </summary>
public class LeadListDto
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? CompanyName { get; set; }
    public string Source { get; set; } = string.Empty;
    public string Status { get; set; } = "New";
    public decimal? EstimatedValue { get; set; }
    public string? InterestLevel { get; set; }
    public DateTime? LastContactDate { get; set; }
    public bool IsActive { get; set; } = true;
}

/// <summary>
/// Lead DTO for details
/// </summary>
public class LeadDetailDto
{
    public int Id { get; set; }
    public int? CustomerId { get; set; }
    public string? CustomerName { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? CompanyName { get; set; }
    public string? JobTitle { get; set; }
    public string Source { get; set; } = string.Empty;
    public string Status { get; set; } = "New";
    public decimal? EstimatedValue { get; set; }
    public int? AssignedSalesRepId { get; set; }
    public string? AssignedSalesRepName { get; set; }
    public DateTime? LastContactDate { get; set; }
    public int? FollowUpDaysRemaining { get; set; }
    public string? InterestLevel { get; set; }
    public string? PreferredCourse { get; set; }
    public string? Notes { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedDate { get; set; }
    public DateTime? ModifiedDate { get; set; }
}

/// <summary>
/// Lead DTO for creating
/// </summary>
public class CreateLeadDto
{
    public int? CustomerId { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? CompanyName { get; set; }
    public string? JobTitle { get; set; }
    public string Source { get; set; } = string.Empty;
    public string Status { get; set; } = "New";
    public decimal? EstimatedValue { get; set; }
    public int? AssignedSalesRepId { get; set; }
    public string? InterestLevel { get; set; }
    public string? PreferredCourse { get; set; }
    public string? Notes { get; set; }
}

/// <summary>
/// Lead DTO for updating
/// </summary>
public class UpdateLeadDto
{
    public int? CustomerId { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? CompanyName { get; set; }
    public string? JobTitle { get; set; }
    public string Source { get; set; } = string.Empty;
    public string Status { get; set; } = "New";
    public decimal? EstimatedValue { get; set; }
    public int? AssignedSalesRepId { get; set; }
    public string? InterestLevel { get; set; }
    public string? PreferredCourse { get; set; }
    public string? Notes { get; set; }
    public bool IsActive { get; set; } = true;
}

/// <summary>
/// Lead conversion DTO
/// </summary>
public class ConvertLeadToCustomerDto
{
    public int LeadId { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? Country { get; set; }
    public string? IndustryType { get; set; }
    public int? EmployeeCount { get; set; }
    public decimal? AnnualRevenue { get; set; }
    public string? CustomerSegment { get; set; }
}
