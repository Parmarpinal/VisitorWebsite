using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace VisitorWebsite.Areas.Admin.Models
{
    public class OrganizationModel
    {
        public int OrganizationID { get; set; }

        [Required]
        [Display(Name = "Organization name")]
        public string OrganizationName { get; set; }

        [Required]
        [MaxLength(50)]
        public string Head { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        [MaxLength(500)]
        public string Address { get; set; }

        [Required]
        [Display(Name = "Date of establishment")]
        public DateTime EstablishedDate { get; set; }

        public int? ReceptionistCount { get; set; }
    }
    public class OrganizationDropDownModel
    {
        public int OrganizationID { get; set; }
        public string OrganizationName { get; set; }
    }
}
