namespace EnglishTrainingCenter.Application.DTOs.CRM;

/// <summary>
/// Customer DTO for listing
/// </summary>
public class CustomerListDto
{
    public int Id { get; set; }
    public string CompanyName { get; set; } = string.Empty;
    public string ContactName { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string Status { get; set; } = "Active";
    public string? CustomerSegment { get; set; }
    public DateTime? LastInteractionDate { get; set; }
    public bool IsActive { get; set; } = true;
}

/// <summary>
/// Customer DTO for details
/// </summary>
public class CustomerDetailDto
{
    public int Id { get; set; }
    public string CompanyName { get; set; } = string.Empty;
    public string ContactName { get; set; } = string.Empty;
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
    public int? PreferredLanguageLevel { get; set; }
    public int? AssignedAccountManagerId { get; set; }
    public string? AssignedAccountManagerName { get; set; }
    public DateTime? LastInteractionDate { get; set; }
    public string? Notes { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedDate { get; set; }
    public DateTime? ModifiedDate { get; set; }
}

/// <summary>
/// Customer DTO for creating
/// </summary>
public class CreateCustomerDto
{
    public string CompanyName { get; set; } = string.Empty;
    public string ContactName { get; set; } = string.Empty;
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
    public int? PreferredLanguageLevel { get; set; }
    public int? AssignedAccountManagerId { get; set; }
    public string? Notes { get; set; }
}

/// <summary>
/// Customer DTO for updating
/// </summary>
public class UpdateCustomerDto
{
    public string CompanyName { get; set; } = string.Empty;
    public string ContactName { get; set; } = string.Empty;
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
    public int? PreferredLanguageLevel { get; set; }
    public int? AssignedAccountManagerId { get; set; }
    public string? Notes { get; set; }
    public bool IsActive { get; set; } = true;
}
