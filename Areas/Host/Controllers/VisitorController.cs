using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using VisitorWebsite.Areas.Host.Models;

namespace VisitorWebsite.Areas.Host.Controllers
{
    [Area("Host")]
    [CheckAccess]
    public class VisitorController : BaseController
    {
        public VisitorController(IConfiguration configuration) : base(configuration) { }

        public IActionResult GetVisitor()
        {
            List<VisitorModel> Visitors = new List<VisitorModel>();
            HttpResponseMessage response = _client.GetAsync($"Visitor").Result;
            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                Visitors = JsonConvert.DeserializeObject<List<VisitorModel>>(data);
            }
            int? id = CommonVariable.UserID();
            List<VisitorModel> newVisitors = Visitors.Where(x => x.HostID == id).ToList();
            return View("VisitorList", newVisitors);
        }
    }
}
