using Microsoft.AspNetCore.Mvc;
using MyProject.Models;
using MyProject.Repository;
using MyProject.ViewModels;
using System.Net;

namespace MyProject.Controllers
{
    
    [ApiController]
    [Route("api/[controller]")]
    public class AccountsController : ControllerBase
    {
        private readonly AccountRepository accountRepository;
        private readonly EmployeeRepository employeeRepository;

        public AccountsController(AccountRepository accountRepository, EmployeeRepository employeeRepository)
        {
            this.accountRepository = accountRepository;
            this.employeeRepository = employeeRepository;
        }

        [HttpGet]
        public ActionResult Get()
        {
            var get = accountRepository.Get();
            if (get == null)
            //Bisa pakai Get.count
            {
                return NotFound("Data Notfound");
            }
            return StatusCode(200, new { status = HttpStatusCode.OK, message = "Request succeeded", TotalData = get.Count(), Data = get });
        }



        [HttpPost("Register")]
        public ActionResult Insert(Account account)
        {
            if (accountRepository.IsIdExists(account.NIK))
            {

                return StatusCode(409, new { status = HttpStatusCode.Conflict, message = "Nik has been registered", Data = account.NIK, account.Employee.Email });
            }

            else if (employeeRepository.IsIdExists(account.NIK)) //Jika NIK sudah ada pada Data Pegawai
            {

                accountRepository.Insert(account);
                return StatusCode(201, new { status = HttpStatusCode.OK, message = "Data Berhasil Ditambahkan", Data = account });

            }
            return StatusCode(409, new { status = HttpStatusCode.Conflict, message = "NIK Tidak Sesuai", Data = account.NIK });
        }

        [HttpPost("Login")]
        //public ActionResult Login(LoginVM loginVM)
        public ActionResult Login(LoginVM loginVM)
        {
            //VERIFY BCrypt PADA CONTROLLER
            /*var verified = accountRepository.Login(loginVM.email); 
            if (verified == null)
            {
                return StatusCode(400, new { status = HttpStatusCode.BadRequest, message = "Login Failed, Email Salah" });
            }
            else if (!BCrypt.Net.BCrypt.Verify(loginVM.password, verified.Password))
            {
                return StatusCode(400, new { status = HttpStatusCode.BadRequest, message = "Login Failed, Password Salah" });
            }

            return StatusCode(200, new { status = HttpStatusCode.OK, message = "Anda berhasil Login" });*/

            //VERIFY  BCrypt PADA REPOSITORY yang mengembalikan int
            var verified = accountRepository.Login(loginVM);
            if (verified == 0) //0 pada validasi repo email salah
            {
                return StatusCode(400, new { status = HttpStatusCode.BadRequest, message = "Login Failed, Email Salah" });
            }
            else if (verified == 1) //1 pada validasi repo password salah
            {
                return StatusCode(400, new { status = HttpStatusCode.BadRequest, message = "Login Failed, Password Salah" });
            }

            return StatusCode(200, new { status = HttpStatusCode.OK, message = "Anda berhasil Login" });


            //VERIFY PADA REPOSITORY HANYA MENGEMBALIKAN True False bool
            /*var verified = accountRepository.LoginBool(loginVM);
            if (verified == false)
            {
                return StatusCode(400, new { status = HttpStatusCode.BadRequest, message = "Login Failed, Email atau Password Salah" });
            }

            return StatusCode(200, new { status = HttpStatusCode.OK, message = "Anda berhasil Login" });*/


        }
    }
}
