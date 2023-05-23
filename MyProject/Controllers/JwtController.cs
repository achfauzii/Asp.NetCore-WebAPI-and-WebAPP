using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MyProject.Context;
using MyProject.Models;
using MyProject.Repository;
using MyProject.Repository.Interface;
using MyProject.ViewModels;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;


namespace MyProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JwtController : ControllerBase
    {
        public IConfiguration _configuration;
        private readonly AccountRepository account; //Repository Account
        private readonly MyContext _Context; //Ambil dari DbContext sesuai nama yaitu Mycontext yang ada pada folder context


        public JwtController(IConfiguration config, MyContext myContext, AccountRepository accountRepository)
        {
            _configuration = config;
            _Context = myContext;
            account = accountRepository;
        }

         [HttpPost("Login")]
        public async Task<IActionResult> Post(LoginVM login)
        {
            if (login!= null && login.email != null && login.password != null)
            {
                var verified = account.Login(login);
                if (verified == 0) //0 pada validasi repo email salah
                {
                    return StatusCode(400, new { status = HttpStatusCode.BadRequest, message = "Login Failed, Email Salah" });
                }
                else if (verified == 1) //1 pada validasi repo password salah
                {
                    return StatusCode(400, new { status = HttpStatusCode.BadRequest, message = "Login Failed, Password Salah" });
                }


                var accountInfo = await GetUser(login.email);


                if (accountInfo != null)
                {
                    //create claims details based on the user information
                    var claims = new[] {
                    new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                        new Claim("Nik", accountInfo.NIK),
                        new Claim("First Name", accountInfo.FirstName),
                        new Claim("Last Name", accountInfo.LastName),
                        new Claim("Email", accountInfo.Email)
                    };

                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                    var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                    var token = new JwtSecurityToken(
                        _configuration["Jwt:Issuer"],
                        _configuration["Jwt:Audience"],
                        claims,
                        expires: DateTime.UtcNow.AddDays(1),
                        signingCredentials: signIn);
                    var Token = new JwtSecurityTokenHandler().WriteToken(token);
                    return StatusCode(200, new { status = HttpStatusCode.OK, token = Token });

                }
                else
                {
                    return BadRequest("Invalid credentials");
                }
            }
            else
            {
                return BadRequest();
            }
        }

        private async Task<Employee> GetUser(string email)
        {
            return await _Context.Employees.FirstOrDefaultAsync(u => u.Email == email);
        }
    }
}
    