using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PizzaPlanet.DBData;
using PizzaPlanet.Library;
using PizzaPlanet.Web.Models;

namespace PizzaPlanet.Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login([Bind("Username,StoreId")] PizzaUser pizzaUser)
        {
            var tryUser = PizzaPlanet.Library.User.TryUser(pizzaUser.Username);
            if (tryUser != null)
            {
                PizzaPlanet.Web.Controllers.UserController.user = tryUser;
                return View("Index");
            }
            return View();
        }

        public IActionResult Logout()
        {
            if (UserController.user != null)
            {
                ViewData["Message"] = "<" + UserController.user.Name + "> is now logged out!";
                UserController.user = null;
            }
            return View("Login");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
