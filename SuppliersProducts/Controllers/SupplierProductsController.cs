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
    public class SupplierProductsController : Controller
    {
        private readonly DataBaseContext _context;

        public SupplierProductsController(DataBaseContext context)
        {
            _context = context;
        }

        // GET: SupplierProducts
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Index()
        {
            var dataBaseContext = _context.SupplierProduct.Include(s => s.Product).Include(s => s.Supplier);
            return View(await dataBaseContext.ToListAsync());
        }

        // GET: SupplierProducts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var supplierProduct = await _context.SupplierProduct
                .Include(s => s.Product)
                .Include(s => s.Supplier)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (supplierProduct == null)
            {
                return NotFound();
            }

            return View(supplierProduct);
        }

        // GET: SupplierProducts/Create
        public IActionResult Create()
        {
            ViewData["ProductID"] = new SelectList(_context.Products, "ID", "Name");
            ViewData["SupplierID"] = new SelectList(_context.Supplier, "ID", "FullName");
            return View();
        }

        // POST: SupplierProducts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,ProductID,SupplierID")] SupplierProduct supplierProduct)
        {
            if (ModelState.IsValid)
            {
                _context.Add(supplierProduct);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProductID"] = new SelectList(_context.Products, "ID", "Name", supplierProduct.ProductID);
            ViewData["SupplierID"] = new SelectList(_context.Supplier, "ID", "FullName", supplierProduct.SupplierID);
            return View(supplierProduct);
        }

        // GET: SupplierProducts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var supplierProduct = await _context.SupplierProduct.FindAsync(id);
            if (supplierProduct == null)
            {
                return NotFound();
            }
            ViewData["ProductID"] = new SelectList(_context.Products, "ID", "Name", supplierProduct.ProductID);
            ViewData["SupplierID"] = new SelectList(_context.Supplier, "ID", "FullName", supplierProduct.SupplierID);
            return View(supplierProduct);
        }

        // POST: SupplierProducts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,ProductID,SupplierID")] SupplierProduct supplierProduct)
        {
            if (id != supplierProduct.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(supplierProduct);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SupplierProductExists(supplierProduct.ID))
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
            ViewData["ProductID"] = new SelectList(_context.Products, "ID", "Name", supplierProduct.ProductID);
            ViewData["SupplierID"] = new SelectList(_context.Supplier, "ID", "FullName", supplierProduct.SupplierID);
            return View(supplierProduct);
        }

        // GET: SupplierProducts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var supplierProduct = await _context.SupplierProduct
                .Include(s => s.Product)
                .Include(s => s.Supplier)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (supplierProduct == null)
            {
                return NotFound();
            }

            return View(supplierProduct);
        }

        // POST: SupplierProducts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var supplierProduct = await _context.SupplierProduct.FindAsync(id);
            _context.SupplierProduct.Remove(supplierProduct);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SupplierProductExists(int id)
        {
            return _context.SupplierProduct.Any(e => e.ID == id);
        }
    }
}
