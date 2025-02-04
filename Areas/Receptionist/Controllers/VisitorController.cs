using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using VisitorWebsite.Areas.Receptionist.Models;

namespace VisitorWebsite.Areas.Receptionist.Controllers
{
    [Area("Receptionist")]
    public class VisitorController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _client;

        public VisitorController(IConfiguration configuration)
        {
            _configuration = configuration;
            _client = new HttpClient
            {
                BaseAddress = new System.Uri(_configuration["WebApiBaseUrl"])
            };
        }

        #region GetVisitors
        public async Task<IActionResult> GetVisitor()
        {
            List<VisitorModel> Visitors = new List<VisitorModel>();
            HttpResponseMessage response = await _client.GetAsync("Visitor");
            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                Visitors = JsonConvert.DeserializeObject<List<VisitorModel>>(data);
            }
            var v = Visitors.Where(x => x.CheckIn != null && x.CheckOut == null);
            return View("VisitorListForCheckedIn", v);
        }
        #endregion

        #region GetVisitors
        public async Task<IActionResult> GetAllVisitor()
        {
            List<VisitorModel> Visitors = new List<VisitorModel>();
            HttpResponseMessage response = await _client.GetAsync("Visitor");
            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                Visitors = JsonConvert.DeserializeObject<List<VisitorModel>>(data);
            }
            return View("VisitorList", Visitors);
        }
        #endregion

        #region UserDropDown
        public async Task UserDropDown()
        {
            List<UserDropDownModel> users = new List<UserDropDownModel>();
            HttpResponseMessage response = await _client.GetAsync($"DropDown/users");
            if (response.IsSuccessStatusCode)
            {
                string data = await response.Content.ReadAsStringAsync();
                users = JsonConvert.DeserializeObject<List<UserDropDownModel>>(data);
                ViewBag.UserList = users;
            }
        }
        #endregion

        #region OrganizationDropDown
        public async Task OrganizationDropDown()
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

        #region LoadDepartment
        public async Task<List<DepartmentDropDownModel>> LoadDepartment(int id)
        {
            List<DepartmentDropDownModel> departments = new List<DepartmentDropDownModel>();
            HttpResponseMessage response = await _client.GetAsync($"DropDown/departments/{id}");
            if (response.IsSuccessStatusCode)
            {
                string data = await response.Content.ReadAsStringAsync();
                departments = JsonConvert.DeserializeObject<List<DepartmentDropDownModel>>(data);
            }
            return departments;
        }
        #endregion

        #region GetDepartmentByOrganizationID
        [HttpPost]
        public async Task<JsonResult> GetDepartmentByOrganizationID(int id)
        {
            List<DepartmentDropDownModel> departments = await LoadDepartment(id);
            return Json(departments);
        }
        #endregion

        #region LoadHost
        public async Task<List<HostDropDownModel>> LoadHost(int id)
        {
            List<HostDropDownModel> hosts = new List<HostDropDownModel>();
            HttpResponseMessage response = await _client.GetAsync($"DropDown/hosts/{id}");
            if (response.IsSuccessStatusCode)
            {
                string data = await response.Content.ReadAsStringAsync();
                hosts = JsonConvert.DeserializeObject<List<HostDropDownModel>>(data);
            }
            return hosts;
        }
        #endregion

        #region GetHostByDepartmentID
        [HttpPost]
        public async Task<JsonResult> GetHostByDepartmentID(int id)
        {
            List<HostDropDownModel> hosts = await LoadHost(id);
            return Json(hosts);
        }
        #endregion

        #region VisitorForm
        public async Task<IActionResult> VisitorForm()
        {
            await OrganizationDropDown();
            await UserDropDown();
            return View(new VisitorModel() {
            });
        }
        #endregion

        #region Save
        public async Task<IActionResult> Save(VisitorModel model)
        {

            if (ModelState.IsValid)
            {
                Console.WriteLine("org = " + model.OrganizationID);
                Console.WriteLine("dept = " + model.DepartmentID);
                Console.WriteLine("pur] = " + model.Purpose);
                Console.WriteLine("user = " + model.UserID);
                Console.WriteLine("host = " + model.HostID);
                Console.WriteLine("noperson] = " + model.NoOfPerson);

                Console.WriteLine("id = " + model.IDProof);
                Console.WriteLine("idno = " + model.IDProofNumber);
                var json = JsonConvert.SerializeObject(model);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response;
                
                response = await _client.PostAsync($"Visitor", content);

                if (response.IsSuccessStatusCode)
                    return RedirectToAction("GetVisitor");
            }
            await OrganizationDropDown();
            await UserDropDown();
            return RedirectToAction("VisitorForm");
        }
        #endregion

        #region ProfilePage
        public async Task<IActionResult> ProfilePage(int id)
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
