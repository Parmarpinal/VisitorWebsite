using System.ComponentModel.DataAnnotations;

namespace VisitorWebsite.Areas.Admin.Models
{
    public class DepartmentModel
    {
        public int DepartmentID { get; set; }

        [Required(ErrorMessage = "Department name must not be null.")]
        public string DepartmentName { get; set; }

        [Required]
        [Display(Name ="Organization name")]
        public int OrganizationID { get; set; }
        public string? OrganizationName { get; set; }

        public int? HostCount { get; set; }
    }

    public class DepartmentDropDownModel
    {
        public int DepartmentID { get; set; }
        public string DepartmentName { get; set; }
    }
}
