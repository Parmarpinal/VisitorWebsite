using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Reflection;
using VisitorWebsite.Areas.Host.Models;

namespace VisitorWebsite.Areas.Host.Controllers
{
    [CheckAccess]
    [Area("Host")]
    public class HomeController : BaseController
    {
        public HomeController(IConfiguration configuration) : base(configuration) { }

        #region Index
        public IActionResult Index()
        {
            return View();
        }
        #endregion

        #region EditNotificationToRead
        public async Task<IActionResult> EditNotificationToRead(int id)
        {
            HttpResponseMessage response = await _client.PutAsync($"Notification/{id}", new StringContent(""));

            if (response.IsSuccessStatusCode)
            {
                TempData["ErrorMessage"] = "Error occured.";
            }
            return RedirectToAction("Index");
        }
        #endregion

        #region ProfilePage
        public async Task<IActionResult> Profile()
        {
            HostModel user = new HostModel();
            int? id = CommonVariable.UserID();
            HttpResponseMessage response = await _client.GetAsync($"User/{id}");
            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                user = JsonConvert.DeserializeObject<HostModel>(data);
            }
            return View(user);
        }
        #endregion

    }
}
