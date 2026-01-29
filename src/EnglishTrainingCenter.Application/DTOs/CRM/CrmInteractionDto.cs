namespace EnglishTrainingCenter.Application.DTOs.CRM;

/// <summary>
/// CRM Interaction DTO for listing
/// </summary>
public class CrmInteractionListDto
{
    public int Id { get; set; }
    public int? CustomerId { get; set; }
    public int? LeadId { get; set; }
    public int? OpportunityId { get; set; }
    public string InteractionType { get; set; } = string.Empty;
    public string Subject { get; set; } = string.Empty;
    public DateTime InteractionDate { get; set; }
    public string? Outcome { get; set; }
    public bool IsActive { get; set; } = true;
}

/// <summary>
/// CRM Interaction DTO for details
/// </summary>
public class CrmInteractionDetailDto
{
    public int Id { get; set; }
    public int? CustomerId { get; set; }
    public int? LeadId { get; set; }
    public int? OpportunityId { get; set; }
    public string InteractionType { get; set; } = string.Empty;
    public string Subject { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime InteractionDate { get; set; }
    public int? DurationMinutes { get; set; }
    public int? CreatedByUserId { get; set; }
    public string? CreatedByUserName { get; set; }
    public string? Outcome { get; set; }
    public DateTime? NextFollowUpDate { get; set; }
    public string? Attachments { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedDate { get; set; }
    public DateTime? ModifiedDate { get; set; }
}

/// <summary>
/// CRM Interaction DTO for creating
/// </summary>
public class CreateCrmInteractionDto
{
    public int? CustomerId { get; set; }
    public int? LeadId { get; set; }
    public int? OpportunityId { get; set; }
    public string InteractionType { get; set; } = string.Empty;
    public string Subject { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime InteractionDate { get; set; } = DateTime.UtcNow;
    public int? DurationMinutes { get; set; }
    public string? Outcome { get; set; }
    public DateTime? NextFollowUpDate { get; set; }
    public string? Attachments { get; set; }
}

/// <summary>
/// CRM Interaction DTO for updating
/// </summary>
public class UpdateCrmInteractionDto
{
    public int? CustomerId { get; set; }
    public int? LeadId { get; set; }
    public int? OpportunityId { get; set; }
    public string InteractionType { get; set; } = string.Empty;
    public string Subject { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime InteractionDate { get; set; }
    public int? DurationMinutes { get; set; }
    public string? Outcome { get; set; }
    public DateTime? NextFollowUpDate { get; set; }
    public string? Attachments { get; set; }
    public bool IsActive { get; set; } = true;
}
