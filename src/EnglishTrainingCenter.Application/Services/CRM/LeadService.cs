using AutoMapper;
using EnglishTrainingCenter.Application.DTOs.CRM;
using EnglishTrainingCenter.Domain.Entities;
using EnglishTrainingCenter.Domain.Interfaces;

namespace EnglishTrainingCenter.Application.Services.CRM;

/// <summary>
/// Service for managing leads
/// </summary>
public interface ILeadService
{
    Task<LeadDetailDto?> GetLeadByIdAsync(int id);
    Task<List<LeadListDto>> GetAllLeadsAsync(int pageNumber = 1, int pageSize = 10);
    Task<List<LeadListDto>> GetLeadsByStatusAsync(string status, int pageNumber = 1, int pageSize = 10);
    Task<List<LeadListDto>> GetLeadsByInterestLevelAsync(string interestLevel);
    Task<List<LeadListDto>> SearchLeadsAsync(string searchTerm, int pageNumber = 1, int pageSize = 10);
    Task<int> CreateLeadAsync(CreateLeadDto createDto);
    Task<bool> UpdateLeadAsync(int id, UpdateLeadDto updateDto);
    Task<bool> DeleteLeadAsync(int id);
    Task<int> ConvertLeadToCustomerAsync(ConvertLeadToCustomerDto convertDto);
    Task<int> GetTotalLeadsAsync();
    Task<int> GetHotLeadsCountAsync();
    Task<decimal> GetTotalEstimatedValueAsync();
}

public class LeadService : ILeadService
{
    private readonly IRepository<Lead> _repository;
    private readonly IRepository<Customer> _customerRepository;
    private readonly IMapper _mapper;

    public LeadService(IRepository<Lead> repository, IRepository<Customer> customerRepository, IMapper mapper)
    {
        _repository = repository;
        _customerRepository = customerRepository;
        _mapper = mapper;
    }

    public async Task<LeadDetailDto?> GetLeadByIdAsync(int id)
    {
        var lead = await _repository.GetByIdAsync(id);
        if (lead == null) return null;

        return _mapper.Map<LeadDetailDto>(lead);
    }

    public async Task<List<LeadListDto>> GetAllLeadsAsync(int pageNumber = 1, int pageSize = 10)
    {
        var leads = await _repository.GetAsync(pageNumber, pageSize);
        return _mapper.Map<List<LeadListDto>>(leads);
    }

    public async Task<List<LeadListDto>> GetLeadsByStatusAsync(string status, int pageNumber = 1, int pageSize = 10)
    {
        var allLeads = await _repository.GetAsync();
        var filtered = allLeads
            .Where(l => l.Status == status && l.IsActive)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        return _mapper.Map<List<LeadListDto>>(filtered);
    }

    public async Task<List<LeadListDto>> GetLeadsByInterestLevelAsync(string interestLevel)
    {
        var allLeads = await _repository.GetAsync();
        var filtered = allLeads
            .Where(l => l.InterestLevel == interestLevel && l.IsActive)
            .ToList();

        return _mapper.Map<List<LeadListDto>>(filtered);
    }

    public async Task<List<LeadListDto>> SearchLeadsAsync(string searchTerm, int pageNumber = 1, int pageSize = 10)
    {
        var allLeads = await _repository.GetAsync();
        var filtered = allLeads
            .Where(l => l.FirstName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                       l.LastName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                       (l.Email != null && l.Email.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)) ||
                       (l.CompanyName != null && l.CompanyName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)))
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        return _mapper.Map<List<LeadListDto>>(filtered);
    }

    public async Task<int> CreateLeadAsync(CreateLeadDto createDto)
    {
        var lead = _mapper.Map<Lead>(createDto);
        lead.Status = "New";
        
        await _repository.AddAsync(lead);
        return lead.Id;
    }

    public async Task<bool> UpdateLeadAsync(int id, UpdateLeadDto updateDto)
    {
        var lead = await _repository.GetByIdAsync(id);
        if (lead == null) return false;

        _mapper.Map(updateDto, lead);
        lead.ModifiedDate = DateTime.UtcNow;

        await _repository.UpdateAsync(lead);
        return true;
    }

    public async Task<bool> DeleteLeadAsync(int id)
    {
        var lead = await _repository.GetByIdAsync(id);
        if (lead == null) return false;

        lead.IsActive = false;
        lead.ModifiedDate = DateTime.UtcNow;

        await _repository.UpdateAsync(lead);
        return true;
    }

    public async Task<int> ConvertLeadToCustomerAsync(ConvertLeadToCustomerDto convertDto)
    {
        var lead = await _repository.GetByIdAsync(convertDto.LeadId);
        if (lead == null) throw new InvalidOperationException("Lead not found");

        var customer = new Customer
        {
            CompanyName = lead.CompanyName ?? "Unknown",
            ContactName = $"{lead.FirstName} {lead.LastName}",
            Email = lead.Email,
            Phone = lead.Phone,
            Address = convertDto.Address,
            City = convertDto.City,
            Country = convertDto.Country,
            IndustryType = convertDto.IndustryType,
            EmployeeCount = convertDto.EmployeeCount,
            AnnualRevenue = convertDto.AnnualRevenue,
            CustomerSegment = convertDto.CustomerSegment,
            Status = "Active",
            IsActive = true
        };

        await _customerRepository.AddAsync(customer);

        lead.Status = "Converted";
        lead.CustomerId = customer.Id;
        lead.ModifiedDate = DateTime.UtcNow;
        await _repository.UpdateAsync(lead);

        return customer.Id;
    }

    public async Task<int> GetTotalLeadsAsync()
    {
        var leads = await _repository.GetAsync();
        return leads.Count(l => l.IsActive);
    }

    public async Task<int> GetHotLeadsCountAsync()
    {
        var leads = await _repository.GetAsync();
        return leads.Count(l => l.InterestLevel == "Hot" && l.IsActive);
    }

    public async Task<decimal> GetTotalEstimatedValueAsync()
    {
        var leads = await _repository.GetAsync();
        return leads
            .Where(l => l.IsActive && l.EstimatedValue.HasValue)
            .Sum(l => l.EstimatedValue ?? 0);
    }
}
