using Microsoft.AspNetCore.Mvc;
using FluentValidation;
using EnglishTrainingCenter.Application.DTOs.CRM;
using EnglishTrainingCenter.Application.Services.CRM;

namespace EnglishTrainingCenter.API.Controllers.CRM;

/// <summary>
/// CRM Lead Management API
/// </summary>
[ApiController]
[Route("api/v1/crm/[controller]")]
[Produces("application/json")]
public class LeadsController : ControllerBase
{
    private readonly ILeadService _leadService;
    private readonly IValidator<CreateLeadDto> _createValidator;
    private readonly IValidator<UpdateLeadDto> _updateValidator;
    private readonly IValidator<ConvertLeadToCustomerDto> _convertValidator;
    private readonly ILogger<LeadsController> _logger;

    public LeadsController(
        ILeadService leadService,
        IValidator<CreateLeadDto> createValidator,
        IValidator<UpdateLeadDto> updateValidator,
        IValidator<ConvertLeadToCustomerDto> convertValidator,
        ILogger<LeadsController> logger)
    {
        _leadService = leadService;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
        _convertValidator = convertValidator;
        _logger = logger;
    }

    /// <summary>
    /// Get all leads
    /// </summary>
    /// <param name="pageNumber">Page number (default: 1)</param>
    /// <param name="pageSize">Page size (default: 10)</param>
    [HttpGet]
    [ProducesResponseType(typeof(List<LeadListDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<List<LeadListDto>>> GetAllLeads(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        try
        {
            var leads = await _leadService.GetAllLeadsAsync(pageNumber, pageSize);
            return Ok(leads);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error getting leads: {ex.Message}");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving leads");
        }
    }

    /// <summary>
    /// Get lead by ID
    /// </summary>
    /// <param name="id">Lead ID</param>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(LeadDetailDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<LeadDetailDto>> GetLeadById(int id)
    {
        try
        {
            var lead = await _leadService.GetLeadByIdAsync(id);
            if (lead == null)
                return NotFound($"Lead with ID {id} not found");

            return Ok(lead);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error getting lead {id}: {ex.Message}");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving the lead");
        }
    }

    /// <summary>
    /// Get leads by status
    /// </summary>
    /// <param name="status">Lead status (New, Qualified, Contacted, Unqualified, Converted)</param>
    /// <param name="pageNumber">Page number (default: 1)</param>
    /// <param name="pageSize">Page size (default: 10)</param>
    [HttpGet("status/{status}")]
    [ProducesResponseType(typeof(List<LeadListDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<List<LeadListDto>>> GetLeadsByStatus(
        string status,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        try
        {
            var leads = await _leadService.GetLeadsByStatusAsync(status, pageNumber, pageSize);
            return Ok(leads);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error getting leads by status: {ex.Message}");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving leads");
        }
    }

    /// <summary>
    /// Get hot leads
    /// </summary>
    [HttpGet("interest/hot")]
    [ProducesResponseType(typeof(List<LeadListDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<List<LeadListDto>>> GetHotLeads()
    {
        try
        {
            var leads = await _leadService.GetLeadsByInterestLevelAsync("Hot");
            return Ok(leads);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error getting hot leads: {ex.Message}");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving hot leads");
        }
    }

    /// <summary>
    /// Search leads
    /// </summary>
    /// <param name="searchTerm">Search term (name, email, or company)</param>
    /// <param name="pageNumber">Page number (default: 1)</param>
    /// <param name="pageSize">Page size (default: 10)</param>
    [HttpGet("search/{searchTerm}")]
    [ProducesResponseType(typeof(List<LeadListDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<List<LeadListDto>>> SearchLeads(
        string searchTerm,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        try
        {
            var leads = await _leadService.SearchLeadsAsync(searchTerm, pageNumber, pageSize);
            return Ok(leads);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error searching leads: {ex.Message}");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while searching leads");
        }
    }

    /// <summary>
    /// Create a new lead
    /// </summary>
    /// <param name="createDto">Lead data</param>
    [HttpPost]
    [ProducesResponseType(typeof(int), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<int>> CreateLead([FromBody] CreateLeadDto createDto)
    {
        try
        {
            var validationResult = await _createValidator.ValidateAsync(createDto);
            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            var leadId = await _leadService.CreateLeadAsync(createDto);
            return CreatedAtAction(nameof(GetLeadById), new { id = leadId }, leadId);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error creating lead: {ex.Message}");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while creating the lead");
        }
    }

    /// <summary>
    /// Update an existing lead
    /// </summary>
    /// <param name="id">Lead ID</param>
    /// <param name="updateDto">Updated lead data</param>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateLead(int id, [FromBody] UpdateLeadDto updateDto)
    {
        try
        {
            var validationResult = await _updateValidator.ValidateAsync(updateDto);
            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            var result = await _leadService.UpdateLeadAsync(id, updateDto);
            if (!result)
                return NotFound($"Lead with ID {id} not found");

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error updating lead {id}: {ex.Message}");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while updating the lead");
        }
    }

    /// <summary>
    /// Delete a lead
    /// </summary>
    /// <param name="id">Lead ID</param>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteLead(int id)
    {
        try
        {
            var result = await _leadService.DeleteLeadAsync(id);
            if (!result)
                return NotFound($"Lead with ID {id} not found");

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error deleting lead {id}: {ex.Message}");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while deleting the lead");
        }
    }

    /// <summary>
    /// Convert lead to customer
    /// </summary>
    /// <param name="convertDto">Conversion data</param>
    [HttpPost("convert")]
    [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<int>> ConvertLeadToCustomer([FromBody] ConvertLeadToCustomerDto convertDto)
    {
        try
        {
            var validationResult = await _convertValidator.ValidateAsync(convertDto);
            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            var customerId = await _leadService.ConvertLeadToCustomerAsync(convertDto);
            return Ok(customerId);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning($"Lead conversion failed: {ex.Message}");
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error converting lead: {ex.Message}");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while converting the lead");
        }
    }

    /// <summary>
    /// Get total estimated value of all leads
    /// </summary>
    [HttpGet("analytics/total-value")]
    [ProducesResponseType(typeof(decimal), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<decimal>> GetTotalEstimatedValue()
    {
        try
        {
            var totalValue = await _leadService.GetTotalEstimatedValueAsync();
            return Ok(totalValue);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error getting total estimated value: {ex.Message}");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving total value");
        }
    }
}
