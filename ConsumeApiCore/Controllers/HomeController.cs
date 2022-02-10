using ConsumeApiCore.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using ConsumeApiCore.Models;
using Newtonsoft.Json;
using System.Text;

namespace ConsumeApiCore.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            List<Emp> empList = new List<Emp>();
            using (var httpClient = new HttpClient())
            {
                using ( var response = await httpClient.GetAsync("http://localhost:85/api/employee"))
                {
                    string apiresponse = await response.Content.ReadAsStringAsync();
                    empList = JsonConvert.DeserializeObject<List<Emp>>(apiresponse);
                }
            }
            return View(empList);
        }

        public IActionResult AddEmployee()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddEmployee(Emp employee)
        {
            Emp emp = new Emp();
            using (var httpClient = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(employee),Encoding.UTF8,"application/json");

                using (var response = await httpClient.PostAsync("http://localhost:85/api/employee",content))
                {
                    string apiresponse = await response.Content.ReadAsStringAsync();
                    emp  =JsonConvert.DeserializeObject<Emp>(apiresponse);
                }
            }
            return RedirectToAction("Index");
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> deleteemp(int empid)
        {
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.DeleteAsync("http://localhost:85/api/employee/" + empid))
                {
                    string apiresponse = await response.Content.ReadAsStringAsync();
                }

            }
            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}