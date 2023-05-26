using Client.Models;
using Microsoft.AspNetCore.Mvc;
using MyProject.Models;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace Client.Controllers
{
    public class ChartsController : Controller
    {
        public static string baseUrl = "http://localhost:8042/api/Employees";
        public async Task<IActionResult> Index()
        {
            var totalEmployee = await GetTotalEmployee();
            var totalDepartment = await GetTotalDepartment();

            ViewBag.TotalData = totalEmployee;
            ViewBag.TotalDepartment = totalDepartment;
            return View();
        }


        public async Task<int> GetTotalEmployee()
        {

            //var accessToken = HttpContext.Session.GetString("tokenJWT");
            var url = baseUrl;
            HttpClient client = new HttpClient();
            //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            string jsonResponse = await client.GetStringAsync(url);

            dynamic data = JsonConvert.DeserializeObject(jsonResponse);
            int totalData = data.totalData;
            return totalData;
        }


        public async Task<int> GetTotalDepartment()
        {

            var accessToken = HttpContext.Session.GetString("tokenJWT");
            var url = "http://localhost:8042/api/Departments";

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            string jsonResponse = await client.GetStringAsync(url);

            dynamic data = JsonConvert.DeserializeObject(jsonResponse);
            int totalDepartment = data.totalData;
            return totalDepartment;
        }
    }
}
