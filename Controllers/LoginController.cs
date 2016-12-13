using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using wall.Models;
using wall.Factory;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;


namespace wall.Controllers
{
    public class LoginController : Controller
    {
         private readonly UserFactory userFactory;
         public LoginController(UserFactory user)
        {
            userFactory = user;
        }
        // GET: /Home/
        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
            ViewBag.Errors = "";
            return View();
        }
        [HttpPost]
        [Route("register")]
        public IActionResult Register(User newuser)
        {
            
            if(ModelState.IsValid)
            {
            var check = userFactory.FindByEmail(newuser.email);
            if(check != null)
            {
            
                ViewBag.ManualError = "This email is already registered";
                ViewBag.Errors = ModelState.Values;
                return View("Index");
            }
            PasswordHasher<User> Hasher = new PasswordHasher<User>();
            newuser.password = Hasher.HashPassword(newuser, newuser.password);
            userFactory.Add(newuser);
            check = userFactory.FindByEmail(newuser.email);
            HttpContext.Session.SetInt32("id", (int)check.Id);
            HttpContext.Session.SetString("name", (string)check.first_name);
            ViewBag.first_name = check.first_name;
            ViewBag.last_name = check.last_name;
            ViewBag.Errors = ModelState.Values;
            return RedirectToAction("Index", "Message");
            }
            
            
            ViewBag.Errors = ModelState.Values;
            return View("Index");
            
        }
        [HttpPost]
        [Route("login")]
        public IActionResult Login(string login_email, string login_password)
        {
            var user = userFactory.FindByEmail(login_email);
            if(user != null)
            {
                var Hasher = new PasswordHasher<User>();
                if(0 != Hasher.VerifyHashedPassword(user, user.password, login_password))
                {
                    HttpContext.Session.SetInt32("id", (int)user.Id);
                    HttpContext.Session.SetString("name", (string)user.first_name);
                    ViewBag.first_name = user.first_name;
                    ViewBag.last_name = user.last_name;
                    ViewBag.Errors = ModelState.Values;
                    return RedirectToAction("Index", "Message");
                }
            }
            ViewBag.ManualError = "Email or Password not found";
            ViewBag.Errors = ModelState.Values;
            return View("Index");
        }
        [HttpPost]
        [Route("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            ViewBag.Errors = ModelState.Values;
            return View("index");  
        }
    }
    
}
    


