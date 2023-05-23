using Microsoft.AspNetCore.Mvc;
using MyProject.Models;
using MyProject.Repository;
using MyProject.Repository.Interface;
using MyProject.ViewModels;
using System.Net;

namespace MyProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase

    {
        private readonly EmployeeRepository employeeRepository;
        private readonly DepartmentRepository departmentRepository; //Repository Department

        public EmployeesController(EmployeeRepository employeeRepository, DepartmentRepository departmentRepository)
        {
            this.employeeRepository = employeeRepository;
            this.departmentRepository = departmentRepository;

        }
     

        [HttpPost]
        public ActionResult Insert(Employee employee)
        {

            if (string.IsNullOrWhiteSpace(employee.Email) || string.IsNullOrWhiteSpace(employee.NIK))
            {
                return StatusCode(400, new { status = HttpStatusCode.BadRequest, message = "Data Kosong atau Mengandung Spasi", Data = employee });
            }
            /*else if (employeeRepository.IsIdExists(employee.NIK)
                || employeeRepository.IsEmaillExists(employee.Email)
                || employeeRepository.IsPhonelExists(employee.Phone))
            {
                return StatusCode(409, new { status = HttpStatusCode.Conflict, message = "Data Already Exists in Database", Data = employee });
            }*/

            employeeRepository.Insert(employee);
            return StatusCode(200, new { status = HttpStatusCode.OK, message = "Data Berhasil Di Tambahkan", Data = employee });
        }

        [HttpPost("Register")]
        public ActionResult Reister(RegisterVM registerVM)
        {
            
            if (string.IsNullOrWhiteSpace(registerVM.Email))
            {
                return StatusCode(400, new { status = HttpStatusCode.BadRequest, message = "Data Kosong atau Mengandung Spasi", Data = registerVM });
            }else if (registerVM.Gender > Gender.Female)
            {
                return StatusCode(400, new { status = HttpStatusCode.BadRequest, message = "Gender hanya menerima 0 Male dan 1 Female", Data = registerVM });
            }
            else if  (employeeRepository.IsEmaillExists(registerVM.Email) || employeeRepository.IsPhonelExists(registerVM.Phone))
            {
                return StatusCode(409, new { status = HttpStatusCode.Conflict, message = "Data Already Exists in Database", Data = registerVM });
            }
            else if (departmentRepository.IsDepIdExists(registerVM.DepartmentId) == false)
            {
                return StatusCode(400, new { status = HttpStatusCode.BadRequest, message = "Error Input DeparmentId", Data = registerVM });
            }else if (registerVM.Password.Length <= 6)
            {
                return StatusCode(400, new { status = HttpStatusCode.BadRequest, message = "Password Harus Lebih dari 6 karakter", Data = registerVM });
            }
            employeeRepository.Register(registerVM);
            return StatusCode(200, new { status = HttpStatusCode.OK, message = "Data Berhasil Di Tambahkan", Data = registerVM });
        }
        //GET All Employee
        [HttpGet]
        public ActionResult Get()
        {
            var get = employeeRepository.Get();
            if (get == null)
            //Bisa pakai Get.count
            {
                return StatusCode(404, new { status = HttpStatusCode.NotFound, message = "Data Not Found", Data = get });
            }
            return StatusCode(200, new { status = HttpStatusCode.OK, message = "Request succeeded", TotalData = get.Count(), Data = get });
        }

        //GET Employee Based On NIK
        [HttpGet("NIK")]
        public ActionResult Get(string NIK)
        {
            var get = employeeRepository.Get(NIK);
            if (get == null)
            {

                return StatusCode(404, new { status = HttpStatusCode.NotFound, message = "Data not found", Data = get });
            }

            return StatusCode(200, new { status = HttpStatusCode.OK, message = "Data ditemukan", Data = get });
        }

        [HttpDelete("NIK")]
        public ActionResult Delete(string NIK)
        {
            if (employeeRepository.IsIdExists(NIK))
            {

                employeeRepository.Delete(NIK);

                return Ok();

            }
            return BadRequest("Not a valid NIK");

        }
        [HttpPut]
        public ActionResult Update(Employee employee)
        {
            if (employeeRepository.IsIdExists(employee.NIK))
            {
                var update = employeeRepository.Update(employee);
                return StatusCode(200, new { status = HttpStatusCode.OK, message = "Request succeeded", Data = update });
            }
            return NotFound();
        }


        [HttpGet("FullNameWithDepartment")]
        public ActionResult GetEmployeesWithDepartments()
        {
            var get = employeeRepository.GetEmployeesWithDepartments();
       

            if (get == null)
            //Bisa pakai Get.count
            {
                return StatusCode(404, new { status = HttpStatusCode.NotFound, message = "Data Not Found", Data = get });
            }
            return StatusCode(200, new { status = HttpStatusCode.OK, message = "Request succeeded", TotalData = get.Count(), Data = get });
        }



        //IMPLEMENTASI CORS (TEST)
        [HttpGet("TestCORS")]
        public ActionResult TestCORS()
        {
            return Ok("TEST CORS Berhasil 2212");
        }

    }
}
