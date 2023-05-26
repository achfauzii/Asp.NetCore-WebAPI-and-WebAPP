
using Client.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.DotNet.MSIdentity.Shared;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace Client.Controllers
{
 
    public class DepartmentsController : Controller
    {
        /*  public DepartmentsController() {
              

          }*/
        //public static string baseUrl = "http://localhost:8042/api/Departments";

        public IActionResult Index()
        {
            

            return View();
        }

        /*  public async Task<IActionResult> Index()
          {

            //var departments = await GetDepartments();
              return View("Departments");
          }*/

        /*  [HttpGet("Departments")]
          public async Task<IActionResult> GetDepartments()
          {

              var accessToken = HttpContext.Session.GetString("tokenJWT");
              var url = baseUrl;
              HttpClient client = new HttpClient();
              client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
              string jsonStr = await client.GetStringAsync(url);
              var response = JsonConvert.DeserializeObject<DepartmentsResponse>(jsonStr);
              var departments = response.data;

              return Json(departments);
          }*/


    }

}

