using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using VisitorWebsite.Models;

namespace VisitorWebsite.Controllers
{
    public class HomeController : Controller
    {
        //private readonly ILogger<HomeController> _logger;

        //public HomeController(ILogger<HomeController> logger)
        //{
        //    _logger = logger;
        //}

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

        [CheckAccess]
        #region Index
        public IActionResult Index()
        {
            return View();
        }
        #endregion

        [CheckAccess]
        #region ProfilePage
        public async Task<IActionResult> Profile()
        {
            UserModel user = new UserModel();
            HttpResponseMessage response = await _client.GetAsync($"User/{CommonVariable.UserID()}");
            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                user = JsonConvert.DeserializeObject<UserModel>(data);
            }
            return View(user);
        }
        #endregion

        [CheckAccess]
        #region VisitList
        public IActionResult VisitList()
        {
            List<VisitorModel> Visits = new List<VisitorModel>();
            HttpResponseMessage response = _client.GetAsync($"Visitor").Result;
            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                Visits = JsonConvert.DeserializeObject<List<VisitorModel>>(data);
            }
            List<VisitorModel> newVisitors = Visits.Where(x => x.UserID == CommonVariable.UserID()).ToList();
            return View(newVisitors);
        }
        #endregion

        #region LoginPage
        public IActionResult Login()
        {
            return View();
        }
        #endregion

        #region SignUpPage
        public IActionResult SignUp()
        {
            /*if (UserID.HasValue)
            {
                var response = await _client.GetAsync($"User/{UserID}");
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    var user = JsonConvert.DeserializeObject<UserModel>(data);
                    await LoadOrganization();
                    if (user.OrganizationID != null)
                    {
                        ViewBag.DepartmentList = LoadDepartment(user.OrganizationID.Value);
                        GetDepartmentByOrganizationID(user.OrganizationID.Value);
                    }
                    return View(user);
                }
            }
            await LoadOrganization();*/
            return View(new UserModel
            {
                UserTypeID = 3
            });
        }
        #endregion

        #region UserLogin
        public async Task<IActionResult> UserLogin(UserLoginModel userLoginModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if(userLoginModel.UserName == "Admin123" && userLoginModel.Password == "Admin@123")
                    {
                        HttpContext.Session.SetString("UserID", "AdminID");
                        return RedirectToAction("Index", "Home", new { area = "Admin" });
                    }
                    var json = JsonConvert.SerializeObject(userLoginModel);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await _client.PostAsync($"User/login", content);

                    if (response.IsSuccessStatusCode)
                    {
                        string data = response.Content.ReadAsStringAsync().Result;
                        UserModel user = JsonConvert.DeserializeObject<UserModel>(data);
                        HttpContext.Session.SetString("UserID", user!.UserID.ToString());
                        HttpContext.Session.SetString("UserName", user.UserName.ToString());
                        HttpContext.Session.SetString("ImagePath", user.ImagePath.ToString());
                        HttpContext.Session.SetString("UserTypeID", user!.UserTypeID.ToString());
                        HttpContext.Session.SetString("OrganizationID", user.OrganizationID.ToString());
                        HttpContext.Session.SetString("DepartmentID", user.DepartmentID.ToString());

                        Console.WriteLine("Name === " + CommonVariable.UserName());
                        Console.WriteLine("ImagePath === " + CommonVariable.ImagePath());

                        if (Convert.ToInt32(user.UserTypeID) == 1)
                        {
                            return RedirectToAction("Index", "Home", new { area = "Host" });
                        }
                        else if(Convert.ToInt32(user.UserTypeID) == 2)
                        {
                            return RedirectToAction("Index", "Home", new { area = "Receptionist" });
                        }
                        else
                        {
                            return RedirectToAction("Index", "Home");
                        }
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Username or password may be wrong. "+response.ReasonPhrase+"!!!";
                        return RedirectToAction("Login", "Home");
                    }
                }
            }
            catch (Exception e)
            {
                TempData["ErrorMessage"] = e.Message;
            }

            return RedirectToAction("Login");
        }
        #endregion

        #region Save

        [HttpPost]
        public async Task<IActionResult> Save(UserModel model)
        {
            if (ModelState.IsValid)
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
                                return View("SignUp", model);
                            }
                        }
                    }
                }
                //✅ Step 2: Send User Data to "User" API(excluding ImageFile)
                model.UserTypeID = 3;
                var userJson = JsonConvert.SerializeObject(model);

                var contentUser = new StringContent(userJson, Encoding.UTF8, "application/json");

                HttpResponseMessage responseUser = await _client.PostAsync("User", contentUser);

                if (responseUser.IsSuccessStatusCode)
                {
                    return RedirectToAction("Login", "Home");
                }
                else
                {
                    Console.WriteLine(responseUser.Content.ToString());
                    TempData["ErrorMessage"] = "User registration failed.";
                    return View("SignUp", model);
                }
            }
            else
            {
                TempData["ErrorMessage"] = "Please enter the valid field values.";
                return View("SignUp", model);
            }
        }
        #endregion

        [CheckAccess]
        //One in whole project - 1 to all areas
        #region Logout
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Home");
        }
        #endregion

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
