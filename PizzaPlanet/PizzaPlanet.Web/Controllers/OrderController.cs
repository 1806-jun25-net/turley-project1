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
    public class OrderController : Controller
    {
        private readonly Project1PizzaPlanetContext _context;

        public OrderController(Project1PizzaPlanetContext context)
        {
            _context = context;
        }

        // GET: Order
        public async Task<IActionResult> Index()
        {
            var project1PizzaPlanetContext = _context.PizzaOrder.Include(p => p.Store).Include(p => p.UsernameNavigation);
            return View(await project1PizzaPlanetContext.ToListAsync());
        }

        // GET: Order/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pizzaOrder = await _context.PizzaOrder
                .Include(p => p.Store)
                .Include(p => p.UsernameNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pizzaOrder == null)
            {
                return NotFound();
            }

            return View(pizzaOrder);
        }

        // GET: Order/Create
        public IActionResult Create()
        {
            ViewData["StoreId"] = new SelectList(_context.Store, "Id", "Id");
            ViewData["Username"] = new SelectList(_context.PizzaUser, "Username", "Username");
            return View();
        }

        // POST: Order/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,StoreId,Username,OrderTime,Total")] PizzaOrder pizzaOrder)
        {
            if (ModelState.IsValid)
            {
                _context.Add(pizzaOrder);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["StoreId"] = new SelectList(_context.Store, "Id", "Id", pizzaOrder.StoreId);
            ViewData["Username"] = new SelectList(_context.PizzaUser, "Username", "Username", pizzaOrder.Username);
            return View(pizzaOrder);
        }

        // GET: Order/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pizzaOrder = await _context.PizzaOrder.FindAsync(id);
            if (pizzaOrder == null)
            {
                return NotFound();
            }
            ViewData["StoreId"] = new SelectList(_context.Store, "Id", "Id", pizzaOrder.StoreId);
            ViewData["Username"] = new SelectList(_context.PizzaUser, "Username", "Username", pizzaOrder.Username);
            return View(pizzaOrder);
        }

        // POST: Order/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("Id,StoreId,Username,OrderTime,Total")] PizzaOrder pizzaOrder)
        {
            if (id != pizzaOrder.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(pizzaOrder);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PizzaOrderExists(pizzaOrder.Id))
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
            ViewData["StoreId"] = new SelectList(_context.Store, "Id", "Id", pizzaOrder.StoreId);
            ViewData["Username"] = new SelectList(_context.PizzaUser, "Username", "Username", pizzaOrder.Username);
            return View(pizzaOrder);
        }

        // GET: Order/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pizzaOrder = await _context.PizzaOrder
                .Include(p => p.Store)
                .Include(p => p.UsernameNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pizzaOrder == null)
            {
                return NotFound();
            }

            return View(pizzaOrder);
        }

        // POST: Order/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            var pizzaOrder = await _context.PizzaOrder.FindAsync(id);
            _context.PizzaOrder.Remove(pizzaOrder);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PizzaOrderExists(decimal id)
        {
            return _context.PizzaOrder.Any(e => e.Id == id);
        }
    }
}
