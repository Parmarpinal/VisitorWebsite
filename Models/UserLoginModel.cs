using System.ComponentModel.DataAnnotations;

namespace VisitorWebsite.Models
{
    public class UserLoginModel
    {
        [Required(ErrorMessage = "Username is required.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [MinLength(4)]
        [MaxLength(16)]
        public string Password { get; set; }
    }
}
