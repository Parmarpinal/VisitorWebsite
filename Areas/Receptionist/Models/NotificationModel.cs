namespace VisitorWebsite.Areas.Receptionist.Models
{
    public class NotificationModel
    {
        public int NotificationID { get; set; }
        public int HostID { get; set; }
        public string Message { get; set; }
        public int VisitorID { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsRead { get; set; }
    }
}
