using AutoMapper;
using EnglishTrainingCenter.Application.DTOs.CRM;
using EnglishTrainingCenter.Domain.Entities;
using EnglishTrainingCenter.Domain.Interfaces;

namespace EnglishTrainingCenter.Application.Services.CRM;

/// <summary>
/// Service for managing customers
/// </summary>
public interface ICustomerService
{
    Task<CustomerDetailDto?> GetCustomerByIdAsync(int id);
    Task<List<CustomerListDto>> GetAllCustomersAsync(int pageNumber = 1, int pageSize = 10);
    Task<List<CustomerListDto>> SearchCustomersAsync(string searchTerm, int pageNumber = 1, int pageSize = 10);
    Task<int> CreateCustomerAsync(CreateCustomerDto createDto);
    Task<bool> UpdateCustomerAsync(int id, UpdateCustomerDto updateDto);
    Task<bool> DeleteCustomerAsync(int id);
    Task<int> GetTotalCustomersAsync();
    Task<decimal> GetTotalRevenueByCustomerAsync(int customerId);
    Task<List<CustomerListDto>> GetActiveCustomersAsync();
}

public class CustomerService : ICustomerService
{
    private readonly IRepository<Customer> _repository;
    private readonly IMapper _mapper;
    private readonly IRepository<Opportunity> _opportunityRepository;

    public CustomerService(IRepository<Customer> repository, IMapper mapper, IRepository<Opportunity> opportunityRepository)
    {
        _repository = repository;
        _mapper = mapper;
        _opportunityRepository = opportunityRepository;
    }

    public async Task<CustomerDetailDto?> GetCustomerByIdAsync(int id)
    {
        var customer = await _repository.GetByIdAsync(id);
        if (customer == null) return null;

        return _mapper.Map<CustomerDetailDto>(customer);
    }

    public async Task<List<CustomerListDto>> GetAllCustomersAsync(int pageNumber = 1, int pageSize = 10)
    {
        var customers = await _repository.GetAsync(pageNumber, pageSize);
        return _mapper.Map<List<CustomerListDto>>(customers);
    }

    public async Task<List<CustomerListDto>> SearchCustomersAsync(string searchTerm, int pageNumber = 1, int pageSize = 10)
    {
        var allCustomers = await _repository.GetAsync();
        var filtered = allCustomers
            .Where(c => c.CompanyName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                       c.ContactName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                       (c.Email != null && c.Email.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)))
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        return _mapper.Map<List<CustomerListDto>>(filtered);
    }

    public async Task<int> CreateCustomerAsync(CreateCustomerDto createDto)
    {
        var customer = _mapper.Map<Customer>(createDto);
        customer.LastInteractionDate = DateTime.UtcNow;
        
        await _repository.AddAsync(customer);
        return customer.Id;
    }

    public async Task<bool> UpdateCustomerAsync(int id, UpdateCustomerDto updateDto)
    {
        var customer = await _repository.GetByIdAsync(id);
        if (customer == null) return false;

        _mapper.Map(updateDto, customer);
        customer.ModifiedDate = DateTime.UtcNow;

        await _repository.UpdateAsync(customer);
        return true;
    }

    public async Task<bool> DeleteCustomerAsync(int id)
    {
        var customer = await _repository.GetByIdAsync(id);
        if (customer == null) return false;

        customer.IsActive = false;
        customer.ModifiedDate = DateTime.UtcNow;

        await _repository.UpdateAsync(customer);
        return true;
    }

    public async Task<int> GetTotalCustomersAsync()
    {
        var customers = await _repository.GetAsync();
        return customers.Count(c => c.IsActive);
    }

    public async Task<decimal> GetTotalRevenueByCustomerAsync(int customerId)
    {
        var opportunities = await _opportunityRepository.GetAsync();
        return opportunities
            .Where(o => o.CustomerId == customerId && o.Stage == "Closed Won")
            .Sum(o => o.Value);
    }

    public async Task<List<CustomerListDto>> GetActiveCustomersAsync()
    {
        var customers = await _repository.GetAsync();
        var activeCustomers = customers.Where(c => c.IsActive).ToList();
        return _mapper.Map<List<CustomerListDto>>(activeCustomers);
    }
}
