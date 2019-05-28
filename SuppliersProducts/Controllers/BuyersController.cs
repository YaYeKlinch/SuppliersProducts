using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SuppliersProducts.Data;
using SuppliersProducts.Models;

namespace SuppliersProducts.Controllers
{
    public class BuyersController : Controller
    {
        private readonly DataBaseContext _context;

        public BuyersController(DataBaseContext context)
        {
            _context = context;
        }

        // GET: Buyers
        public async Task<IActionResult> Index(string sortOrder, string SearchStringName, string SearchStringNationality)
        {
            ViewData["FullNameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "fullname_desc" : "";
            ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";
            ViewData["AddressSortParm"] = sortOrder == "Address" ? "address_desc" : "Address";

            var buyers = from s in _context.Buyers
                             select s;

            if (!String.IsNullOrEmpty(SearchStringName))
            {
                buyers = buyers.Where(s => s.FullName.Contains(SearchStringName));
                ViewData["CurrentFullNameFilter"] = SearchStringName;
            }
            if (!String.IsNullOrEmpty(SearchStringNationality))
            {
                buyers = buyers.Where(s => s.Address.Contains(SearchStringNationality));
                ViewData["CurrentNationalityFilter"] = SearchStringNationality;
            }
            switch (sortOrder)
            {
                case "fullname_desc":
                    buyers = buyers.OrderByDescending(s => s.FullName);
                    break;
                case "Date":
                    buyers = buyers.OrderBy(s => s.Date);
                    break;
                case "date_desc":
                    buyers = buyers.OrderByDescending(s => s.Date);
                    break;
                case "Address":
                    buyers = buyers.OrderBy(s => s.Address);
                    break;
                case "address_desc":
                    buyers = buyers.OrderByDescending(s => s.Address);
                    break;
                default:
                    buyers = buyers.OrderBy(s => s.FullName);
                    break;
            }

            return View(await buyers.AsNoTracking().ToListAsync());
        }

        // GET: Buyers/Details/5
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var buyer = await _context.Buyers
                .FirstOrDefaultAsync(m => m.ID == id);
            if (buyer == null)
            {
                return NotFound();
            }

            return View(buyer);
        }

        // GET: Buyers/Create
        [Authorize(Roles = "admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Buyers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Create([Bind("ID,FullName,Date,Address")] Buyer buyer)
        {
            if (ModelState.IsValid)
            {
                _context.Add(buyer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(buyer);
        }

        // GET: Buyers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var buyer = await _context.Buyers.FindAsync(id);
            if (buyer == null)
            {
                return NotFound();
            }
            return View(buyer);
        }

        // POST: Buyers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(int id, [Bind("ID,FullName,Date,Address")] Buyer buyer)
        {
            if (id != buyer.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(buyer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BuyerExists(buyer.ID))
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
            return View(buyer);
        }

        // GET: Buyers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var buyer = await _context.Buyers
                .FirstOrDefaultAsync(m => m.ID == id);
            if (buyer == null)
            {
                return NotFound();
            }

            return View(buyer);
        }

        // POST: Buyers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var buyer = await _context.Buyers.FindAsync(id);
            _context.Buyers.Remove(buyer);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BuyerExists(int id)
        {
            return _context.Buyers.Any(e => e.ID == id);
        }
    }
}
