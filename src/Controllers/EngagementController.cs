using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using AgenticCopilot.Models;
using AgenticCopilot.DTOs;
using AgenticCopilot.Services;

namespace AgenticCopilot.Controllers
{
    /// <summary>
    /// Controller for managing engagements with full CRUD operations
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class EngagementController : ControllerBase
    {
        private readonly IEngagementService _engagementService;
        private readonly ILogger<EngagementController> _logger;

        /// <summary>
        /// Initializes a new instance of the EngagementController
        /// </summary>
        /// <param name="engagementService">Engagement service instance</param>
        /// <param name="logger">Logger instance</param>
        public EngagementController(
            IEngagementService engagementService,
            ILogger<EngagementController> logger)
        {
            _engagementService = engagementService ?? throw new ArgumentNullException(nameof(engagementService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Get all engagements
        /// </summary>
        /// <returns>List of all engagements</returns>
        /// <response code="200">Returns the list of engagements</response>
        /// <response code="500">Internal server error</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Engagement>), 200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<IEnumerable<Engagement>>> GetAllEngagements()
        {
            try
            {
                _logger.LogInformation("Retrieving all engagements");
                var engagements = await _engagementService.GetAllEngagementsAsync();
                return Ok(engagements);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all engagements");
                return StatusCode(500, "An error occurred while retrieving engagements");
            }
        }

        /// <summary>
        /// Get an engagement by ID
        /// </summary>
        /// <param name="id">Unique identifier of the engagement</param>
        /// <returns>The requested engagement</returns>
        /// <response code="200">Returns the engagement</response>
        /// <response code="404">Engagement not found</response>
        /// <response code="500">Internal server error</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Engagement), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<Engagement>> GetEngagementById(Guid id)
        {
            try
            {
                _logger.LogInformation("Retrieving engagement with ID: {EngagementId}", id);

                var engagement = await _engagementService.GetEngagementByIdAsync(id);

                if (engagement == null)
                {
                    _logger.LogWarning("Engagement with ID: {EngagementId} not found", id);
                    return NotFound($"Engagement with ID {id} not found");
                }

                return Ok(engagement);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving engagement with ID: {EngagementId}", id);
                return StatusCode(500, "An error occurred while retrieving the engagement");
            }
        }

        /// <summary>
        /// Create a new engagement
        /// </summary>
        /// <param name="createDto">Engagement creation data</param>
        /// <returns>The created engagement</returns>
        /// <response code="201">Engagement created successfully</response>
        /// <response code="400">Invalid request data</response>
        /// <response code="500">Internal server error</response>
        [HttpPost]
        [ProducesResponseType(typeof(Engagement), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<Engagement>> CreateEngagement([FromBody] CreateEngagementDto createDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Invalid model state for engagement creation");
                    return BadRequest(ModelState);
                }

                _logger.LogInformation("Creating new engagement: {EngagementName}", createDto.Name);

                var engagement = await _engagementService.CreateEngagementAsync(createDto);

                _logger.LogInformation("Engagement created with ID: {EngagementId}", engagement.Id);

                return CreatedAtAction(
                    nameof(GetEngagementById),
                    new { id = engagement.Id },
                    engagement);
            }
            catch (ArgumentNullException ex)
            {
                _logger.LogError(ex, "Null argument provided for engagement creation");
                return BadRequest("Invalid engagement data");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating engagement");
                return StatusCode(500, "An error occurred while creating the engagement");
            }
        }

        /// <summary>
        /// Update an existing engagement
        /// </summary>
        /// <param name="id">Unique identifier of the engagement to update</param>
        /// <param name="updateDto">Updated engagement data</param>
        /// <returns>The updated engagement</returns>
        /// <response code="200">Engagement updated successfully</response>
        /// <response code="400">Invalid request data</response>
        /// <response code="404">Engagement not found</response>
        /// <response code="500">Internal server error</response>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(Engagement), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<Engagement>> UpdateEngagement(Guid id, [FromBody] UpdateEngagementDto updateDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Invalid model state for engagement update");
                    return BadRequest(ModelState);
                }

                _logger.LogInformation("Updating engagement with ID: {EngagementId}", id);

                var engagement = await _engagementService.UpdateEngagementAsync(id, updateDto);

                if (engagement == null)
                {
                    _logger.LogWarning("Engagement with ID: {EngagementId} not found for update", id);
                    return NotFound($"Engagement with ID {id} not found");
                }

                _logger.LogInformation("Engagement with ID: {EngagementId} updated successfully", id);

                return Ok(engagement);
            }
            catch (ArgumentNullException ex)
            {
                _logger.LogError(ex, "Null argument provided for engagement update");
                return BadRequest("Invalid engagement data");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating engagement with ID: {EngagementId}", id);
                return StatusCode(500, "An error occurred while updating the engagement");
            }
        }

        /// <summary>
        /// Delete an engagement
        /// </summary>
        /// <param name="id">Unique identifier of the engagement to delete</param>
        /// <returns>No content on success</returns>
        /// <response code="204">Engagement deleted successfully</response>
        /// <response code="404">Engagement not found</response>
        /// <response code="500">Internal server error</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeleteEngagement(Guid id)
        {
            try
            {
                _logger.LogInformation("Deleting engagement with ID: {EngagementId}", id);

                var result = await _engagementService.DeleteEngagementAsync(id);

                if (!result)
                {
                    _logger.LogWarning("Engagement with ID: {EngagementId} not found for deletion", id);
                    return NotFound($"Engagement with ID {id} not found");
                }

                _logger.LogInformation("Engagement with ID: {EngagementId} deleted successfully", id);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting engagement with ID: {EngagementId}", id);
                return StatusCode(500, "An error occurred while deleting the engagement");
            }
        }

        /// <summary>
        /// Check if an engagement exists
        /// </summary>
        /// <param name="id">Unique identifier of the engagement</param>
        /// <returns>Boolean indicating existence</returns>
        /// <response code="200">Returns existence status</response>
        /// <response code="500">Internal server error</response>
        [HttpHead("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> EngagementExists(Guid id)
        {
            try
            {
                var exists = await _engagementService.EngagementExistsAsync(id);

                if (exists)
                {
                    return Ok();
                }

                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking engagement existence with ID: {EngagementId}", id);
                return StatusCode(500, "An error occurred while checking engagement existence");
            }
        }
    }
}
