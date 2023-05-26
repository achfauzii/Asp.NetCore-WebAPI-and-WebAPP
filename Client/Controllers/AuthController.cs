using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace Client.Controllers
{
    public class AuthController : Controller
    {
        public IConfiguration _configuration;


        public AuthController(IConfiguration _configuration)
        {
            this._configuration = _configuration;
          
        }

        public IActionResult Index()
        {
           /* if (HttpContext.Session.GetString("tokenJWT") == null)
            {
                return View("Login");
            }
            else
            {
                return RedirectToAction("Index", "Departments");
            }*/

            return View("Login");
        }
        // [HttpPost]
        /*  public IActionResult Login(string email)
         {
             if (HttpContext.Session.GetString("email") == null)
             {
                 if (ModelState.IsValid)
                 {
                     HttpContext.Session.SetString("email", email);
                     //var tokenString = GenerateJSONWebToken(email);

                     return RedirectToAction("Index", "Departments");
                 }
             }

             return View();
         }*/


        public IActionResult Login(string token)
        {

            /*if (HttpContext.Session.GetString("tokenJWT") == null)
            {
                if (ModelState.IsValid)
                {
                    HttpContext.Session.SetString("tokenJWT", token);
                    return RedirectToAction("Index", "Departments");
                }
            }

            return View();*/
            HttpContext.Session.SetString("tokenJWT", token);
            return RedirectToAction("Index", "Charts");
        }


        public IActionResult Logout()
        {
            HttpContext.Session.Remove("tokenJWT");
            HttpContext.Session.Clear();
            return View("Login");

        }

    }

}
