using AutoMapper;
using EnglishTrainingCenter.Application.DTOs.CRM;
using EnglishTrainingCenter.Domain.Entities;
using EnglishTrainingCenter.Domain.Interfaces;

namespace EnglishTrainingCenter.Application.Services.CRM;

/// <summary>
/// Service for managing CRM notes
/// </summary>
public interface ICrmNoteService
{
    Task<CrmNoteDetailDto?> GetNoteByIdAsync(int id);
    Task<List<CrmNoteListDto>> GetAllNotesAsync(int pageNumber = 1, int pageSize = 10);
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

public class CrmNoteService : ICrmNoteService
{
    private readonly IRepository<CrmNote> _repository;
    private readonly IMapper _mapper;

    public CrmNoteService(IRepository<CrmNote> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<CrmNoteDetailDto?> GetNoteByIdAsync(int id)
    {
        var note = await _repository.GetByIdAsync(id);
        if (note == null) return null;

        return _mapper.Map<CrmNoteDetailDto>(note);
    }

    public async Task<List<CrmNoteListDto>> GetAllNotesAsync(int pageNumber = 1, int pageSize = 10)
    {
        var notes = await _repository.GetAsync(pageNumber, pageSize);
        return _mapper.Map<List<CrmNoteListDto>>(notes);
    }

    public async Task<List<CrmNoteListDto>> GetNotesByCustomerAsync(int customerId)
    {
        var allNotes = await _repository.GetAsync();
        var filtered = allNotes
            .Where(n => n.CustomerId == customerId && n.IsActive)
            .OrderByDescending(n => n.CreatedDate)
            .ToList();

        return _mapper.Map<List<CrmNoteListDto>>(filtered);
    }

    public async Task<List<CrmNoteListDto>> GetNotesByLeadAsync(int leadId)
    {
        var allNotes = await _repository.GetAsync();
        var filtered = allNotes
            .Where(n => n.LeadId == leadId && n.IsActive)
            .OrderByDescending(n => n.CreatedDate)
            .ToList();

        return _mapper.Map<List<CrmNoteListDto>>(filtered);
    }

    public async Task<List<CrmNoteListDto>> GetNotesByOpportunityAsync(int opportunityId)
    {
        var allNotes = await _repository.GetAsync();
        var filtered = allNotes
            .Where(n => n.OpportunityId == opportunityId && n.IsActive)
            .OrderByDescending(n => n.CreatedDate)
            .ToList();

        return _mapper.Map<List<CrmNoteListDto>>(filtered);
    }

    public async Task<List<CrmNoteListDto>> GetNotesByTagAsync(string tag)
    {
        var allNotes = await _repository.GetAsync();
        var filtered = allNotes
            .Where(n => n.IsActive && 
                       n.Tags != null && 
                       n.Tags.Contains(tag, StringComparison.OrdinalIgnoreCase))
            .OrderByDescending(n => n.CreatedDate)
            .ToList();

        return _mapper.Map<List<CrmNoteListDto>>(filtered);
    }

    public async Task<List<CrmNoteListDto>> SearchNotesAsync(string searchTerm)
    {
        var allNotes = await _repository.GetAsync();
        var filtered = allNotes
            .Where(n => n.IsActive &&
                       (n.Title.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                        n.Content.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)))
            .OrderByDescending(n => n.CreatedDate)
            .ToList();

        return _mapper.Map<List<CrmNoteListDto>>(filtered);
    }

    public async Task<int> CreateNoteAsync(CreateCrmNoteDto createDto, int userId)
    {
        var note = _mapper.Map<CrmNote>(createDto);
        note.CreatedByUserId = userId;
        
        await _repository.AddAsync(note);
        return note.Id;
    }

    public async Task<bool> UpdateNoteAsync(int id, UpdateCrmNoteDto updateDto)
    {
        var note = await _repository.GetByIdAsync(id);
        if (note == null) return false;

        _mapper.Map(updateDto, note);
        note.ModifiedDate = DateTime.UtcNow;

        await _repository.UpdateAsync(note);
        return true;
    }

    public async Task<bool> DeleteNoteAsync(int id)
    {
        var note = await _repository.GetByIdAsync(id);
        if (note == null) return false;

        note.IsActive = false;
        note.ModifiedDate = DateTime.UtcNow;

        await _repository.UpdateAsync(note);
        return true;
    }

    public async Task<int> GetTotalNotesAsync()
    {
        var notes = await _repository.GetAsync();
        return notes.Count(n => n.IsActive);
    }
}
