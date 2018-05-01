using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using BankAccounts.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace BankAccounts.Controllers
{
    public class AccountController : Controller
    {

        private AccountContext _context;
 
        public AccountController(AccountContext context)
        {
            _context = context;
        }



        [HttpGet]
        [Route("dashboard")]
        public IActionResult Index() {
            if(HttpContext.Session.GetInt32("id") != null)
            {
                User myUser = _context.Users.Include(user => user.Accounts)
                                .SingleOrDefault(User => User.Id == HttpContext.Session.GetInt32("id"));

                ViewBag.User = myUser;
                return View();
            } else {
                return RedirectToAction("Index", "Home");
            }

            
        }

        [HttpGet]
        [Route("users/{id}/accounts/create")]
        public IActionResult CreateAccount(int id) {
            Account newAccount = new Account
            {
                AccountBalance = 0,
                UserId = id

            };

            _context.Accounts.Add(newAccount);
            _context.SaveChanges();
            return RedirectToAction("Index", "Account");
        }

        [HttpGet]
        [Route("users/{id}/accounts/{aid}")]
        public IActionResult ViewAccount(int id, int aid) {
            if(HttpContext.Session.GetInt32("id") != null)
            {
                User myUser = _context.Users.SingleOrDefault(User => User.Id == HttpContext.Session.GetInt32("id"));
                Account myAccount = _context.Accounts
                                    .Include(Account => Account.Records)
                                    .SingleOrDefault(Account => Account.Id == aid);
                ViewBag.User = myUser;
                ViewBag.Account = myAccount;
                if(TempData["error"] != null)
                {
                    ViewBag.Error = TempData["error"];
                }
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        [Route("users/{id}/accounts/{aid}/activity")]
        public IActionResult AccountActivity(int id, int aid, AccountActivityViewModel activity)
        {
            Account myAccount = _context.Accounts.SingleOrDefault(Account => Account.Id == aid);

            if(ModelState.IsValid)
            {
                Record newRecord = new Record
                {
                    AccountId = aid,
                    Amount = activity.Amount
                };

                if(newRecord.Amount + myAccount.AccountBalance > 0 )
                {
                    myAccount.AccountBalance = myAccount.AccountBalance + newRecord.Amount;
                    _context.Records.Add(newRecord);
                    _context.SaveChanges();   
                }
                else {
                    TempData["error"] = "You cannot withdraw that amount.";
                }

                
                return RedirectToAction("ViewAccount", new { id = id, aid = aid});
            }

            return RedirectToAction("Index");
    
        }
        
    }
}
