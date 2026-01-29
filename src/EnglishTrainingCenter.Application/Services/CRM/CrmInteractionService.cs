using AutoMapper;
using EnglishTrainingCenter.Application.DTOs.CRM;
using EnglishTrainingCenter.Domain.Entities;
using EnglishTrainingCenter.Domain.Interfaces;

namespace EnglishTrainingCenter.Application.Services.CRM;

/// <summary>
/// Service for managing CRM interactions
/// </summary>
public interface ICrmInteractionService
{
    Task<CrmInteractionDetailDto?> GetInteractionByIdAsync(int id);
    Task<List<CrmInteractionListDto>> GetAllInteractionsAsync(int pageNumber = 1, int pageSize = 10);
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

public class CrmInteractionService : ICrmInteractionService
{
    private readonly IRepository<CrmInteraction> _repository;
    private readonly IMapper _mapper;

    public CrmInteractionService(IRepository<CrmInteraction> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<CrmInteractionDetailDto?> GetInteractionByIdAsync(int id)
    {
        var interaction = await _repository.GetByIdAsync(id);
        if (interaction == null) return null;

        return _mapper.Map<CrmInteractionDetailDto>(interaction);
    }

    public async Task<List<CrmInteractionListDto>> GetAllInteractionsAsync(int pageNumber = 1, int pageSize = 10)
    {
        var interactions = await _repository.GetAsync(pageNumber, pageSize);
        return _mapper.Map<List<CrmInteractionListDto>>(interactions);
    }

    public async Task<List<CrmInteractionListDto>> GetInteractionsByCustomerAsync(int customerId)
    {
        var allInteractions = await _repository.GetAsync();
        var filtered = allInteractions
            .Where(i => i.CustomerId == customerId && i.IsActive)
            .OrderByDescending(i => i.InteractionDate)
            .ToList();

        return _mapper.Map<List<CrmInteractionListDto>>(filtered);
    }

    public async Task<List<CrmInteractionListDto>> GetInteractionsByLeadAsync(int leadId)
    {
        var allInteractions = await _repository.GetAsync();
        var filtered = allInteractions
            .Where(i => i.LeadId == leadId && i.IsActive)
            .OrderByDescending(i => i.InteractionDate)
            .ToList();

        return _mapper.Map<List<CrmInteractionListDto>>(filtered);
    }

    public async Task<List<CrmInteractionListDto>> GetInteractionsByOpportunityAsync(int opportunityId)
    {
        var allInteractions = await _repository.GetAsync();
        var filtered = allInteractions
            .Where(i => i.OpportunityId == opportunityId && i.IsActive)
            .OrderByDescending(i => i.InteractionDate)
            .ToList();

        return _mapper.Map<List<CrmInteractionListDto>>(filtered);
    }

    public async Task<List<CrmInteractionListDto>> GetInteractionsByTypeAsync(string type)
    {
        var allInteractions = await _repository.GetAsync();
        var filtered = allInteractions
            .Where(i => i.InteractionType == type && i.IsActive)
            .OrderByDescending(i => i.InteractionDate)
            .ToList();

        return _mapper.Map<List<CrmInteractionListDto>>(filtered);
    }

    public async Task<int> CreateInteractionAsync(CreateCrmInteractionDto createDto)
    {
        var interaction = _mapper.Map<CrmInteraction>(createDto);
        
        await _repository.AddAsync(interaction);
        return interaction.Id;
    }

    public async Task<bool> UpdateInteractionAsync(int id, UpdateCrmInteractionDto updateDto)
    {
        var interaction = await _repository.GetByIdAsync(id);
        if (interaction == null) return false;

        _mapper.Map(updateDto, interaction);
        interaction.ModifiedDate = DateTime.UtcNow;

        await _repository.UpdateAsync(interaction);
        return true;
    }

    public async Task<bool> DeleteInteractionAsync(int id)
    {
        var interaction = await _repository.GetByIdAsync(id);
        if (interaction == null) return false;

        interaction.IsActive = false;
        interaction.ModifiedDate = DateTime.UtcNow;

        await _repository.UpdateAsync(interaction);
        return true;
    }

    public async Task<int> GetTotalInteractionsAsync()
    {
        var interactions = await _repository.GetAsync();
        return interactions.Count(i => i.IsActive);
    }

    public async Task<List<CrmInteractionListDto>> GetPendingFollowUpsAsync()
    {
        var allInteractions = await _repository.GetAsync();
        var pending = allInteractions
            .Where(i => i.IsActive && 
                       i.NextFollowUpDate.HasValue && 
                       i.NextFollowUpDate <= DateTime.UtcNow)
            .OrderBy(i => i.NextFollowUpDate)
            .ToList();

        return _mapper.Map<List<CrmInteractionListDto>>(pending);
    }
}
