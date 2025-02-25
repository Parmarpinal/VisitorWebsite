namespace VisitorWebsite.Models
{
    public class VisitorModel
    {
        public int VisitorID { get; set; }
        public int OrganizationID { get; set; }
        public int DepartmentID { get; set; }
        public string Purpose { get; set; }
        public int UserID { get; set; }
        public string? VisitorName { get; set; }
        public int HostID { get; set; }
        public string? HostName { get; set; }
        public int NoOfPerson { get; set; }
        public string IDProof { get; set; }
        public string IDProofNumber { get; set; }
        public DateTime VisitDate { get; set; }
        public TimeOnly? CheckIn { get; set; }
        public TimeOnly? CheckOut { get; set; }
        public string? OrganizationName { get; set; }
        public string? DepartmentName { get; set; }


        public string? VisitorImage { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? Head { get; set; }
    }
}
