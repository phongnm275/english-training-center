using Microsoft.AspNetCore.Mvc;
using FluentValidation;
using EnglishTrainingCenter.Application.DTOs.CRM;
using EnglishTrainingCenter.Application.Services.CRM;

namespace EnglishTrainingCenter.API.Controllers.CRM;

/// <summary>
/// CRM Opportunity Management API
/// </summary>
[ApiController]
[Route("api/v1/crm/[controller]")]
[Produces("application/json")]
public class OpportunitiesController : ControllerBase
{
    private readonly IOpportunityService _opportunityService;
    private readonly IValidator<CreateOpportunityDto> _createValidator;
    private readonly IValidator<UpdateOpportunityDto> _updateValidator;
    private readonly IValidator<UpdateOpportunityStageDto> _stageValidator;
    private readonly ILogger<OpportunitiesController> _logger;

    public OpportunitiesController(
        IOpportunityService opportunityService,
        IValidator<CreateOpportunityDto> createValidator,
        IValidator<UpdateOpportunityDto> updateValidator,
        IValidator<UpdateOpportunityStageDto> stageValidator,
        ILogger<OpportunitiesController> logger)
    {
        _opportunityService = opportunityService;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
        _stageValidator = stageValidator;
        _logger = logger;
    }

    /// <summary>
    /// Get all opportunities
    /// </summary>
    /// <param name="pageNumber">Page number (default: 1)</param>
    /// <param name="pageSize">Page size (default: 10)</param>
    [HttpGet]
    [ProducesResponseType(typeof(List<OpportunityListDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<List<OpportunityListDto>>> GetAllOpportunities(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        try
        {
            var opportunities = await _opportunityService.GetAllOpportunitiesAsync(pageNumber, pageSize);
            return Ok(opportunities);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error getting opportunities: {ex.Message}");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving opportunities");
        }
    }

    /// <summary>
    /// Get opportunity by ID
    /// </summary>
    /// <param name="id">Opportunity ID</param>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(OpportunityDetailDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<OpportunityDetailDto>> GetOpportunityById(int id)
    {
        try
        {
            var opportunity = await _opportunityService.GetOpportunityByIdAsync(id);
            if (opportunity == null)
                return NotFound($"Opportunity with ID {id} not found");

            return Ok(opportunity);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error getting opportunity {id}: {ex.Message}");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving the opportunity");
        }
    }

    /// <summary>
    /// Get opportunities by stage
    /// </summary>
    /// <param name="stage">Stage (Prospect, Qualification, Proposal, Negotiation, Closed Won, Closed Lost)</param>
    [HttpGet("stage/{stage}")]
    [ProducesResponseType(typeof(List<OpportunityListDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<List<OpportunityListDto>>> GetOpportunitiesByStage(string stage)
    {
        try
        {
            var opportunities = await _opportunityService.GetOpportunitiesByStageAsync(stage);
            return Ok(opportunities);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error getting opportunities by stage: {ex.Message}");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving opportunities");
        }
    }

    /// <summary>
    /// Get opportunities by customer
    /// </summary>
    /// <param name="customerId">Customer ID</param>
    [HttpGet("customer/{customerId}")]
    [ProducesResponseType(typeof(List<OpportunityListDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<List<OpportunityListDto>>> GetOpportunitiesByCustomer(int customerId)
    {
        try
        {
            var opportunities = await _opportunityService.GetOpportunitiesByCustomerAsync(customerId);
            return Ok(opportunities);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error getting opportunities by customer: {ex.Message}");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving opportunities");
        }
    }

    /// <summary>
    /// Search opportunities
    /// </summary>
    /// <param name="searchTerm">Search term (name or description)</param>
    /// <param name="pageNumber">Page number (default: 1)</param>
    /// <param name="pageSize">Page size (default: 10)</param>
    [HttpGet("search/{searchTerm}")]
    [ProducesResponseType(typeof(List<OpportunityListDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<List<OpportunityListDto>>> SearchOpportunities(
        string searchTerm,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        try
        {
            var opportunities = await _opportunityService.SearchOpportunitiesAsync(searchTerm, pageNumber, pageSize);
            return Ok(opportunities);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error searching opportunities: {ex.Message}");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while searching opportunities");
        }
    }

    /// <summary>
    /// Create a new opportunity
    /// </summary>
    /// <param name="createDto">Opportunity data</param>
    [HttpPost]
    [ProducesResponseType(typeof(int), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<int>> CreateOpportunity([FromBody] CreateOpportunityDto createDto)
    {
        try
        {
            var validationResult = await _createValidator.ValidateAsync(createDto);
            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            var opportunityId = await _opportunityService.CreateOpportunityAsync(createDto);
            return CreatedAtAction(nameof(GetOpportunityById), new { id = opportunityId }, opportunityId);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error creating opportunity: {ex.Message}");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while creating the opportunity");
        }
    }

    /// <summary>
    /// Update an existing opportunity
    /// </summary>
    /// <param name="id">Opportunity ID</param>
    /// <param name="updateDto">Updated opportunity data</param>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateOpportunity(int id, [FromBody] UpdateOpportunityDto updateDto)
    {
        try
        {
            var validationResult = await _updateValidator.ValidateAsync(updateDto);
            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            var result = await _opportunityService.UpdateOpportunityAsync(id, updateDto);
            if (!result)
                return NotFound($"Opportunity with ID {id} not found");

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error updating opportunity {id}: {ex.Message}");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while updating the opportunity");
        }
    }

    /// <summary>
    /// Update opportunity stage
    /// </summary>
    /// <param name="id">Opportunity ID</param>
    /// <param name="stageDto">Stage update data</param>
    [HttpPatch("{id}/stage")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateOpportunityStage(int id, [FromBody] UpdateOpportunityStageDto stageDto)
    {
        try
        {
            var validationResult = await _stageValidator.ValidateAsync(stageDto);
            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            var result = await _opportunityService.UpdateOpportunityStageAsync(id, stageDto);
            if (!result)
                return NotFound($"Opportunity with ID {id} not found");

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error updating opportunity stage: {ex.Message}");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while updating the opportunity stage");
        }
    }

    /// <summary>
    /// Delete an opportunity
    /// </summary>
    /// <param name="id">Opportunity ID</param>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteOpportunity(int id)
    {
        try
        {
            var result = await _opportunityService.DeleteOpportunityAsync(id);
            if (!result)
                return NotFound($"Opportunity with ID {id} not found");

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error deleting opportunity {id}: {ex.Message}");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while deleting the opportunity");
        }
    }

    /// <summary>
    /// Get total pipeline value
    /// </summary>
    [HttpGet("analytics/pipeline-value")]
    [ProducesResponseType(typeof(decimal), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<decimal>> GetTotalPipelineValue()
    {
        try
        {
            var totalValue = await _opportunityService.GetTotalPipelineValueAsync();
            return Ok(totalValue);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error getting pipeline value: {ex.Message}");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving pipeline value");
        }
    }

    /// <summary>
    /// Get overdue opportunities
    /// </summary>
    [HttpGet("overdue/list")]
    [ProducesResponseType(typeof(List<OpportunityListDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<List<OpportunityListDto>>> GetOverdueOpportunities()
    {
        try
        {
            var opportunities = await _opportunityService.GetOverdueOpportunitiesAsync();
            return Ok(opportunities);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error getting overdue opportunities: {ex.Message}");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving overdue opportunities");
        }
    }
}
