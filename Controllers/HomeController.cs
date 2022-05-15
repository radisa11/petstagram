using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using petstagram.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace petstagram.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        public MyContext _context;

        public HomeController(ILogger<HomeController> logger,MyContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpPost("register")]
        public IActionResult Register(User newUser)
        {
            if(ModelState.IsValid)
            {
                if(_context.Users.Any( u => u.Email == newUser.Email))
                {
                    ModelState.AddModelError("Email", "Email is already in use");
                    return View("Index");
                }
                PasswordHasher<User> Hasher = new PasswordHasher<User>();
                newUser.Password = Hasher.HashPassword(newUser, newUser.Password);
                _context.Add(newUser);
                _context.SaveChanges();
                HttpContext.Session.SetInt32("loggedInUser", newUser.UserId);
                return RedirectToAction("Dashboard");
            }else{
                return View("Index");
            }
            
        }

        [HttpPost("login")]
        public IActionResult Login(LogUser logUser)
        {
            if(ModelState.IsValid)
            {
                User userindb = _context.Users.FirstOrDefault( d => d.Email == logUser.LogEmail);
                if (userindb == null)
                {
                    ModelState.AddModelError("LogEmail", "Invalid login attempt");
                    return View("Index");
                }
                PasswordHasher<LogUser> hasher = new PasswordHasher<LogUser>();
                var result = hasher.VerifyHashedPassword(logUser, userindb.Password, logUser.LogPassword);
                if(result ==0)
                {
                    ModelState.AddModelError("LogEmail", "Invalid login attempt");
                    return View("Index");
                }
                HttpContext.Session.SetInt32("loggedInUser", userindb.UserId);
                return RedirectToAction("Dashboard");
            }else{
            return View("Index");
            }
        }

        [HttpGet("Dashboard")]
        public IActionResult Dashboard()
        {
            if(HttpContext.Session.GetInt32("loggedInUser") == null)
            {
                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpGet("addPost")]
        public IActionResult AddPost()
        {
            if(HttpContext.Session.GetInt32("loggedInUser") == null)
            {
                return RedirectToAction("Index");
            }
            return View();
        }
        [HttpGet("post/{postId}")]
        public IActionResult onePost(int postId)
        {
            if(HttpContext.Session.GetInt32("loggedInUser") == null)
            {
                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpGet("logout")]
        public IActionResult Logout(int postId)
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }

        [HttpGet("edit/{postId}")]
        public IActionResult Edit(int postId)
        {
            if(HttpContext.Session.GetInt32("loggedInUser") == null)
            {
                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpPost("update")]
        public IActionResult Update()
        {
            if(HttpContext.Session.GetInt32("loggedInUser") == null)
            {
                return RedirectToAction("Index");
            }
            return RedirectToAction("Dashboard");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
