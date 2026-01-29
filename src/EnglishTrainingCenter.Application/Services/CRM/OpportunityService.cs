using AutoMapper;
using EnglishTrainingCenter.Application.DTOs.CRM;
using EnglishTrainingCenter.Domain.Entities;
using EnglishTrainingCenter.Domain.Interfaces;

namespace EnglishTrainingCenter.Application.Services.CRM;

/// <summary>
/// Service for managing opportunities
/// </summary>
public interface IOpportunityService
{
    Task<OpportunityDetailDto?> GetOpportunityByIdAsync(int id);
    Task<List<OpportunityListDto>> GetAllOpportunitiesAsync(int pageNumber = 1, int pageSize = 10);
    Task<List<OpportunityListDto>> GetOpportunitiesByStageAsync(string stage);
    Task<List<OpportunityListDto>> GetOpportunitiesByCustomerAsync(int customerId);
    Task<List<OpportunityListDto>> SearchOpportunitiesAsync(string searchTerm, int pageNumber = 1, int pageSize = 10);
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

public class OpportunityService : IOpportunityService
{
    private readonly IRepository<Opportunity> _repository;
    private readonly IMapper _mapper;

    public OpportunityService(IRepository<Opportunity> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<OpportunityDetailDto?> GetOpportunityByIdAsync(int id)
    {
        var opportunity = await _repository.GetByIdAsync(id);
        if (opportunity == null) return null;

        return _mapper.Map<OpportunityDetailDto>(opportunity);
    }

    public async Task<List<OpportunityListDto>> GetAllOpportunitiesAsync(int pageNumber = 1, int pageSize = 10)
    {
        var opportunities = await _repository.GetAsync(pageNumber, pageSize);
        return _mapper.Map<List<OpportunityListDto>>(opportunities);
    }

    public async Task<List<OpportunityListDto>> GetOpportunitiesByStageAsync(string stage)
    {
        var allOpportunities = await _repository.GetAsync();
        var filtered = allOpportunities
            .Where(o => o.Stage == stage && o.IsActive)
            .ToList();

        return _mapper.Map<List<OpportunityListDto>>(filtered);
    }

    public async Task<List<OpportunityListDto>> GetOpportunitiesByCustomerAsync(int customerId)
    {
        var allOpportunities = await _repository.GetAsync();
        var filtered = allOpportunities
            .Where(o => o.CustomerId == customerId && o.IsActive)
            .ToList();

        return _mapper.Map<List<OpportunityListDto>>(filtered);
    }

    public async Task<List<OpportunityListDto>> SearchOpportunitiesAsync(string searchTerm, int pageNumber = 1, int pageSize = 10)
    {
        var allOpportunities = await _repository.GetAsync();
        var filtered = allOpportunities
            .Where(o => o.OpportunityName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                       o.Description.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        return _mapper.Map<List<OpportunityListDto>>(filtered);
    }

    public async Task<int> CreateOpportunityAsync(CreateOpportunityDto createDto)
    {
        var opportunity = _mapper.Map<Opportunity>(createDto);
        opportunity.Probability = createDto.Probability ?? 0;
        
        await _repository.AddAsync(opportunity);
        return opportunity.Id;
    }

    public async Task<bool> UpdateOpportunityAsync(int id, UpdateOpportunityDto updateDto)
    {
        var opportunity = await _repository.GetByIdAsync(id);
        if (opportunity == null) return false;

        _mapper.Map(updateDto, opportunity);
        opportunity.ModifiedDate = DateTime.UtcNow;

        await _repository.UpdateAsync(opportunity);
        return true;
    }

    public async Task<bool> UpdateOpportunityStageAsync(int id, UpdateOpportunityStageDto stageDto)
    {
        var opportunity = await _repository.GetByIdAsync(id);
        if (opportunity == null) return false;

        opportunity.Stage = stageDto.Stage;
        if (stageDto.Probability.HasValue)
        {
            opportunity.Probability = stageDto.Probability;
        }

        if (stageDto.Stage == "Closed Won" && !opportunity.ActualCloseDate.HasValue)
        {
            opportunity.ActualCloseDate = DateTime.UtcNow;
        }

        opportunity.ModifiedDate = DateTime.UtcNow;

        await _repository.UpdateAsync(opportunity);
        return true;
    }

    public async Task<bool> DeleteOpportunityAsync(int id)
    {
        var opportunity = await _repository.GetByIdAsync(id);
        if (opportunity == null) return false;

        opportunity.IsActive = false;
        opportunity.ModifiedDate = DateTime.UtcNow;

        await _repository.UpdateAsync(opportunity);
        return true;
    }

    public async Task<decimal> GetTotalPipelineValueAsync()
    {
        var opportunities = await _repository.GetAsync();
        return opportunities
            .Where(o => o.IsActive && o.Stage != "Closed Lost")
            .Sum(o => o.Value * ((o.Probability ?? 0) / 100));
    }

    public async Task<decimal> GetPipelineValueByStageAsync(string stage)
    {
        var opportunities = await _repository.GetAsync();
        return opportunities
            .Where(o => o.IsActive && o.Stage == stage)
            .Sum(o => o.Value * ((o.Probability ?? 0) / 100));
    }

    public async Task<int> GetWonOpportunitiesCountAsync()
    {
        var opportunities = await _repository.GetAsync();
        return opportunities.Count(o => o.Stage == "Closed Won");
    }

    public async Task<int> GetLostOpportunitiesCountAsync()
    {
        var opportunities = await _repository.GetAsync();
        return opportunities.Count(o => o.Stage == "Closed Lost");
    }

    public async Task<List<OpportunityListDto>> GetOverdueOpportunitiesAsync()
    {
        var opportunities = await _repository.GetAsync();
        var overdue = opportunities
            .Where(o => o.IsActive && 
                       o.EstimatedCloseDate.HasValue && 
                       o.EstimatedCloseDate < DateTime.UtcNow &&
                       o.Stage != "Closed Won" &&
                       o.Stage != "Closed Lost")
            .ToList();

        return _mapper.Map<List<OpportunityListDto>>(overdue);
    }
}
