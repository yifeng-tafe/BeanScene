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
    public class ReservationTimeController : Controller
    {
        private readonly ApplicationDBContext _context;

        public ReservationTimeController(ApplicationDBContext context)
        {
            _context = context;
        }

        // GET: ReservationTime
        public async Task<IActionResult> Index()
        {
            var applicationDBContext = _context.ReservationTime.Include(f => f.ReservationTypes);
            return View(await applicationDBContext.ToListAsync());
        }

        // GET: ReservationTime/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ReservationTime == null)
            {
                return NotFound();
            }

            var reservationTime = await _context.ReservationTime
                .Include(f => f.ReservationTypes)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reservationTime == null)
            {
                return NotFound();
            }

            var model = new ReservationTimeViewModel()
            {
                ReservationTime = reservationTime,
                ReservationTypes = _context.ReservationType.ToList()
            };
            return View(model);
        }

        // GET: ReservationTime/Create
        public IActionResult Create()
        {
            var model = new ReservationTimeViewModel()
            {
                ReservationTypes = _context.ReservationType.ToList()
            };
            return View(model);
        }

        // POST: ReservationTime/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ReservationTimeViewModel rsvTimeVM)
        {
            var reservationTime = new ReservationTime
            {
                Name = rsvTimeVM.ReservationTime.Name,
                ReservationTypes = rsvTimeVM.ReservationTime.ReservationTypes,
                ReservationTypeId = rsvTimeVM.ReservationTime.ReservationTypeId,
            };
            //if (ModelState.IsValid)
            //{
            _context.Add(reservationTime);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
            //}
            return View(reservationTime);
        }

        // GET: ReservationTime/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ReservationTime == null)
            {
                return NotFound();
            }


            var reservationTime = await _context.ReservationTime.FindAsync(id);
            if (reservationTime == null)
            {
                return NotFound();
            }

            var model = new ReservationTimeViewModel()
            {
                ReservationTime = reservationTime,
                ReservationTypes = _context.ReservationType.ToList()
            };
            return View(model);
        }

        // POST: ReservationTime/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ReservationTimeViewModel rsvTimeVM)
        {
            //if (id != reservationTime.Id)
            //{
            //    return NotFound();
            //}

            //if (ModelState.IsValid)
            //{
            //    try
            //    {
            //        _context.Update(reservationTime);
            //        await _context.SaveChangesAsync();
            //    }
            //    catch (DbUpdateConcurrencyException)
            //    {
            //        if (!ReservationTimeExists(reservationTime.Id))
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
            rsvTimeVM.ReservationTypes = _context.ReservationType.ToList();
            _context.Update(rsvTimeVM.ReservationTime);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: ReservationTime/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ReservationTime == null)
            {
                return NotFound();
            }

            var reservationTime = await _context.ReservationTime
                .Include(t => t.ReservationTypes)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reservationTime == null)
            {
                return NotFound();
            }

            var model = new ReservationTimeViewModel()
            {
                ReservationTime = reservationTime,
                ReservationTypes = _context.ReservationType.ToList()
            };

            return View(model);
        }

        // POST: ReservationTime/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ReservationTime == null)
            {
                return Problem("Entity set 'ApplicationDBContext.ReservationTime'  is null.");
            }
            var reservationTime = await _context.ReservationTime.FindAsync(id);
            if (reservationTime != null)
            {
                _context.ReservationTime.Remove(reservationTime);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReservationTimeExists(int id)
        {
            return (_context.ReservationTime?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
