using System.ComponentModel.DataAnnotations;

namespace AgenticCopilot.DTOs
{
    /// <summary>
    /// Data Transfer Object for creating a new engagement
    /// </summary>
    public class CreateEngagementDto
    {
        /// <summary>
        /// Name of the engagement
        /// </summary>
        [Required(ErrorMessage = "Name is required")]
        [StringLength(200, MinimumLength = 3, ErrorMessage = "Name must be between 3 and 200 characters")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Client name associated with the engagement
        /// </summary>
        [Required(ErrorMessage = "ClientName is required")]
        [StringLength(200, MinimumLength = 2, ErrorMessage = "ClientName must be between 2 and 200 characters")]
        public string ClientName { get; set; } = string.Empty;

        /// <summary>
        /// Partner name associated with the engagement
        /// </summary>
        [Required(ErrorMessage = "PartnerName is required")]
        [StringLength(200, MinimumLength = 2, ErrorMessage = "PartnerName must be between 2 and 200 characters")]
        public string PartnerName { get; set; } = string.Empty;
    }
}
