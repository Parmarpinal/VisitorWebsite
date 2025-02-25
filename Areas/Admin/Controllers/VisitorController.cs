using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using VisitorWebsite.Areas.Admin.Models;

namespace VisitorWebsite.Areas.Admin.Controllers
{
    [Area("Admin")]
    [CheckAccess]
    public class VisitorController : Controller
    {
        Uri baseAddress = new Uri("http://localhost:5153/api");
        HttpClient _client;

        public VisitorController()
        {
            _client = new HttpClient();
            _client.BaseAddress = baseAddress;
        }
        public IActionResult GetVisitor()
        {
            List<VisitorModel> Visitors = new List<VisitorModel>();
            HttpResponseMessage response = _client.GetAsync($"{_client.BaseAddress}/Visitor").Result;
            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                Visitors = JsonConvert.DeserializeObject<List<VisitorModel>>(data);
            }
            return View("VisitorList", Visitors);
        }
    }
}
