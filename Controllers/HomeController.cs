using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ActCenter.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;

namespace ActCenter.Controllers
{
    public class HomeController : Controller
    {
        private MyContext dbContext;
        public HomeController(MyContext context)
        {
            dbContext = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost("register-user")]
        public IActionResult Register (User user) {
            if(ModelState.IsValid) {
                if(dbContext.Users.Any(a => a.Email == user.Email)) {
                    ModelState.AddModelError("Email", "Email already exists...");
                    return View("Index");
                } else {
                    PasswordHasher<User> Hasher = new PasswordHasher<User>();
                    user.Password = Hasher.HashPassword(user, user.Password);
                    dbContext.SaveChanges();
                    dbContext.Users.Add(user);
                    dbContext.SaveChanges();
                    HttpContext.Session.SetInt32("logged_user", user.UserId);
                    return RedirectToAction("Dashboard");
                }
                
            } else {
                return View("Index");
            }
        }
        [HttpGet("Dashboard")]
        public IActionResult Dashboard() {
            ViewModel Dashview = new ViewModel
            {
                AllEvents = dbContext.Events.Include(c => c.CreatedBy).Include(j => j.Joined).ThenInclude(u => u.User).OrderByDescending(e => e.D).ToList(),
                OneUser =dbContext.Users.FirstOrDefault(us => us.UserId == (int)HttpContext.Session.GetInt32("logged_user"))
            };
            if(HttpContext.Session.GetInt32("logged_user") != null) {
                return View(Dashview);
            } else {
                ModelState.AddModelError("Email", "You are not logged in!");
                return View("Index");
            }
            }
        [HttpGet("clear")]
        public IActionResult Clear() {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }

        [HttpPost("login-user")]
        public IActionResult LoginUser(LoginUser logged) {
            var userInDb = dbContext.Users.FirstOrDefault(u => u.Email == logged.Email);
            if(ModelState.IsValid) {
                if(userInDb == null) {
                    ModelState.AddModelError("Email", "Invalid email/password!");
                    return View("Index");
                }
                var hasher = new PasswordHasher<LoginUser>();
                var result = hasher.VerifyHashedPassword(logged, userInDb.Password, logged.Password);
                if(result == 0) {
                    ModelState.AddModelError("Password", "Invalid Email/Password");
                    return View("Index");
                }
                HttpContext.Session.SetInt32("logged_user", userInDb.UserId);
                return RedirectToAction("Dashboard", User);
            }
            else {
                return View("Index");
            }

        }
        [HttpGet("New")]
        public IActionResult New(){
            ViewModel Newview = new ViewModel
            {
                OneEvent = new Event()
            };
            return View();
        }
        [HttpPost("createactivity")]
        public IActionResult CreateActivity(ViewModel data){
            if(ModelState.IsValid){
            Event e = new Event
            
            {
                UserId = dbContext.Users.FirstOrDefault(us => us.UserId == (int)HttpContext.Session.GetInt32("logged_user")).UserId,
                EventName = data.OneEvent.EventName,
                Duration = data.OneEvent.Duration,
                CreatedBy = dbContext.Users.FirstOrDefault(us => us.UserId == (int)HttpContext.Session.GetInt32("logged_user")),
                D = data.OneEvent.D,
                Hr = data.OneEvent.Hr,
                Description = data.OneEvent.Description,
            };
           
                dbContext.Events.Add(e);
                dbContext.SaveChanges();
            return RedirectToAction("Dashboard");
            } else {
                ModelState.AddModelError("EventName","What's the big event?");
                // ModelState.AddModelError("Event","What's the big event?");
                // ModelState.AddModelError("Event","What's the big event?");
                // ModelState.AddModelError("Event","What's the big event?");
                // ModelState.AddModelError("Event","What's the big event?");
                // ModelState.AddModelError("Event","What's the big event?");
                // ModelState.AddModelError("Event","What's the big event?");
                return View("New", data);
            }
        }
        [HttpGetAttribute("delete/Id")]
        public IActionResult Delete(int Id)
        {
            Event GetGone = dbContext.Events.SingleOrDefault(e => e.EventId == Id);
            dbContext.Events.Remove(GetGone);
            dbContext.SaveChanges();
            return RedirectToAction("Dashboard");
        }
        [HttpGetAttribute("join/Id")]
        public IActionResult Join(int Id)
        {
            UserEvent jo = new UserEvent
            {
                UserId = dbContext.Users.FirstOrDefault(us => us.UserId == (int)HttpContext.Session.GetInt32("logged_user")).UserId,
                EventId = Id
            };
            ViewModel Dashview = new ViewModel
            {
                AllEvents = dbContext.Events.Include(c => c.CreatedBy).Include(j => j.Joined).ThenInclude(u => u.User).OrderByDescending(e => e.D).ToList(),
                OneUser =dbContext.Users.FirstOrDefault(us => us.UserId == (int)HttpContext.Session.GetInt32("logged_user"))
            };
                dbContext.Add(jo);
                dbContext.SaveChanges();
                return RedirectToAction("Dashboard");

        }
        [HttpGetAttribute("leave/Id")]
        public IActionResult Leave(int Id)
        {
            UserEvent go = dbContext.Joined.SingleOrDefault(e => e.EventId == Id);
            dbContext.Joined.Remove(go);
            dbContext.SaveChanges();
            return RedirectToAction("Dashboard");
        }
        [HttpGet("Act/{Id}")]
        public IActionResult Act(int Id) {
            ViewModel Actview = new ViewModel
            {
                AllEvents = dbContext.Events.Include(c => c.CreatedBy).Include(j => j.Joined).ThenInclude(u => u.User).OrderByDescending(e => e.D).ToList(),
                OneUser =dbContext.Users.FirstOrDefault(us => us.UserId == (int)HttpContext.Session.GetInt32("logged_user")),
                OneEvent = dbContext.Events.SingleOrDefault(k => k.EventId == Id)
            };
            if(HttpContext.Session.GetInt32("logged_user") != null) {
                return View("Act",Actview);
            } else {
                ModelState.AddModelError("Email", "You are not logged in!");
                return View("Index");
            }
            }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
