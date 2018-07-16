using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PizzaPlanet.DBData;

namespace PizzaPlanet.Web
{
    public class StoreController : Controller
    {
        private readonly Project1PizzaPlanetContext _context;
        private static string LastSort = "Most Recent";

        public StoreController(Project1PizzaPlanetContext context)
        {
            _context = context;
        }

        // GET: Store
        public async Task<IActionResult> Index()
        {
            return View(await _context.Store.ToListAsync());
        }

        public async Task<IActionResult> HistoryChoose()
        {
            return View(await _context.Store.ToListAsync());
        }

        //private static readonly string[] sorts = { "Most Recent", "Least Recent", "Most Expensive", "Least Expensive" };
        public async Task<IActionResult> History(int? id, string sort = "")
        {
            if (sort == "")
                sort = LastSort;
            if (id == null)
            {
                return NotFound();
            }
            var store = await _context.Store
                .FirstOrDefaultAsync(m => m.Id == id);
            if (store == null)
            {
                return NotFound();
            }
            LastSort = sort;
            ViewData["Sort"] = sort;
            ViewData["Store"] = id;
            switch (sort)
            {
                case "Most Recent":
                    return View(await _context.PizzaOrder.Where(o => (o.StoreId == id)).OrderBy(o => (DateTime.Now - o.OrderTime)).ToListAsync());
                case "Least Recent":
                    return View(await _context.PizzaOrder.Where(o => (o.StoreId == id)).OrderBy(o => o.OrderTime).ToListAsync());
                case "Most Expensive":
                    return View(await _context.PizzaOrder.Where(o => (o.StoreId == id)).OrderBy(o => (500 - o.Total)).ToListAsync());
                case "Least Expensive":
                    return View(await _context.PizzaOrder.Where(o => (o.StoreId == id)).OrderBy(o => o.Total).ToListAsync());
                default:
                    break;
            }
            return View(await _context.PizzaOrder.Where(o => (o.StoreId == id)).OrderBy(o => o.OrderTime).ToListAsync());
        }

        // GET: Store/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var store = await _context.Store
                .FirstOrDefaultAsync(m => m.Id == id);
            if (store == null)
            {
                return NotFound();
            }

            return View(store);
        }

        // GET: Store/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Store/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Income,NextOrder,Dough,Sauce,Cheese,Pepperoni,Sausage,Ham,Bacon,Beef,Onion,GreenPepper,Mushroom,BlackOlive")] Store store)
        {
            if (ModelState.IsValid)
            {
                _context.Add(store);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(store);
        }

        // GET: Store/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var store = await _context.Store.FindAsync(id);
            if (store == null)
            {
                return NotFound();
            }
            return View(store);
        }

        // POST: Store/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Income,NextOrder,Dough,Sauce,Cheese,Pepperoni,Sausage,Ham,Bacon,Beef,Onion,GreenPepper,Mushroom,BlackOlive")] Store store)
        {
            if (id != store.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(store);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StoreExists(store.Id))
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
            return View(store);
        }

        // GET: Store/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var store = await _context.Store
                .FirstOrDefaultAsync(m => m.Id == id);
            if (store == null)
            {
                return NotFound();
            }

            return View(store);
        }

        // POST: Store/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var store = await _context.Store.FindAsync(id);
            _context.Store.Remove(store);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StoreExists(int id)
        {
            return _context.Store.Any(e => e.Id == id);
        }
    }
}
