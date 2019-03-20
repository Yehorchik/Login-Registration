using System;
using System.Collections.Generic;
using System.Linq;
using LoginRegistration.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LoginRegistration.Controllers
{
    public class UserController:Controller
    {
        private UsersContext dbContext;
        public UserController(UsersContext context)
        {
            dbContext = context;
        }

        [HttpGet("")]
        public  IActionResult Index()
        {
          return View("index");
        }

        [HttpGet("login")]
        public IActionResult Login()
        {
            return View("login");
        }

        [HttpGet("success/{userid}")]
        public  IActionResult Success(int userid)
        {
          Users oneUser = dbContext.users.FirstOrDefault(u => u.UserId == userid);
          List<Transactions> AllTransactions = dbContext.transactions.Where(u => u.UserId == userid).ToList();
          decimal Balance = 0;
          foreach(var amount in AllTransactions)
          {
            Balance += amount.Amount;
          }
          ViewBag.Transactions = AllTransactions;
          ViewBag.User = oneUser;
          HttpContext.Session.SetInt32("id" , oneUser.UserId);
          HttpContext.Session.SetInt32("balance" , (int)Balance);
          ViewBag.Balance = String.Format("{0:0.00}", HttpContext.Session.GetInt32("balance")); 
          return View("success");
        }

        [HttpPost("register")]
        public IActionResult SignUp(Users user)
        {
            if(ModelState.IsValid)
            {
                if(dbContext.users.Any(u => u.Email == user.Email))
                {
                    ModelState.AddModelError("Email", "Email already in use!");
                    return View("index");
                }
                PasswordHasher<Users> Hasher = new PasswordHasher<Users>();
                user.Password = Hasher.HashPassword(user, user.Password);
                dbContext.Add(user);
                dbContext.SaveChanges();
                string email = user.Email;
                Users oneUser = dbContext.users.FirstOrDefault(u => u.Email == email);
                return Redirect($"success/{oneUser.UserId}");
            }
            else
            {
                return View("index");
            }

        }

        [HttpPost("confirm")]
        public IActionResult Confirm(LoginUser userSubmission)
        {
            if(ModelState.IsValid)
            {
                // If inital ModelState is valid, query for a user with provided email
                var userInDb = dbContext.users.FirstOrDefault(u => u.Email == userSubmission.Email);
                // If no user exists with provided email
                if(userInDb == null)
                {
                    // Add an error to ModelState and return to View!
                    ModelState.AddModelError("Email", "Invalid Email");
                    return View("login");
                }
                
                // Initialize hasher object
                var hasher = new PasswordHasher<LoginUser>();
                // varify provided password against hash stored in db
                var result = hasher.VerifyHashedPassword(userSubmission, userInDb.Password, userSubmission.Password);
                // result can be compared to 0 for failure
                if(result == 0)
                {
                    ModelState.AddModelError("Password" , "Invalid Password");
                    return View("login");
                }
                return Redirect($"success/{userInDb.UserId}" );
            }
            else
            {
                return View("login");
            }
        }
        [HttpPost("makeaction")]
        public IActionResult Make (Transactions transaction)
        {
            int? balance = HttpContext.Session.GetInt32("balance");
            Users oneUser = dbContext.users.FirstOrDefault(u=> u.UserId == transaction.UserId);
            List<Transactions> AllTransactions = dbContext.transactions.Where(u => u.UserId == oneUser.UserId).ToList();
            ViewBag.User = oneUser;
            ViewBag.Transactions = String.Format("{0:0.00}",AllTransactions); 
            ViewBag.Balance = String.Format("{0:0.00}", HttpContext.Session.GetInt32("balance")); 
            if(transaction.Amount <= 0 && balance <= 0 )
            {
                ModelState.AddModelError("Amount" , "Balance is 0 , you cann ot make the transaction");
                return View("success");
            }
            if(transaction.Amount == 0)
            {
                return View("success");
            }
            dbContext.Add(transaction);
            dbContext.SaveChanges();
            return RedirectToAction("Success" , new { userid = oneUser.UserId});

        }


    }
}