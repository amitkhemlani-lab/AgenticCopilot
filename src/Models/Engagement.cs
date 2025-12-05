using System;
using System.ComponentModel.DataAnnotations;

namespace AgenticCopilot.Models
{
    /// <summary>
    /// Represents an engagement entity with client and partner information
    /// </summary>
    public class Engagement
    {
        /// <summary>
        /// Unique identifier for the engagement
        /// </summary>
        [Key]
        public Guid Id { get; set; }

        /// <summary>
        /// Name of the engagement
        /// </summary>
        [Required]
        [StringLength(200, MinimumLength = 3)]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Client name associated with the engagement
        /// </summary>
        [Required]
        [StringLength(200, MinimumLength = 2)]
        public string ClientName { get; set; } = string.Empty;

        /// <summary>
        /// Partner name associated with the engagement
        /// </summary>
        [Required]
        [StringLength(200, MinimumLength = 2)]
        public string PartnerName { get; set; } = string.Empty;

        /// <summary>
        /// Timestamp when the engagement was created
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Timestamp when the engagement was last updated
        /// </summary>
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
