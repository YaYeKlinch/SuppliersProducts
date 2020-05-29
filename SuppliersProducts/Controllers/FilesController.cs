using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SuppliersProducts.Data;
using SuppliersProducts.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using SuppliersProducts.ViewModel;
using System.IO;
using Microsoft.AspNetCore.Authorization;
using System.Runtime.InteropServices.ComTypes;

namespace SuppliersProducts.Controllers
{
    public class FilesController : Controller
    {
        private readonly DataBaseContext _context;
        private readonly IHostingEnvironment _hostingEnvironment;
        public FilesController(DataBaseContext context, IHostingEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }

        // GET: Files
        [Authorize]
        public async Task<IActionResult> Index()
        {
            return View(await _context.File.ToListAsync());
        }

        // GET: Files/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var file = await _context.File
                .FirstOrDefaultAsync(m => m.ID == id);
            if (file == null)
            {
                return NotFound();
            }

            return View(file);
        }

        // GET: Files/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Files/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,file")] FileView model, IFormFile file)
        {
            /*var dir = _hostingEnvironment.ContentRootPath;

            using (var fileStream=new FileStream(Path.Combine(dir,file.FileName),FileMode))*/
            if (file != null)
            {

                string path = _hostingEnvironment.ContentRootPath +"\\"+ file.FileName;

                using (var fileStream = new System.IO.FileStream(path, System.IO.FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }
                model.file.CopyTo(new System.IO.FileStream(path, System.IO.FileMode.Create));
                ViewModel.File newFile = new ViewModel.File
                {
                    filePath = path
                };
                _context.Add(newFile);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));

            }

            return View();
        }
        // GET: Files/Edit/5
        public IActionResult Compare()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Compare(string fp1,string fp2)
        {
            Plagiator test = new Plagiator();
            double comparator = test.AveragePlagTest(fp1, fp2);

            return Content(comparator.ToString());
        }

        // POST: Files/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,filePath")] ViewModel.File file)
        {
            if (id != file.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(file);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FileExists(file.ID))
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
            return View(file);
        }

        // GET: Files/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var file = await _context.File
                .FirstOrDefaultAsync(m => m.ID == id);
            if (file == null)
            {
                return NotFound();
            }

            return View(file);
        }

        // POST: Files/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var file = await _context.File.FindAsync(id);
            _context.File.Remove(file);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FileExists(int id)
        {
            return _context.File.Any(e => e.ID == id);
        }
    }
}
