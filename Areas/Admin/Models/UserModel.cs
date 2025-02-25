using System.ComponentModel.DataAnnotations;

namespace VisitorWebsite.Areas.Admin.Models
{
    public class UserModel
    {
        public int UserID { get; set; }
        public int UserTypeID { get; set; }
        public string? UserTypeName { get; set; }

        [Required]
        [Display(Name ="User name")]
        [MaxLength(100)]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public string? ImagePath { get; set; }

        public IFormFile? ImageFile { get; set; }

        [Required]
        [MinLength(4)]
        [MaxLength(8)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Mobile number should not be empty")]
        [Display(Name = "Mobile number")]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Please enter a valid mobile number.")]
        public string MobileNo { get; set; }

        [Required]
        public string Gender { get; set; }

        [Required]
        [Range(10,100)]
        public int Age { get; set; }

        [Required]
        [MaxLength(100)]
        public string City { get; set; }

        [Display(Name = "Organization name")]
        public int? OrganizationID { get; set; }
        public string? OrganizationName { get; set; }
        public int? DepartmentID { get; set; }

        [Display(Name = "Department name")]
        public string? DepartmentName { get; set; }
        public DateTime? JoiningDate { get; set; }
    }
}
