namespace EnglishTrainingCenter.Application.DTOs.CRM;

/// <summary>
/// CRM Note DTO for listing
/// </summary>
public class CrmNoteListDto
{
    public int Id { get; set; }
    public int? CustomerId { get; set; }
    public int? LeadId { get; set; }
    public int? OpportunityId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Tags { get; set; }
    public DateTime CreatedDate { get; set; }
    public bool IsPrivate { get; set; } = false;
    public bool IsActive { get; set; } = true;
}

/// <summary>
/// CRM Note DTO for details
/// </summary>
public class CrmNoteDetailDto
{
    public int Id { get; set; }
    public int? CustomerId { get; set; }
    public int? LeadId { get; set; }
    public int? OpportunityId { get; set; }
    public string? InteractionId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public int CreatedByUserId { get; set; }
    public string? CreatedByUserName { get; set; }
    public bool IsPrivate { get; set; } = false;
    public string? Tags { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedDate { get; set; }
    public DateTime? ModifiedDate { get; set; }
}

/// <summary>
/// CRM Note DTO for creating
/// </summary>
public class CreateCrmNoteDto
{
    public int? CustomerId { get; set; }
    public int? LeadId { get; set; }
    public int? OpportunityId { get; set; }
    public string? InteractionId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public bool IsPrivate { get; set; } = false;
    public string? Tags { get; set; }
}

/// <summary>
/// CRM Note DTO for updating
/// </summary>
public class UpdateCrmNoteDto
{
    public int? CustomerId { get; set; }
    public int? LeadId { get; set; }
    public int? OpportunityId { get; set; }
    public string? InteractionId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public bool IsPrivate { get; set; } = false;
    public string? Tags { get; set; }
    public bool IsActive { get; set; } = true;
}
