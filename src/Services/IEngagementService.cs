using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AgenticCopilot.Models;
using AgenticCopilot.DTOs;

namespace AgenticCopilot.Services
{
    /// <summary>
    /// Interface for engagement service operations
    /// </summary>
    public interface IEngagementService
    {
        /// <summary>
        /// Retrieves all engagements
        /// </summary>
        /// <returns>List of all engagements</returns>
        Task<IEnumerable<Engagement>> GetAllEngagementsAsync();

        /// <summary>
        /// Retrieves an engagement by its unique identifier
        /// </summary>
        /// <param name="id">Unique identifier of the engagement</param>
        /// <returns>Engagement if found, null otherwise</returns>
        Task<Engagement?> GetEngagementByIdAsync(Guid id);

        /// <summary>
        /// Creates a new engagement
        /// </summary>
        /// <param name="createDto">Engagement creation data</param>
        /// <returns>Created engagement</returns>
        Task<Engagement> CreateEngagementAsync(CreateEngagementDto createDto);

        /// <summary>
        /// Updates an existing engagement
        /// </summary>
        /// <param name="id">Unique identifier of the engagement to update</param>
        /// <param name="updateDto">Updated engagement data</param>
        /// <returns>Updated engagement if found, null otherwise</returns>
        Task<Engagement?> UpdateEngagementAsync(Guid id, UpdateEngagementDto updateDto);

        /// <summary>
        /// Deletes an engagement
        /// </summary>
        /// <param name="id">Unique identifier of the engagement to delete</param>
        /// <returns>True if deleted, false if not found</returns>
        Task<bool> DeleteEngagementAsync(Guid id);

        /// <summary>
        /// Checks if an engagement exists
        /// </summary>
        /// <param name="id">Unique identifier of the engagement</param>
        /// <returns>True if exists, false otherwise</returns>
        Task<bool> EngagementExistsAsync(Guid id);
    }
}
