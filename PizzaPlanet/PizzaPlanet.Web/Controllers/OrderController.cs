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
        public static PizzaPlanet.Library.Order order = null;

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
        public async Task<IActionResult> Details(decimal? id, bool user)
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
            ViewData["Previous"] = user;
            return View(PizzaPlanet.Library.Mapper.Map(pizzaOrder));
        }

        // GET: Order/Create
        public async Task<IActionResult> New()
        {
            ViewData["StoreIds"] = await _context.Store.Select(s=>s.Id).ToListAsync();
            return View();
        }
        
        public async Task<IActionResult> Create(int id)
        {
            var orders = await _context.PizzaOrder.Where(o => o.Username == UserController.user.Name).Where(o =>o.StoreId==id).ToListAsync();
            DateTime lastOrder = DateTime.MinValue;
            if (orders.Count() > 0)
                lastOrder = orders.OrderBy(o => DateTime.Now - o.OrderTime).First().OrderTime;
            var mins = 120 - Math.Ceiling((DateTime.Now - lastOrder).TotalMinutes);
            if (mins > 0)
            {
                string msg  = "You have ordered from Store#"+ id + " too recently. Try again in " + mins + " minutes.";
                ViewData["Message"] = msg;
                return View("Message");
            }
            order = new PizzaPlanet.Library.Order(UserController.user, PizzaPlanet.Library.Mapper.Map(await _context.Store.Where(s => s.Id == id).SingleAsync()));
            return RedirectToAction("Edit");
        }

        // GET: Order/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id != null && id != order.Id) {
                var orders = await _context.PizzaOrder.Where(o => o.Username == UserController.user.Name).Where(o => o.StoreId == id).ToListAsync();
                DateTime lastOrder = DateTime.MinValue;
                if (orders.Count() > 0)
                    lastOrder = orders.OrderBy(o => DateTime.Now - o.OrderTime).First().OrderTime;
                var mins = 120 - Math.Ceiling((DateTime.Now - lastOrder).TotalMinutes);
                if (mins > 0)
                {
                    ViewData["Message"] = "You have ordered from Store#" + id + " too recently. Try again in " + mins + " minutes.";
                }
                else
                {
                    order.Store = PizzaPlanet.Library.Store.GetStore((int)id);
                }
            }
            ViewData["StoreIds"] = await _context.Store.Select(s => s.Id).ToListAsync();
            return View(order);
        }

        public IActionResult EditPizza()
        {
            return View(new Library.Pizza());
        }

        public IActionResult DeletePizza(int i)
        {
            order.RemovePizza(i);
            return RedirectToAction("Edit");

        }

        [HttpPost]
        public IActionResult CompletePizza(Library.Pizza pizza)
        {
            order.AddPizza(pizza);
            return RedirectToAction("Edit");
        }

        public IActionResult Submit()
        {
            if (order.Store.PlaceOrder(order))
            {
                PizzaPlanet.Library.PizzaRepository.Repo().PlaceOrder(order);
                string msg = "Order " + order.IdFull() + " successfully placed!";
                ViewData["Message"] = msg;
            }
            else
            {
                ViewData["Message"] = "There was an error in placing your order. Please call Store#" + order.Store.Id + " for details.";
            }
            order = null;
            return View("Message");
        }

        public IActionResult Delete()
        {
            order = null;
            return RedirectToAction("Index", "Home");
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
