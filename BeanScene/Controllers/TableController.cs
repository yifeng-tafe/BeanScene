using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BeanScene.Areas.Identity.Data;
using BeanScene.Models;
using BeanScene.ViewModels;

namespace BeanScene.Controllers
{
    public class TableController : Controller
    {
        private readonly ApplicationDBContext _context;

        public TableController(ApplicationDBContext context)
        {
            _context = context;
        }

        // GET: Table
        public async Task<IActionResult> Index()
        {
            var applicationDBContext = _context.Table.Include(t => t.Areas);
            return View(await applicationDBContext.ToListAsync());
        }

        // GET: Table/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Table == null)
            {
                return NotFound();
            }

            var table = await _context.Table
                .Include(t => t.Areas)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (table == null)
            {
                return NotFound();
            }

            var model = new TableViewModel()
            {
                Table = table,
                Areas = _context.Area.ToList()
            };

            return View(model);
        }

        // GET: Table/Create
        public IActionResult Create()
        {
            //ViewData["AreaID"] = new SelectList(_context.Area, "Id", "Name");

            var model = new TableViewModel()
            {
                Areas = _context.Area.ToList()
            };
            return View(model);
        }

        // POST: Table/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TableViewModel tableVM)
        {
            var table = new Table
            {
                Name = tableVM.Table.Name,
                Areas = tableVM.Table.Areas,
                AreaID = tableVM.Table.AreaID

            };
            
            
            
            //if (ModelState.IsValid)
            //{
                _context.Add(table);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            //}
            //ViewData["AreaID"] = new SelectList(_context.Area, "Id", "Name", table.AreaID);
            return View(table);
        }

        // GET: Table/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Table == null)
            {
                return NotFound();
            }

            var table = await _context.Table.FindAsync(id);
            if (table == null)
            {
                return NotFound();
            }

            var model = new TableViewModel()
            {
                Table = table,
                Areas = _context.Area.ToList()
            };
            
            
            
            //ViewData["AreaID"] = new SelectList(_context.Area, "Id", "Name", table.AreaID);
            return View(model);
        }

        // POST: Table/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(TableViewModel tableVM)
        {
            //if (id != table.Id)
            //{
            //    return NotFound();
            //}

            //if (ModelState.IsValid)
            //{
            //    try
            //    {
            //        _context.Update(table);
            //        await _context.SaveChangesAsync();
            //    }
            //    catch (DbUpdateConcurrencyException)
            //    {
            //        if (!TableExists(table.Id))
            //        {
            //            return NotFound();
            //        }
            //        else
            //        {
            //            throw;
            //        }
            //    }
            //    return RedirectToAction(nameof(Index));
            //}

            tableVM.Areas = _context.Area.ToList();
            _context.Update(tableVM.Table);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
            //ViewData["AreaID"] = new SelectList(_context.Area, "Id", "Name", table.AreaID);
            //return View(table);
        }

        // GET: Table/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Table == null)
            {
                return NotFound();
            }

            var table = await _context.Table
                .Include(t => t.Areas)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (table == null)
            {
                return NotFound();
            }
            var model = new TableViewModel()
            {
                Table = table,
                Areas = _context.Area.ToList()
            };

            return View(model);
        }

        // POST: Table/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Table == null)
            {
                return Problem("Entity set 'ApplicationDBContext.Table'  is null.");
            }
            var table = await _context.Table.FindAsync(id);
            if (table != null)
            {
                _context.Table.Remove(table);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TableExists(int id)
        {
          return (_context.Table?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
