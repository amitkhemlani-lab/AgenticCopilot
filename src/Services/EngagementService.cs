using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AgenticCopilot.Models;
using AgenticCopilot.DTOs;

namespace AgenticCopilot.Services
{
    /// <summary>
    /// In-memory implementation of engagement service for demonstration
    /// In production, this would interact with a database through a repository pattern
    /// </summary>
    public class EngagementService : IEngagementService
    {
        private readonly List<Engagement> _engagements;
        private readonly object _lock = new object();

        public EngagementService()
        {
            _engagements = new List<Engagement>();
        }

        /// <inheritdoc />
        public Task<IEnumerable<Engagement>> GetAllEngagementsAsync()
        {
            lock (_lock)
            {
                return Task.FromResult(_engagements.AsEnumerable());
            }
        }

        /// <inheritdoc />
        public Task<Engagement> GetEngagementByIdAsync(Guid id)
        {
            lock (_lock)
            {
                var engagement = _engagements.FirstOrDefault(e => e.Id == id);
                return Task.FromResult(engagement);
            }
        }

        /// <inheritdoc />
        public Task<Engagement> CreateEngagementAsync(CreateEngagementDto createDto)
        {
            if (createDto == null)
                throw new ArgumentNullException(nameof(createDto));

            var engagement = new Engagement
            {
                Id = Guid.NewGuid(),
                Name = createDto.Name,
                ClientName = createDto.ClientName,
                PartnerName = createDto.PartnerName,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            lock (_lock)
            {
                _engagements.Add(engagement);
            }

            return Task.FromResult(engagement);
        }

        /// <inheritdoc />
        public Task<Engagement> UpdateEngagementAsync(Guid id, UpdateEngagementDto updateDto)
        {
            if (updateDto == null)
                throw new ArgumentNullException(nameof(updateDto));

            lock (_lock)
            {
                var engagement = _engagements.FirstOrDefault(e => e.Id == id);
                if (engagement == null)
                    return Task.FromResult<Engagement>(null);

                engagement.Name = updateDto.Name;
                engagement.ClientName = updateDto.ClientName;
                engagement.PartnerName = updateDto.PartnerName;
                engagement.UpdatedAt = DateTime.UtcNow;

                return Task.FromResult(engagement);
            }
        }

        /// <inheritdoc />
        public Task<bool> DeleteEngagementAsync(Guid id)
        {
            lock (_lock)
            {
                var engagement = _engagements.FirstOrDefault(e => e.Id == id);
                if (engagement == null)
                    return Task.FromResult(false);

                _engagements.Remove(engagement);
                return Task.FromResult(true);
            }
        }

        /// <inheritdoc />
        public Task<bool> EngagementExistsAsync(Guid id)
        {
            lock (_lock)
            {
                var exists = _engagements.Any(e => e.Id == id);
                return Task.FromResult(exists);
            }
        }
    }
}
