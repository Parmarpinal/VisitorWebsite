using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using VisitorWebsite.Areas.Admin.Models;

namespace VisitorWebsite.Areas.Admin.Controllers
{
    [CheckAccess]
    [Area("Admin")]
    public class UserController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _client;

        public UserController(IConfiguration configuration)
        {
            _configuration = configuration;
            _client = new HttpClient
            {
                BaseAddress = new System.Uri(_configuration["WebApiBaseUrl"])
            };
        }

        #region HostList
        public async Task<IActionResult> GetHosts(int? departmentID)
        {
            List<UserModel> hosts = await GetUser(null,departmentID,1);  
            return View("HostList", hosts);
        }
        #endregion

        #region ReceptionistList
        public async Task<IActionResult> GetReceptionists(int? organizationID)
        {
            List<UserModel> receptionist = await GetUser(organizationID,null,2);
            return View("ReceptionistList", receptionist);
        }
        #endregion

        #region GetUser
        public async Task<List<UserModel>> GetUser(int? organizationID,int? departmentID, int typeID)
        {
            List<UserModel> users = new List<UserModel>();
            String queryString = $"search?orgID={organizationID}&deptID={departmentID}&typeID={typeID}";
            HttpResponseMessage response = await _client.GetAsync($"User/{queryString}");
            if (response.IsSuccessStatusCode)
            {
                string data = await response.Content.ReadAsStringAsync();
                users = JsonConvert.DeserializeObject<List<UserModel>>(data);
            }
            List<UserModel> hosts = new List<UserModel>();
            List<UserModel> receptionist = new List<UserModel>();
            foreach (var v in users)
            {
                if(v.UserTypeID == 1)
                {
                    hosts.Add(v);
                }else if(v.UserTypeID == 2)
                {
                    receptionist.Add(v);
                }
            }
            if (typeID == 1)
            {
                return hosts;
            }
            else
            {
                return receptionist;
            }
        }
        #endregion

        #region HostForm
        public async Task<IActionResult> HostForm(int? UserID)
        {
            await LoadOrganization();
            if (UserID.HasValue)
            {
                var response = await _client.GetAsync($"User/{UserID}");
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    var user = JsonConvert.DeserializeObject<UserModel>(data);
                    //await LoadOrganization();
                    //if(user.OrganizationID != null)
                    //{
                    //    ViewBag.DepartmentList = LoadDepartment(user.OrganizationID.Value);
                    //    GetDepartmentByOrganizationID(user.OrganizationID.Value);
                    //}

                    ViewBag.DepartmentList = LoadDepartment((int)user.OrganizationID);
                    await GetDepartmentByOrganizationID((int)user.OrganizationID);

                    return View(user);
                }
            }
            await LoadOrganization();
            return View(new UserModel());
        }
        #endregion

        #region ReceptionistForm
        public async Task<IActionResult> ReceptionistForm(int? userID)
        {
            if (userID.HasValue)
            {
                var response = await _client.GetAsync($"User/{userID}");
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    var User = JsonConvert.DeserializeObject<UserModel>(data);
                    await LoadOrganization();
                    return View(User);
                }
            }
            await LoadOrganization();
            return View(new UserModel());
        }
        #endregion

        #region Save

        [HttpPost]
        public async Task<IActionResult> Save(UserModel model)
        {
            if (ModelState.IsValid)
            {
                if(model.UserID == 0)
                {
                    IFormFile? file = model.ImageFile;
                    if (file != null && file.Length > 0)
                    {
                        using (var content = new MultipartFormDataContent())
                        {
                            using (var fileStream = file.OpenReadStream())
                            {
                                content.Add(new StreamContent(fileStream), "file", file.FileName);

                                HttpResponseMessage response = await _client.PostAsync("Image/upload", content);

                                if (response.IsSuccessStatusCode)
                                {
                                    string imageUrl = await response.Content.ReadAsStringAsync();
                                    Console.WriteLine(imageUrl);
                                    model.ImagePath = imageUrl;
                                }
                                else
                                {
                                    TempData["ErrorMessage"] = "Failed to upload file.";
                                    await LoadOrganization();
                                    if (model.UserTypeID == 1)
                                    {
                                        return View("HostForm", model);
                                    }
                                    else
                                    {
                                        return View("ReceptionistForm", model);
                                    }
                                }
                            }
                        }
                    }
                }
                
                //✅ Step 2: Send User Data to "User" API(excluding ImageFile)
                var userJson = JsonConvert.SerializeObject(model);

                var contentUser = new StringContent(userJson, Encoding.UTF8, "application/json");

                HttpResponseMessage responseUser;
                if (model.UserID == 0)
                {
                    responseUser = await _client.PostAsync("User", contentUser);
                }
                else
                {
                    if(model.ImagePath == null)
                    {
                        model.ImagePath = "";
                    }
                    responseUser = await _client.PutAsync($"User/{model.UserID}", contentUser);
                }


                if (responseUser.IsSuccessStatusCode)
                {
                    if (model.UserTypeID == 1)
                    {
                        return RedirectToAction("GetHosts");
                    }
                    else
                    {
                        return RedirectToAction("GetReceptionists");
                    }
                    //return RedirectToAction("Login", "Home");
                }
                else
                {
                    //Console.WriteLine(responseUser.Content.ToString());
                    TempData["ErrorMessage"] = "User registration failed.";
                    //return View("SignUp", model);
                    await LoadOrganization();
                    if (model.UserTypeID == 1)
                    {
                        return View("HostForm", model);
                    }
                    else
                    {
                        return View("ReceptionistForm", model);
                    }
                }
            }

            TempData["ErrorMessage"] = "User registration failed.";
            //return View("SignUp", model);
            await LoadOrganization();
            if (model.UserTypeID == 1)
            {
                return View("HostForm", model);
            }
            else
            {
                return View("ReceptionistForm", model);
            }
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
    }
}
