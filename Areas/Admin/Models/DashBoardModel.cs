namespace VisitorWebsite.Areas.Admin.Models
{
    public class DashboardCounts
    {
        public string Metric { get; set; }
        public int Value { get; set; }
    }

    public class RecentOrganizationModel
    {
        public int OrganizationID { get; set; }
        public string OrganizationName { get; set; }
        public int DepartmentCount { get; set; }
        public int ReceptionistCount { get; set; }
        public string Head { get; set; }
        public string City { get; set; }
        public DateTime EstablishedDate { get; set; }
    }

    public class RecentDepartmentModel
    {
        public int DepartmentID { get; set; }
        public string DepartmentName { get; set; }
        public int OrganizationID { get; set; }
        public string OrganizationName { get; set; }
        public int HostCount { get; set; }
    }

    public class TopVisitorModel
    {
        public int VisitorID { get; set; }
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string Purpose { get; set; }
        public int HostID { get; set; }
        public string HostName { get; set; }
        public DateTime VisitDate { get; set; }
        public TimeOnly? CheckIn { get; set; }
        public TimeOnly? CheckOut { get; set; }
    }
    public class TopHostModel
    {
        public int HostID { get; set; }
        public string HostName { get; set; }
        public int OrganizationID { get; set; }
        public int DepartmentID { get; set; }
        public string OrganizationName { get; set; }
        public string DepartmentName { get; set; }
        public int VisitorCount { get; set; }
        public DateTime JoiningDate { get; set; }
    }

    public class TopReceptionistModel
    {
        public int ReceptionistID { get; set; }
        public string ReceptionistName { get; set; }
        public int OrganizationID { get; set; }
        public string OrganizationName { get; set; }
        public DateTime JoiningDate { get; set; }
    }

    public class DashBoardModel
    {
        public List<DashboardCounts> Counts { get; set; }
        public List<RecentOrganizationModel> RecentOrganizations { get; set; }
        public List<RecentDepartmentModel> RecentDepartments { get; set; }
        public List<TopReceptionistModel> TopReceptionists { get; set; }
        public List<TopHostModel> TopHosts { get; set; }
        public List<TopVisitorModel> TopVisitors { get; set; }
    }
}
