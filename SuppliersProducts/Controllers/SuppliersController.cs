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
    public class SuppliersController : Controller
    {
        private readonly DataBaseContext _context;

        public SuppliersController(DataBaseContext context)
        {
            _context = context;
        }

        // GET: Suppliers
        [Authorize(Roles = "admin, user")]
        public async Task<IActionResult> Index(string sortOrder, string SearchStringFullName, string SearchStringNationality, DateTime SearchStringSupplyDate)
        {
            ViewData["FullNameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["NationalitySortParm"] = sortOrder == "Nationality" ? "nationality_desc" : "Nationality";
            ViewData["SupplyDateSortParm"] = sortOrder == "SupplyDate" ? "supplydate_desc" : "SupplyDate";

            var supplier = from s in _context.Supplier
                            select s;

            if (!String.IsNullOrEmpty(SearchStringFullName))
            {
                supplier = supplier.Where(s => s.FullName.Contains(SearchStringFullName));
                ViewData["CurrentFullNameFilter"] = SearchStringFullName;
            }
            if (!String.IsNullOrEmpty(SearchStringNationality))
            {
                supplier = supplier.Where(s => s.Nationality.Contains(SearchStringNationality));
                ViewData["CurrentNationalityFilter"] = SearchStringNationality;
            }
            if (SearchStringSupplyDate != null && SearchStringSupplyDate != DateTime.Parse("1/1/0001 12:00:00 AM"))
            {
                supplier = supplier.Where(s => s.SupplyDate.Equals(SearchStringSupplyDate));
                ViewData["CurrentSupplyDateFilter"] = SearchStringSupplyDate.ToShortDateString();
            }
            switch (sortOrder)
            {
                case "name_desc":
                    supplier = supplier.OrderByDescending(s => s.FullName);
                    break;
                case "Nationality":
                    supplier = supplier.OrderByDescending(s => s.Nationality);
                    break;
                case "nationality_desc":
                    supplier = supplier.OrderBy(s => s.Nationality);
                    break;
                case "SupplyDate":
                    supplier = supplier.OrderBy(s => s.SupplyDate);
                    break;
                case "supplydate_desc":
                    supplier = supplier.OrderByDescending(s => s.SupplyDate);
                    break;
                default:
                    supplier = supplier.OrderBy(s => s.FullName);
                    break;
            }

            return View(await supplier.AsNoTracking().ToListAsync());
        }

        // GET: Suppliers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var supplier = await _context.Supplier
                .FirstOrDefaultAsync(m => m.ID == id);
            if (supplier == null)
            {
                return NotFound();
            }

            return View(supplier);
        }

        // GET: Suppliers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Suppliers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,FullName,Nationality,SupplyDate")] Supplier supplier)
        {
            if (ModelState.IsValid)
            {
                _context.Add(supplier);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(supplier);
        }

        // GET: Suppliers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var supplier = await _context.Supplier.FindAsync(id);
            if (supplier == null)
            {
                return NotFound();
            }
            return View(supplier);
        }

        // POST: Suppliers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,FullName,Nationality,SupplyDate")] Supplier supplier)
        {
            if (id != supplier.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(supplier);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SupplierExists(supplier.ID))
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
            return View(supplier);
        }

        // GET: Suppliers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var supplier = await _context.Supplier
                .FirstOrDefaultAsync(m => m.ID == id);
            if (supplier == null)
            {
                return NotFound();
            }

            return View(supplier);
        }

        // POST: Suppliers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var supplier = await _context.Supplier.FindAsync(id);
            _context.Supplier.Remove(supplier);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SupplierExists(int id)
        {
            return _context.Supplier.Any(e => e.ID == id);
        }
    }
}
