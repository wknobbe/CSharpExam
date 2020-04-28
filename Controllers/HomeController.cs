using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using CSharpExam.Models;

namespace CSharpExam.Controllers
{
    public class HomeController : Controller
    {
        private MyContext _context;
        public HomeController(MyContext context)
        {
            _context = context;
        }
        [HttpGet("")]
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost("register")]
        public IActionResult Register(User newUser)
        {
            if(ModelState.IsValid)
            {
                if(_context.Users.Any(u => u.Email == newUser.Email))
                {
                    ModelState.AddModelError("Email", "This email is already registered.");
                    return View("Index");
                }
                else
                {
                    PasswordHasher<User> hasher = new PasswordHasher<User>();
                    newUser.Password = hasher.HashPassword(newUser, newUser.Password);
                    _context.Users.Add(newUser);
                    _context.SaveChanges();
                    return Redirect("/");
                }
            }
            else
            {
                return View("Index");
            }
        }
        [HttpPost("login")]
        public IActionResult Login(LoginUser userInfo)
        {
            if(ModelState.IsValid)
            {
                var userInDb = _context.Users.FirstOrDefault(u => u.Email == userInfo.Email);
                if(userInDb == null)
                {
                    ModelState.AddModelError("Email", "This email has not been registered.");
                    return View("Index");
                }
                else
                {
                    var hasher = new PasswordHasher<LoginUser>();
                    var result = hasher.VerifyHashedPassword(userInfo, userInDb.Password, userInfo.Password);
                    if(result == 0)
                    {
                        ModelState.AddModelError("Password", "Password is incorrect.");
                        return View("Index");
                    }
                    else
                    {
                        User userLoggedIn = _context.Users.FirstOrDefault(u => u.Email == userInfo.Email);
                        HttpContext.Session.SetInt32("UserId", userLoggedIn.UserId);
                        return RedirectToAction("Dashboard");
                    }
                }
            }
            else
            {
                return View("Index");
            }
        }
        [HttpGet("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return Redirect("/");
        }
        [HttpGet("dashboard")]
        public IActionResult Dashboard()
        {
            int? userInSessions = HttpContext.Session.GetInt32("UserId");
            if(userInSessions == null)
            {
                return Redirect("/");
            }
            else
            {
                User user = _context.Users.FirstOrDefault(u => u.UserId == HttpContext.Session.GetInt32("UserId"));
                ViewBag.User = user;
                List<Event> events = _context.Events.Include(e => e.Planner).Include(e => e.Participants).ThenInclude(p => p.User).OrderBy(e => e.Date).ToList();
                return View(events);
            }
        }
        [HttpGet("rsvp/{eventId}/{userId}")]
        public IActionResult RSVP(int eventId, int userId)
        {
            Invite rsvp = new Invite();
            rsvp.EventId = eventId;
            rsvp.UserId = userId;
            _context.Invites.Add(rsvp);
            _context.SaveChanges();
            return Redirect("/dashboard");
        }
        [HttpGet("rsvp/undo/{eventId}/{userId}")]
        public IActionResult UnRSVP(int eventId, int userId)
        {
            Invite unrsvp = _context.Invites.FirstOrDefault(i => i.UserId == userId && i.EventId == eventId);
            _context.Invites.Remove(unrsvp);
            _context.SaveChanges();
            return Redirect("/dashboard");
        }
        [HttpGet("delete/{eventId}")]
        public IActionResult Delete(int eventId)
        {
            Event delete = _context.Events.FirstOrDefault(e => e.EventId == eventId);
            _context.Events.Remove(delete);
            _context.SaveChanges();
            return Redirect("/dashboard");
        }
        [HttpGet("event/new")]
        public IActionResult NewEvent()
        {
            return View();
        }
        [HttpPost("event/new/create")]
        public IActionResult CreateEvent(Event newEvent)
        {
            if(ModelState.IsValid)
            {
                newEvent.UserId = (int)HttpContext.Session.GetInt32("UserId");
                _context.Events.Add(newEvent);
                _context.SaveChanges();
                return Redirect($"/event/info/{newEvent.EventId}");
            }
            else
            {
                return View("NewEvent");
            }
        }
        [HttpGet("event/info/{eventId}")]
        public IActionResult EventInfo(int eventId)
        {
            User user = _context.Users.FirstOrDefault(u => u.UserId == HttpContext.Session.GetInt32("UserId"));
            ViewBag.User = user;
            Event eventInfo = _context.Events.Include(e => e.Planner).Include(e => e.Participants).ThenInclude(p => p.User).FirstOrDefault(e => e.EventId == eventId);
            return View(eventInfo);
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
