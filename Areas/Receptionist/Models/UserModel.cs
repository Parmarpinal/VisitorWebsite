namespace VisitorWebsite.Areas.Receptionist.Models
{
    public class UserModel
    {
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string? ImagePath { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string MobileNo { get; set; }
        public string Gender { get; set; }
        public int Age { get; set; }
        public string City { get; set; }
        public DateTime? JoiningDate { get; set; }
    }
}
