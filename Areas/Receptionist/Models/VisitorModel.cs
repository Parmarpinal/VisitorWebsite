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

        public string? ImagePath { get; set; }   

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
        public DateTime VisitDate { get; set; }

        public TimeSpan? CheckIn { get; set; }

        public TimeSpan? CheckOut { get; set; }
        public string? OrganizationName { get; set; }
        public string? DepartmentName { get; set; }
    }
}
