using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace VisitorWebsite.Areas.Receptionist.Models
{
    public class VisitorModel
    {
        public int VisitorID { get; set; }

        [Required(ErrorMessage = "Organization name must not be null.")]
        public int OrganizationID { get; set; }

        [Required(ErrorMessage = "Department name must not be null.")]
        public int DepartmentID { get; set; }

        [Required]
        [MaxLength(500)]
        public string Purpose { get; set; }
        [Required]
        [Display(Name = "User name")]
        public int UserID { get; set; }

        public string? VisitorName { get; set; }
        [Required]
        [Display(Name = "Host name")]
        public int HostID { get; set; }
        public string? HostName { get; set; }

        [Required]
        [Range(1,50)]
        [Display(Name = "No. of person")]
        public int NoOfPerson { get; set; }
        [Required(ErrorMessage = "ID proof must not be null.")]
        public string IDProof { get; set; }

        [Required(ErrorMessage ="ID proof number must not be null.")]
        [MinLength(4)]
        [MaxLength(20)]
        public string IDProofNumber { get; set; }
        public DateTime? VisitDate { get; set; }

        public TimeSpan? CheckIn { get; set; }

        public TimeSpan? CheckOut { get; set; }
        public string? OrganizationName { get; set; }
        public string? DepartmentName { get; set; }
    }
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
