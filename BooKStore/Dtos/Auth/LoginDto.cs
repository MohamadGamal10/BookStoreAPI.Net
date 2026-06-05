using System.ComponentModel.DataAnnotations;

namespace BooKStore.Dtos.Auth
{
    public class LoginDto
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters long.")]
        public string Password { get; set; }
    }
}
