using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using SuppliersProducts.Data;
using SuppliersProducts.Models;
using SuppliersProducts.ViewModel;

namespace SuppliersProducts.Controllers
{
    public class StudentLabWorksController : Controller
    {
        private readonly DataBaseContext _context;
        IHostingEnvironment _hostingEnvironment;

        public StudentLabWorksController(DataBaseContext context, IHostingEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }

        // GET: SupplierProducts
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var dataBaseContext = _context.StudentLabWork.Include(s => s.LabWork).Include(s => s.Student);
            return View(await dataBaseContext.ToListAsync());
        }

        // GET: SupplierProducts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var studentLabWork = await _context.StudentLabWork
                .Include(s => s.LabWork)
                .Include(s => s.Student)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (studentLabWork == null)
            {
                return NotFound();
            }

            return View(studentLabWork);
        }

        // GET: SupplierProducts/Create
        public IActionResult Create()
        {
            ViewData["LabWorkID"] = new SelectList(_context.LabWorks, "ID", "Name");
            ViewData["StudentID"] = new SelectList(_context.Student, "ID", "FullName");
            ViewData["Path"] = new SelectList(_context.StudentLabWork, "ID", "Path");
            return View();
        }

        // POST: SupplierProducts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,LabWorkID,StudentID,Path")] SLabView sLabView, IFormFile Path)
        {
            /* if (file != null)
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

                        }*/

            if (ModelState.IsValid)
            {
                if(Path!=null)
                {
                    string path = _hostingEnvironment.ContentRootPath + "\\" + Path.FileName;
                    using (var fileStream = new System.IO.FileStream(path, System.IO.FileMode.Create))
                    {
                        await Path.CopyToAsync(fileStream);
                    }
                    sLabView.Path.CopyTo(new System.IO.FileStream(path, System.IO.FileMode.Create));
                    StudentLabWork studentLabWork = new StudentLabWork
                    {
                        StudentID = sLabView.StudentID,
                        LabWorkID = sLabView.LabWorkID,
                        Path = path

                    };
                    _context.Add(studentLabWork);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                ViewData["LabWorkID"] = new SelectList(_context.LabWorks, "ID", "Name", sLabView.LabWorkID);
                ViewData["StudentID"] = new SelectList(_context.Student, "ID", "FullName", sLabView.StudentID);
                //ViewData["Path"] = new SelectList(_context.StudentLabWork, "ID", "Path", sLabView.Path);

            }

            return View();
        }
        public async Task<IActionResult> Compare([Bind("ID,StudentID,Path")] StudentLabWork studentLab)
        {
            /*string res = null;
            if (ModelState.IsValid)
            {
                string f1 = studentLab.Path;
                string f2 = studentLab.Path;                               //Я єбу як прикрутити. Сподіваюся в тебе вийде!)
                Plagiator test = new Plagiator();
                res = test.AveragePlagTest(f1, f2).ToString();
            }

            ViewData["StudentID"] = new SelectList(_context.Student, "ID", "FullName", studentLab.StudentID);
            ViewData["LabWorkID"] = new SelectList(_context.LabWorks, "ID", "Name", studentLab.LabWorkID);
            ViewData["StudentID"] = new SelectList(_context.Student, "ID", "FullName", studentLab.StudentID);
            ViewData["LabWorkID"] = new SelectList(_context.LabWorks, "ID", "Name", studentLab.LabWorkID);*/
            return View();
        }

            // GET: SupplierProducts/Edit/5
            public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var studentLabWork = await _context.StudentLabWork.FindAsync(id);
            if (studentLabWork == null)
            {
                return NotFound();
            }
            ViewData["LabWorkID"] = new SelectList(_context.LabWorks, "ID", "Name", studentLabWork.LabWorkID);
            ViewData["StudentID"] = new SelectList(_context.Student, "ID", "FullName", studentLabWork.StudentID);
            return View(studentLabWork);
        }

        // POST: SupplierProducts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,LabWorkID,StudentID")] StudentLabWork studentLabWork)
        {
            if (id != studentLabWork.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(studentLabWork);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentLabWorkExists(studentLabWork.ID))
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
            ViewData["LabWorkID"] = new SelectList(_context.LabWorks, "ID", "Name", studentLabWork.LabWorkID);
            ViewData["StudentID"] = new SelectList(_context.Student, "ID", "FullName", studentLabWork.StudentID);
            return View(studentLabWork);
        }

        // GET: SupplierProducts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var studentLabWork = await _context.StudentLabWork
                .Include(s => s.LabWork)
                .Include(s => s.Student)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (studentLabWork == null)
            {
                return NotFound();
            }

            return View(studentLabWork);
        }

        // POST: SupplierProducts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var studentLabWork = await _context.StudentLabWork.FindAsync(id);
            _context.StudentLabWork.Remove(studentLabWork);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StudentLabWorkExists(int id)
        {
            return _context.StudentLabWork.Any(e => e.ID == id);
        }
    }
}
