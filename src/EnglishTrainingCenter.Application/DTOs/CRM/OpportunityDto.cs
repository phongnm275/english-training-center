namespace EnglishTrainingCenter.Application.DTOs.CRM;

/// <summary>
/// Opportunity DTO for listing
/// </summary>
public class OpportunityListDto
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public string? CustomerName { get; set; }
    public string OpportunityName { get; set; } = string.Empty;
    public decimal Value { get; set; }
    public string Stage { get; set; } = "Prospect";
    public decimal? Probability { get; set; }
    public DateTime? EstimatedCloseDate { get; set; }
    public bool IsActive { get; set; } = true;
}

/// <summary>
/// Opportunity DTO for details
/// </summary>
public class OpportunityDetailDto
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public string? CustomerName { get; set; }
    public string OpportunityName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Value { get; set; }
    public string Currency { get; set; } = "USD";
    public string Stage { get; set; } = "Prospect";
    public decimal? Probability { get; set; }
    public DateTime? EstimatedCloseDate { get; set; }
    public DateTime? ActualCloseDate { get; set; }
    public int? AssignedOwnerId { get; set; }
    public string? AssignedOwnerName { get; set; }
    public string? CompetitorInfo { get; set; }
    public string? WinLossReason { get; set; }
    public string? Notes { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedDate { get; set; }
    public DateTime? ModifiedDate { get; set; }
}

/// <summary>
/// Opportunity DTO for creating
/// </summary>
public class CreateOpportunityDto
{
    public int CustomerId { get; set; }
    public string OpportunityName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Value { get; set; }
    public string Currency { get; set; } = "USD";
    public string Stage { get; set; } = "Prospect";
    public decimal? Probability { get; set; }
    public DateTime? EstimatedCloseDate { get; set; }
    public int? AssignedOwnerId { get; set; }
    public string? CompetitorInfo { get; set; }
}

/// <summary>
/// Opportunity DTO for updating
/// </summary>
public class UpdateOpportunityDto
{
    public int CustomerId { get; set; }
    public string OpportunityName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Value { get; set; }
    public string Currency { get; set; } = "USD";
    public string Stage { get; set; } = "Prospect";
    public decimal? Probability { get; set; }
    public DateTime? EstimatedCloseDate { get; set; }
    public DateTime? ActualCloseDate { get; set; }
    public int? AssignedOwnerId { get; set; }
    public string? CompetitorInfo { get; set; }
    public string? WinLossReason { get; set; }
    public string? Notes { get; set; }
    public bool IsActive { get; set; } = true;
}

/// <summary>
/// Opportunity stage update DTO
/// </summary>
public class UpdateOpportunityStageDto
{
    public string Stage { get; set; } = string.Empty;
    public decimal? Probability { get; set; }
}
