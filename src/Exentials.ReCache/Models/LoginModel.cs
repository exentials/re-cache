using System.ComponentModel.DataAnnotations;

namespace Exentials.ReCache.Models
{
    /// <summary>
    /// Authentication data
    /// </summary>
    public class LoginModel
    {
        /// <summary>
        /// User name
        /// </summary>
        [Required]
        public string? Username { get; set; }
        /// <summary>
        /// Password
        /// </summary>
        [Required]
        public string? Password { get; set; }
    }
}
