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

        public UserController(Project1PizzaPlanetContext context)
        {
            _context = context;
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

        // GET: User/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: User/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Username,StoreId")] PizzaUser pizzaUser)
        {
            if (ModelState.IsValid)
            {
                _context.Add(pizzaUser);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(pizzaUser);
        }

        // GET: User/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pizzaUser = await _context.PizzaUser.FindAsync(id);
            if (pizzaUser == null)
            {
                return NotFound();
            }
            return View(pizzaUser);
        }

        // POST: User/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Username,StoreId")] PizzaUser pizzaUser)
        {
            if (id != pizzaUser.Username)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(pizzaUser);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PizzaUserExists(pizzaUser.Username))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(pizzaUser);
        }

        // GET: User/Delete/5
        public async Task<IActionResult> Delete(string id)
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

        // POST: User/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var pizzaUser = await _context.PizzaUser.FindAsync(id);
            _context.PizzaUser.Remove(pizzaUser);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PizzaUserExists(string id)
        {
            return _context.PizzaUser.Any(e => e.Username == id);
        }
    }
}
