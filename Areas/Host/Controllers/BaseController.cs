using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using System.Configuration;
using VisitorWebsite.Areas.Host.Models;

namespace VisitorWebsite.Areas.Host.Controllers
{
    public class BaseController : Controller
    {
        protected readonly HttpClient _client; 
        protected readonly IConfiguration _configuration;

        public BaseController(IConfiguration configuration)
        {
            _configuration = configuration;
            _client = new HttpClient
            {
                BaseAddress = new System.Uri(_configuration["WebApiBaseUrl"])
            };
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            GetNotifications(); // Fetch notifications before any action executes
            base.OnActionExecuting(context);
        }

        #region GetNotifications
        public void GetNotifications()
        {
            List<NotificationModel> notifications = new List<NotificationModel>();
            int? id = CommonVariable.UserID();
            HttpResponseMessage response = _client.GetAsync($"Notification/GetNotificationsByHostID/{id}").Result;
            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                notifications = JsonConvert.DeserializeObject<List<NotificationModel>>(data);
            }
            List<NotificationModel> newNotifications = notifications.Where(x => x.IsRead == false).ToList();
            ViewBag.Notifications = newNotifications;
        }
        #endregion
    }
}
