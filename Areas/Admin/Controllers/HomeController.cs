using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Data;
using VisitorWebsite.Areas.Admin.Models;

namespace VisitorWebsite.Areas.Admin.Controllers
{
    [Area("Admin")]
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
        public async Task<IActionResult> Index()
        {
            DashBoardModel dashboardData = new DashBoardModel();

            try
            {
                HttpResponseMessage response = await _client.GetAsync("Dashboard"); // Call the API

                if (response.IsSuccessStatusCode)
                {
                    string data = await response.Content.ReadAsStringAsync();
                    var backendData = JsonConvert.DeserializeObject<DashBoardModel>(data);
                    dashboardData.Counts = backendData.Counts;
                    dashboardData.RecentOrganizations = backendData.RecentOrganizations;
                    dashboardData.RecentDepartments = backendData.RecentDepartments;
                    dashboardData.TopReceptionists = backendData.TopReceptionists;
                    dashboardData.TopVisitors = backendData.TopVisitors;
                    dashboardData.TopHosts = backendData.TopHosts;
                }
                else
                {
                    ViewBag.Error = $"API Error: {response.StatusCode}";
                    Console.WriteLine($"Error: {response.StatusCode}");
                    Console.WriteLine($"Response: {await response.Content.ReadAsStringAsync()}");
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error connecting to API.";
                Console.WriteLine($"Exception: {ex.Message}");
            }

            return View(dashboardData);
        }

        //public IActionResult Index()
        //{
        //    var dashboardData = new DashBoardModel
        //    {
        //        Counts = new List<DashboardCounts>(),
        //        RecentOrganizations = new List<RecentOrganizationModel>(),
        //        RecentDepartments = new List<RecentDepartmentModel>(),
        //        TopReceptionists = new List<TopReceptionistModel>(),
        //        TopVisitors = new List<TopVisitorModel>(),
        //        TopHosts = new List<TopHostModel>()
        //    };

        //    HttpResponseMessage response = _client.GetAsync($"{_client.BaseAddress}/Dashboard").Result;
        //    if (response.IsSuccessStatusCode)
        //    {
        //        string data = response.Content.ReadAsStringAsync().Result;
        //        dashboardData = JsonConvert.DeserializeObject<DashBoardModel>(data);
        //    }
        //    foreach (var v in dashboardData.RecentDepartments)
        //    {
        //        Console.WriteLine(v);
        //    }
        //    Console.WriteLine(dashboardData.Counts);
        //    return View();
        //}
    }
}
