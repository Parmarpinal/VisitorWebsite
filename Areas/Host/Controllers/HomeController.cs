using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using VisitorWebsite.Areas.Host.Models;

namespace VisitorWebsite.Areas.Host.Controllers
{
    [Area("Host")]
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

        #region Index
        public async Task<IActionResult> Index(int id)
        {
            HostModel user = new HostModel();
            HttpResponseMessage response = await _client.GetAsync($"User/specific/{id}");
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
