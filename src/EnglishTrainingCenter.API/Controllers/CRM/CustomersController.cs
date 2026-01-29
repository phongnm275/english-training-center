using Microsoft.AspNetCore.Mvc;
using FluentValidation;
using EnglishTrainingCenter.Application.DTOs.CRM;
using EnglishTrainingCenter.Application.Services.CRM;

namespace EnglishTrainingCenter.API.Controllers.CRM;

/// <summary>
/// CRM Customer Management API
/// </summary>
[ApiController]
[Route("api/v1/crm/[controller]")]
[Produces("application/json")]
public class CustomersController : ControllerBase
{
    private readonly ICustomerService _customerService;
    private readonly IValidator<CreateCustomerDto> _createValidator;
    private readonly IValidator<UpdateCustomerDto> _updateValidator;
    private readonly ILogger<CustomersController> _logger;

    public CustomersController(
        ICustomerService customerService,
        IValidator<CreateCustomerDto> createValidator,
        IValidator<UpdateCustomerDto> updateValidator,
        ILogger<CustomersController> logger)
    {
        _customerService = customerService;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
        _logger = logger;
    }

    /// <summary>
    /// Get all customers
    /// </summary>
    /// <param name="pageNumber">Page number (default: 1)</param>
    /// <param name="pageSize">Page size (default: 10)</param>
    [HttpGet]
    [ProducesResponseType(typeof(List<CustomerListDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<List<CustomerListDto>>> GetAllCustomers(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        try
        {
            var customers = await _customerService.GetAllCustomersAsync(pageNumber, pageSize);
            return Ok(customers);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error getting customers: {ex.Message}");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving customers");
        }
    }

    /// <summary>
    /// Get customer by ID
    /// </summary>
    /// <param name="id">Customer ID</param>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(CustomerDetailDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<CustomerDetailDto>> GetCustomerById(int id)
    {
        try
        {
            var customer = await _customerService.GetCustomerByIdAsync(id);
            if (customer == null)
                return NotFound($"Customer with ID {id} not found");

            return Ok(customer);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error getting customer {id}: {ex.Message}");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving the customer");
        }
    }

    /// <summary>
    /// Search customers
    /// </summary>
    /// <param name="searchTerm">Search term (company name, contact name, or email)</param>
    /// <param name="pageNumber">Page number (default: 1)</param>
    /// <param name="pageSize">Page size (default: 10)</param>
    [HttpGet("search/{searchTerm}")]
    [ProducesResponseType(typeof(List<CustomerListDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<List<CustomerListDto>>> SearchCustomers(
        string searchTerm,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        try
        {
            var customers = await _customerService.SearchCustomersAsync(searchTerm, pageNumber, pageSize);
            return Ok(customers);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error searching customers: {ex.Message}");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while searching customers");
        }
    }

    /// <summary>
    /// Create a new customer
    /// </summary>
    /// <param name="createDto">Customer data</param>
    [HttpPost]
    [ProducesResponseType(typeof(int), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<int>> CreateCustomer([FromBody] CreateCustomerDto createDto)
    {
        try
        {
            var validationResult = await _createValidator.ValidateAsync(createDto);
            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            var customerId = await _customerService.CreateCustomerAsync(createDto);
            return CreatedAtAction(nameof(GetCustomerById), new { id = customerId }, customerId);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error creating customer: {ex.Message}");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while creating the customer");
        }
    }

    /// <summary>
    /// Update an existing customer
    /// </summary>
    /// <param name="id">Customer ID</param>
    /// <param name="updateDto">Updated customer data</param>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateCustomer(int id, [FromBody] UpdateCustomerDto updateDto)
    {
        try
        {
            var validationResult = await _updateValidator.ValidateAsync(updateDto);
            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            var result = await _customerService.UpdateCustomerAsync(id, updateDto);
            if (!result)
                return NotFound($"Customer with ID {id} not found");

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error updating customer {id}: {ex.Message}");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while updating the customer");
        }
    }

    /// <summary>
    /// Delete a customer
    /// </summary>
    /// <param name="id">Customer ID</param>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteCustomer(int id)
    {
        try
        {
            var result = await _customerService.DeleteCustomerAsync(id);
            if (!result)
                return NotFound($"Customer with ID {id} not found");

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error deleting customer {id}: {ex.Message}");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while deleting the customer");
        }
    }

    /// <summary>
    /// Get active customers
    /// </summary>
    [HttpGet("active/list")]
    [ProducesResponseType(typeof(List<CustomerListDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<List<CustomerListDto>>> GetActiveCustomers()
    {
        try
        {
            var customers = await _customerService.GetActiveCustomersAsync();
            return Ok(customers);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error getting active customers: {ex.Message}");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving active customers");
        }
    }

    /// <summary>
    /// Get total revenue for a customer
    /// </summary>
    /// <param name="id">Customer ID</param>
    [HttpGet("{id}/revenue")]
    [ProducesResponseType(typeof(decimal), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<decimal>> GetCustomerRevenue(int id)
    {
        try
        {
            var customer = await _customerService.GetCustomerByIdAsync(id);
            if (customer == null)
                return NotFound($"Customer with ID {id} not found");

            var revenue = await _customerService.GetTotalRevenueByCustomerAsync(id);
            return Ok(revenue);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error getting customer revenue {id}: {ex.Message}");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving customer revenue");
        }
    }
}
