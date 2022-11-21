using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BeanScene.Areas.Identity.Data;
using BeanScene.Models;
using Microsoft.AspNetCore.Authorization;

namespace BeanScene.Controllers
{
    [Authorize(Roles = "Manager")]
    public class ReservationTypeController : Controller
    {
        private readonly ApplicationDBContext _context;

        public ReservationTypeController(ApplicationDBContext context)
        {
            _context = context;
        }

        // GET: ReservationType
        public async Task<IActionResult> Index()
        {
              return _context.ReservationType != null ? 
                          View(await _context.ReservationType.ToListAsync()) :
                          Problem("Entity set 'ApplicationDBContext.ReservationType'  is null.");
        }

        // GET: ReservationType/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ReservationType == null)
            {
                return NotFound();
            }

            var reservationType = await _context.ReservationType
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reservationType == null)
            {
                return NotFound();
            }

            return View(reservationType);
        }

        // GET: ReservationType/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ReservationType/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] ReservationType reservationType)
        {
            if (ModelState.IsValid)
            {
                _context.Add(reservationType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(reservationType);
        }

        // GET: ReservationType/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ReservationType == null)
            {
                return NotFound();
            }

            var reservationType = await _context.ReservationType.FindAsync(id);
            if (reservationType == null)
            {
                return NotFound();
            }
            return View(reservationType);
        }

        // POST: ReservationType/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] ReservationType reservationType)
        {
            if (id != reservationType.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(reservationType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReservationTypeExists(reservationType.Id))
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
            return View(reservationType);
        }

        // GET: ReservationType/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ReservationType == null)
            {
                return NotFound();
            }

            var reservationType = await _context.ReservationType
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reservationType == null)
            {
                return NotFound();
            }

            return View(reservationType);
        }

        // POST: ReservationType/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ReservationType == null)
            {
                return Problem("Entity set 'ApplicationDBContext.ReservationType'  is null.");
            }
            var reservationType = await _context.ReservationType.FindAsync(id);
            if (reservationType != null)
            {
                _context.ReservationType.Remove(reservationType);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReservationTypeExists(int id)
        {
          return (_context.ReservationType?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
