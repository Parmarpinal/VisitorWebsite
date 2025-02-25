using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using VisitorWebsite.Areas.Receptionist.Models;

namespace VisitorWebsite.Areas.Receptionist.Controllers
{
    [Area("Receptionist")]
    [CheckAccess]
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
            var v = Visitors.Where(x => x.CheckIn != null && x.CheckOut == null && x.OrganizationID == CommonVariable.OrganizationID());
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
            var v = Visitors.Where(x => x.OrganizationID == CommonVariable.OrganizationID());
            return View("VisitorList", v);
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

        //#region OrganizationDropDown
        //public async Task OrganizationDropDown()
        //{
        //    List<OrganizationDropDownModel> organizations = new List<OrganizationDropDownModel>();
        //    HttpResponseMessage response = await _client.GetAsync($"DropDown/organizations");
        //    if (response.IsSuccessStatusCode)
        //    {
        //        string data = await response.Content.ReadAsStringAsync();
        //        organizations = JsonConvert.DeserializeObject<List<OrganizationDropDownModel>>(data);
        //        ViewBag.OrganizationList = organizations;
        //    }
        //}
        //#endregion

        #region LoadDepartment
        public async Task LoadDepartment()
        {
            List<DepartmentDropDownModel> departments = new List<DepartmentDropDownModel>();
            HttpResponseMessage response = await _client.GetAsync($"DropDown/departments/{CommonVariable.OrganizationID()}");
            if (response.IsSuccessStatusCode)
            {
                string data = await response.Content.ReadAsStringAsync();
                departments = JsonConvert.DeserializeObject<List<DepartmentDropDownModel>>(data);
                ViewBag.DepartmentList = departments;
            }
        }
        #endregion

        //#region GetDepartmentByOrganizationID
        //[HttpPost]
        //public async Task<JsonResult> GetDepartmentByOrganizationID(int id)
        //{
        //    List<DepartmentDropDownModel> departments = await LoadDepartment(id);
        //    return Json(departments);
        //}
        //#endregion

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
            await LoadDepartment();
            await UserDropDown();
            return View(new VisitorModel() {
            });
        }
        #endregion

        #region Save
        public async Task<IActionResult> Save(VisitorModel model)
        {
            model.OrganizationID = Convert.ToInt32(CommonVariable.OrganizationID());

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
                    await UploadNotification(model.HostID, $"Visitor \"{model.VisitorName}\" has checked in at {DateTime.Now.ToString("hh:mm tt")} on {DateTime.Now.ToString("dd-MM-yyyy")}."

, model.VisitorID);
                    return RedirectToAction("GetVisitor");
            }
            await LoadDepartment();
            await UserDropDown();
            return RedirectToAction("VisitorForm");
        }
        #endregion

        #region ProfilePage
        public async Task<IActionResult> ProfilePage()
        {
            ReceptionistModel user = new ReceptionistModel();
            int? id = CommonVariable.UserID();
            HttpResponseMessage response = await _client.GetAsync($"User/{id}");
            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                user = JsonConvert.DeserializeObject<ReceptionistModel>(data);
            }
            return View(user);
        }
        #endregion

        #region LoadUser
        public async Task<JsonResult> LoadUser(int id)
        {
            UserModel user = new UserModel();
            HttpResponseMessage response = await _client.GetAsync($"User/{id}");
            if (response.IsSuccessStatusCode)
            {
                string data = await response.Content.ReadAsStringAsync();
                user = JsonConvert.DeserializeObject<UserModel>(data);
            }
            return Json(user);
        }
        #endregion

        #region UploadNotification
        public async Task UploadNotification(int hostID, string msg, int visitorID)
        {

            NotificationModel model = new NotificationModel();
            model.HostID = hostID;
            model.Message = msg;
            model.VisitorID = visitorID;

            var json = JsonConvert.SerializeObject(model);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage response;

            response = await _client.PostAsync($"Notification", content);
            
        }
        #endregion
    }
}
