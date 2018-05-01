using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using BankAccounts.Models;
using Microsoft.AspNetCore.Identity;
using System.Linq;

namespace BankAccounts.Controllers
{
    public class HomeController : Controller
    {

        private AccountContext _context;
 
        public HomeController(AccountContext context)
        {
            _context = context;
        }


        // GET: /Home/
        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [Route("login")]
        public IActionResult Login(LoginViewModel user)
        {
            PasswordHasher<User> hasher = new PasswordHasher<User>();
            User myUser = _context.Users.SingleOrDefault(User => User.Email == user.Email);

            if(myUser != null) {
                if(hasher.VerifyHashedPassword(myUser, myUser.Password, user.Password) == 0)
                {
                    HttpContext.Session.SetInt32("id", myUser.Id);
                }
            }

            return RedirectToAction("Index", "Account");
        }

        [HttpGet]
        [Route("register")]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [Route("register_user")]
        public IActionResult RegisterUser(User user)
        {
            if(ModelState.IsValid)
            {
                PasswordHasher<User> hasher = new PasswordHasher<User>();
                string hashedpw = hasher.HashPassword(user, user.Password);
                User newUser = new User
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    Password = hashedpw
                };

                _context.Add(newUser);
                _context.SaveChanges();

                User myUser = _context.Users.SingleOrDefault(User => User.Email == user.Email);

                HttpContext.Session.SetInt32("id", myUser.Id);
                return RedirectToAction("Index", "Account");
            }
            else
            {
                Console.WriteLine("******* VALIDATION FAILED*******");
                return RedirectToAction("Index", "Home");
            }
        }
    }
}
