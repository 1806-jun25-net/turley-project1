using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PizzaPlanet.DBData;

namespace PizzaPlanet.Web.Controllers
{
    public class UserController : Controller
    {
        private readonly Project1PizzaPlanetContext _context;
        public static PizzaPlanet.Library.User user = null;
        public static string LastSort = "Most Recent";

        public UserController(Project1PizzaPlanetContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> History(string sort = "")
        {
            if (sort == "")
                sort = LastSort;
            LastSort = sort;
            ViewData["Sort"] = sort;
            switch (sort)
            {
                case "Most Recent":
                    return View(await _context.PizzaOrder.Where(u => (u.Username == user.Name)).OrderBy(o=> (DateTime.Now - o.OrderTime)).ToListAsync());
                case "Least Recent":
                    return View(await _context.PizzaOrder.Where(u => (u.Username == user.Name)).OrderBy(o => o.OrderTime).ToListAsync());
                case "Most Expensive":
                    return View(await _context.PizzaOrder.Where(u => (u.Username == user.Name)).OrderBy(o => (500 - o.Total)).ToListAsync());
                case "Least Expensive":
                    return View(await _context.PizzaOrder.Where(u => (u.Username == user.Name)).OrderBy(o => o.Total).ToListAsync());
                default:
                    break;
            }
            return View(await _context.PizzaOrder.Where(u => (u.Username == user.Name)).ToListAsync());
        }

        // GET: User
        public async Task<IActionResult> Index()
        {
            return View(await _context.PizzaUser.ToListAsync());
        }

        // GET: User/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pizzaUser = await _context.PizzaUser
                .FirstOrDefaultAsync(m => m.Username == id);
            if (pizzaUser == null)
            {
                return NotFound();
            }

            return View(pizzaUser);
        }

        private bool PizzaUserExists(string id)
        {
            return _context.PizzaUser.Any(e => e.Username == id);
        }
    }
}
