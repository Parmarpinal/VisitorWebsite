using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using VisitorWebsite.Areas.Receptionist.Models;

namespace VisitorWebsite.Areas.Receptionist.Controllers
{
    [Area("Receptionist")]
    public class HomeController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _client;

        public HomeController(IConfiguration configuration)
        {
            _configuration = configuration;
            _client = new HttpClient
            {
                BaseAddress = new System.Uri(_configuration["WebApiBaseUrl"])
            };
        }

        #region ProfilePage
        public async Task<IActionResult> Index(int id)
        {
            ReceptionistModel user = new ReceptionistModel();
            HttpResponseMessage response = await _client.GetAsync($"User/specific/{id}");
            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                user = JsonConvert.DeserializeObject<ReceptionistModel>(data);
            }
            return View(user);
        }
        #endregion
    }
}
