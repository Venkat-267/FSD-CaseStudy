using System.ComponentModel.DataAnnotations;

namespace PayRollManagement.Models
{
    public class RefreshTokenRequest
    {
        [Required]
        public string RefreshToken { get; set; }
    }
}
