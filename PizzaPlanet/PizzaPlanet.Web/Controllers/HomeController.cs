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
        public IActionResult Index(string message)
        {
            ViewData["Message"] = message;
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
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        public IActionResult NewUser()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult NewUser([Bind("Username,StoreId")] PizzaUser pizzaUser)
        {
            var tryUser = PizzaPlanet.Library.User.TryUser(pizzaUser.Username);
            if (tryUser == null)
            {
                PizzaPlanet.Web.Controllers.UserController.user = PizzaPlanet.Library.User.MakeUser(pizzaUser.Username);
                return RedirectToAction(nameof(Index));
            }
            ViewData["Message"] = "Username <" + pizzaUser.Username + "> already exists";
            return View();
        }

        //return RedirectToAction(nameof(Index));


        public IActionResult Logout()
        {
            string msg = "";
            if (UserController.user != null)
            {
                msg = "<" + UserController.user.Name + "> is now logged out";
                ViewData["Message"] = msg;
                UserController.user = null;
            }
            return View("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
