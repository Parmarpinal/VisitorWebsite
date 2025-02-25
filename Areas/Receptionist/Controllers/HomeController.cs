using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using VisitorWebsite.Areas.Receptionist.Models;

namespace VisitorWebsite.Areas.Receptionist.Controllers
{
    [Area("Receptionist")]
    [CheckAccess]
    public class HomeController : Controller
    {
        //private readonly IConfiguration _configuration;
        //private readonly HttpClient _client;

        //public HomeController(IConfiguration configuration)
        //{
        //    _configuration = configuration;
        //    _client = new HttpClient
        //    {
        //        BaseAddress = new System.Uri(_configuration["WebApiBaseUrl"])
        //    };
        //}

        #region Index
        public IActionResult Index(int id)
        {
            return View();
        }
        #endregion
    }
}
