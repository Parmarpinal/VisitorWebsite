using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using VisitorWebsite.Areas.Admin.Models;

namespace VisitorWebsite.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DepartmentController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _client;

        public DepartmentController(IConfiguration configuration)
        {
            _configuration = configuration;
            _client = new HttpClient
            {
                BaseAddress = new System.Uri(_configuration["WebApiBaseUrl"])
            };
        }

        #region GetDepartment
        public async Task<IActionResult> GetDepartment()
        {
            List<DepartmentModel> Departments = new List<DepartmentModel>();
            HttpResponseMessage response = await _client.GetAsync($"Department");
            if (response.IsSuccessStatusCode)
            {
                string data =await response.Content.ReadAsStringAsync();
                Departments = JsonConvert.DeserializeObject<List<DepartmentModel>>(data);
            }
            return View("DepartmentList", Departments);
        }
        #endregion

        #region DepartmentForm
        public async Task<IActionResult> DepartmentForm(int? id)
        {
            if (id.HasValue)
            {
                var response = await _client.GetAsync($"Department/{id}");
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    var Department = JsonConvert.DeserializeObject<DepartmentModel>(data);
                    await LoadOrganization();
                    return View(Department);
                }
            }
            await LoadOrganization();
            return View(new DepartmentModel());
        }
        #endregion

        #region Save

        [HttpPost]
        public async Task<IActionResult> Save(DepartmentModel model)
        {
            if (ModelState.IsValid)
            {
                var json = JsonConvert.SerializeObject(model);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response;

                if (model.DepartmentID == 0)
                    response = await _client.PostAsync($"Department", content);
                else
                    response = await _client.PutAsync($"Department/{model.DepartmentID}", content);

                if (response.IsSuccessStatusCode)
                    return RedirectToAction("GetDepartment");
            }
            await LoadOrganization();
            return View("DepartmentForm", model);
        }
        #endregion

        #region DropDownForOrganization
        public async Task LoadOrganization()
        {
            List<OrganizationDropDownModel> organizations = new List<OrganizationDropDownModel>();
            HttpResponseMessage response = await _client.GetAsync($"DropDown/organizations");
            if (response.IsSuccessStatusCode)
            {
                string data = await response.Content.ReadAsStringAsync();
                organizations = JsonConvert.DeserializeObject<List<OrganizationDropDownModel>>(data);
                ViewBag.OrganizationList = organizations;
            }
        }
        #endregion
    }
}
