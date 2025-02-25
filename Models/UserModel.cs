using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VisitorWebsite.Models
{
    public class UserModel
    {
        public int UserID { get; set; }
        public int UserTypeID { get; set; }
        [Required]
        [MaxLength(50)]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        
        public string? ImagePath { get; set; }

        // Image File for Upload
        // This ensures it's not mapped to the database
        public IFormFile? ImageFile { get; set; }

        [Required]
        [MinLength(4)]
        [MaxLength(8)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$",
                   ErrorMessage = "Entered phone format is not valid.")]
        public string MobileNo { get; set; }

        [Required]
        public string Gender { get; set; }

        [Required]
        [Range(1, 100, ErrorMessage = "Age must be between 1 and 120.")]
        public int Age { get; set; }

        [Required]
        public string City { get; set; }
        public DateTime? JoiningDate { get; set; }
        public int? OrganizationID {  get; set; }
        public int? DepartmentID { get; set; }
    }
}
