using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyProject.Models;
using MyProject.Repository;
using MyProject.Repository.Interface;
using System.Net;

namespace MyProject.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentsController : ControllerBase
    {
        private readonly DepartmentRepository departmentRepository;
        private readonly EmployeeRepository employeeRepository;

        public DepartmentsController(DepartmentRepository departmentRepository, EmployeeRepository employeeRepository)
        {
            this.departmentRepository = departmentRepository;
            this.employeeRepository = employeeRepository;
        }
      
        [HttpGet]
        public ActionResult Get_Department() //GET ALl DEPARTMENT
        {
            var get = departmentRepository.Get_Department();
            if (get == null)
            //Bisa pakai Get.count
            {
                return NotFound("Data Notfound");
            }
            return StatusCode(200, new { status = HttpStatusCode.OK, message = "Request succeeded", TotalData= get.Count(), Data = get});
        }

        [HttpPost]
        public ActionResult Insert(Department department) //INSERT TO TABLE DEPARTMENT
        {

            if (departmentRepository.IsDepIdExists(department.Id))
            {
                return StatusCode(409, new { status = HttpStatusCode.Conflict, message = "Data Already Exists in Database", Data = department });
            }else if (string.IsNullOrEmpty(department.Name))
            {
                return StatusCode(400, new { status = HttpStatusCode.BadRequest, message = "Data harus di isi", Data = department});
            }

            departmentRepository.Insert(department);
            return StatusCode(200, new { status = HttpStatusCode.OK, message = "Data Berhasil Di Tambahkan", Data = department });
        }

        //GET Department Based On Id
        [HttpGet("{Id}")]
        public ActionResult Get(int Id)
        {
            var get = departmentRepository.Get_DepartmentId(Id);
          
       
            return StatusCode(200, new { status = HttpStatusCode.OK, message = "Data ditemukan", Data = get });
        }

        [HttpPut]
        public ActionResult Update(Department department)
        {
            if (departmentRepository.IsDepIdExists(department.Id))
            {
                var update = departmentRepository.Update(department);
                return StatusCode(200, new { status = HttpStatusCode.OK, message = "Data Berhasil di Update", Data = update });
            }
            return NotFound();
        }

        [HttpDelete("Id")]
        public ActionResult Delete(int id)
        {
            if (departmentRepository.IsDepIdExists(id))
            {

                try
                {
                    int result = departmentRepository.Delete(id); 

                    if (result > 0)
                    {
                        return StatusCode(200, new { status = HttpStatusCode.OK, message = "Data behasil di hapus" });
                    }
                    else
                    {
                        return NotFound("Data not found");
                    }
                }
                catch (DbUpdateException ex)
                {
                    return StatusCode(409, new { status = HttpStatusCode.Conflict, message = "The DELETE statement conflicted with the REFERENCE constraint." });
                }

            }
            else {
                return BadRequest("Not a valid Id");
            }
           
        }
    }
}
