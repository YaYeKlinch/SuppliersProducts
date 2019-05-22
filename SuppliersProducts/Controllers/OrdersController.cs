using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SuppliersProducts.Data;
using SuppliersProducts.Models;

namespace SuppliersProducts.Controllers
{
    public class OrdersController : Controller
    {
        private readonly DataBaseContext _context;

        public OrdersController(DataBaseContext context)
        {
            _context = context;
        }

        // GET: Orders
        public async Task<IActionResult> Index(string sortOrder, string searchStringName, DateTime searchStringDate, string searchStringPlace)
        {
            var dataBaseContext = _context.Orders.Include(o => o.Buyer).Include(o => o.Product);

            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";
            ViewData["PlaceSortParm"] = sortOrder == "Place" ? "place_desc" : "Place";
            ViewData["ProductSortParm"] = sortOrder == "Product" ? "product_desc" : "Product";
            ViewData["BuyerSortParm"] = sortOrder == "Buyer" ? "buyer_desc" : "buyer";


            var orders = from s in dataBaseContext
                               select s;

            if (!String.IsNullOrEmpty(searchStringName))
            {
                orders = orders.Where(s => s.Name.Contains(searchStringName));
                ViewData["CurrentNameFilter"] = searchStringName;
            }
            if (searchStringDate != null && searchStringDate != DateTime.Parse("1/1/0001 12:00:00 AM"))
            {
                orders = orders.Where(s => s.Date.Equals(searchStringDate));
                ViewData["CurrentDateFilter"] = searchStringDate.ToShortDateString();
            }
            if (!String.IsNullOrEmpty(searchStringPlace))
            {
                orders = orders.Where(s => s.Place.Contains(searchStringPlace));
                ViewData["CurrentPlaceFilter"] = searchStringPlace;
            }

            switch (sortOrder)
            {
                case "name_desc":
                    orders = orders.OrderByDescending(s => s.Name);
                    break;
                case "Date":
                    orders = orders.OrderBy(s => s.Date);
                    break;
                case "date_desc":
                    orders = orders.OrderByDescending(s => s.Date);
                    break;
                case "Place":
                    orders = orders.OrderBy(s => s.Place);
                    break;
                case "place_desc":
                    orders = orders.OrderByDescending(s => s.Place);
                    break;
                case "Product":
                    orders = orders.OrderBy(s => s.Product.Name);
                    break;
                case "product_desc":
                    orders = orders.OrderByDescending(s => s.Product.Name);
                    break;
                case "Buyer":
                    orders = orders.OrderBy(s => s.Buyer.FullName);
                    break;
                case "buyer_desc":
                    orders = orders.OrderByDescending(s => s.Buyer.FullName);
                    break;

                default:
                    orders = orders.OrderBy(s => s.Name);
                    break;
            }


            return View(await orders.AsNoTracking().ToListAsync());
        }

        // GET: Orders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.Buyer)
                .Include(o => o.Product)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // GET: Orders/Create
        public IActionResult Create()
        {
            ViewData["BuyerID"] = new SelectList(_context.Buyers, "ID", "Address");
            ViewData["ProductID"] = new SelectList(_context.Products, "ID", "Name");
            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,ProductID,BuyerID,Name,Date,Place")] Order order)
        {
            if (ModelState.IsValid)
            {
                _context.Add(order);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BuyerID"] = new SelectList(_context.Buyers, "ID", "Address", order.BuyerID);
            ViewData["ProductID"] = new SelectList(_context.Products, "ID", "Name", order.ProductID);
            return View(order);
        }

        // GET: Orders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            ViewData["BuyerID"] = new SelectList(_context.Buyers, "ID", "Address", order.BuyerID);
            ViewData["ProductID"] = new SelectList(_context.Products, "ID", "Name", order.ProductID);
            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,ProductID,BuyerID,Name,Date,Place")] Order order)
        {
            if (id != order.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.ID))
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
            ViewData["BuyerID"] = new SelectList(_context.Buyers, "ID", "Address", order.BuyerID);
            ViewData["ProductID"] = new SelectList(_context.Products, "ID", "Name", order.ProductID);
            return View(order);
        }

        // GET: Orders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.Buyer)
                .Include(o => o.Product)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.ID == id);
        }
    }
}
