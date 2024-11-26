using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DataAccess.DTO
{
    public class UProfileDTO
    {
        /// <summary>
        /// Profile picture of the user.
        /// </summary>
        [JsonPropertyName("avatar")]
        public IFormFile? Avatar { get; set; }

        /// <summary>
        /// Full name of the user.
        /// </summary>
        [JsonPropertyName("fullName")]
        [Required(ErrorMessage = "Full Name is required.")]
        [StringLength(100, ErrorMessage = "Full Name cannot exceed 100 characters.")]
        public string FullName { get; set; } = string.Empty;

        /// <summary>
        /// Contact phone number.
        /// </summary>
        [JsonPropertyName("phone")]
        [Required(ErrorMessage = "Phone number is required.")]
        [Phone(ErrorMessage = "Invalid phone number format.")]
        public string Phone { get; set; } = string.Empty;

        /// <summary>
        /// City of residence.
        /// </summary>
        [JsonPropertyName("city")]
        [Required(ErrorMessage = "City is required.")]
        public string City { get; set; } = string.Empty;

        /// <summary>
        /// Ward of residence.
        /// </summary>
        [JsonPropertyName("ward")]
        [Required(ErrorMessage = "Ward is required.")]
        public string Ward { get; set; } = string.Empty;

        /// <summary>
        /// District of residence.
        /// </summary>
        [JsonPropertyName("district")]
        [Required(ErrorMessage = "District is required.")]
        public string District { get; set; } = string.Empty;

        /// <summary>
        /// Detailed address.
        /// </summary>
        [JsonPropertyName("address")]
        [Required(ErrorMessage = "Address is required.")]
        [StringLength(200, ErrorMessage = "Address cannot exceed 200 characters.")]
        public string Address { get; set; } = string.Empty;
    }
}
