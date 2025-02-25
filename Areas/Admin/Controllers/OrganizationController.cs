using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using VisitorWebsite.Areas.Admin.Models;

namespace VisitorWebsite.Areas.Admin.Controllers
{
    [Area("Admin")]
    [CheckAccess]
    public class OrganizationController : Controller
    {
        Uri baseAddress = new Uri("http://localhost:5153/api");
        HttpClient _client;

        public OrganizationController()
        {
            _client = new HttpClient();
            _client.BaseAddress = baseAddress;
        }
        public IActionResult GetOrganization()
        {
            List<OrganizationModel> organizations = new List<OrganizationModel>();
            HttpResponseMessage response = _client.GetAsync($"{_client.BaseAddress}/Organization").Result;
            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                organizations = JsonConvert.DeserializeObject<List<OrganizationModel>>(data);
            }
            return View("OrganizationList", organizations);
        }
        public async Task<IActionResult> OrganizationForm(int? organizationID)
        {
            if (organizationID.HasValue)
            {
                var response = await _client.GetAsync($"{_client.BaseAddress}/Organization/{organizationID}");
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    var organization = JsonConvert.DeserializeObject<OrganizationModel>(data);
                    return View(organization);
                }
            }
            return View(new OrganizationModel());
        }

        [HttpPost]
        public async Task<IActionResult> Save(OrganizationModel model)
        {
            if (model.EstablishedDate == DateTime.MinValue)
            {
                model.EstablishedDate = DateTime.Now;
            }

            if (ModelState.IsValid)
            {
                var json = JsonConvert.SerializeObject(model);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response;

                if (model.OrganizationID == 0)
                    response = await _client.PostAsync($"{_client.BaseAddress}/Organization", content);
                else
                    response = await _client.PutAsync($"{_client.BaseAddress}/Organization/{model.OrganizationID}", content);

                if (response.IsSuccessStatusCode)
                    return RedirectToAction("GetOrganization");
            }
            return View("OrganizationForm", model);
        }

    }
}
