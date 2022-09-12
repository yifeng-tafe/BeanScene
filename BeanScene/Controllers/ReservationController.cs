using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BeanScene.Areas.Identity.Data;
using BeanScene.Models;

namespace BeanScene.Controllers
{
    public class ReservationController : Controller
    {
        private readonly ApplicationDBContext _context;

        public ReservationController(ApplicationDBContext context)
        {
            _context = context;
        }

        // GET: Reservation
        public async Task<IActionResult> Index()
        {
            var applicationDBContext = _context.Reservation.Include(r => r.ReserveType);
            return View(await applicationDBContext.ToListAsync());
        }

        // GET: Reservation/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Reservation == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservation
                .Include(r => r.ReserveType)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reservation == null)
            {
                return NotFound();
            }

            return View(reservation);
        }

        // GET: Reservation/Create
        public IActionResult Create()
        {
            ViewData["ReservationTypeId"] = new SelectList(_context.ReservationType, "Id", "Name");
            return View();
        }

        // POST: Reservation/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Reservation reservation)
        {
            //if (ModelState.IsValid)
            //{
            var book = new Reservation
            {
                Id = reservation.Id,
                TableId = reservation.TableId,
                UserId = reservation.UserId,
                ReserveDate = reservation.ReserveDate,
                ReservationTypeId = reservation.ReservationTypeId,
                ReserveType = reservation.ReserveType,
                NumberOfGuest = reservation.NumberOfGuest,
                Requirement = reservation.Requirement,
                FirstName = reservation.FirstName,
                LastName = reservation.LastName,
                Email = reservation.Email,
                ContactNumber = reservation.ContactNumber,
                Status = Reservation.StatusEnum.Requested,
            };

                _context.Add(book);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            //}
            ViewData["ReservationTypeId"] = new SelectList(_context.ReservationType, "Id", "Name", reservation.ReservationTypeId);
            return View(book);
        }

        // GET: Reservation/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Reservation == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservation.FindAsync(id);
            if (reservation == null)
            {
                return NotFound();
            }

            ViewData["ReservationTypeId"] = new SelectList(_context.ReservationType, "Id", "Name", reservation.ReservationTypeId);
            return View(reservation);
        }

        // POST: Reservation/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Reservation reservation)
        {
            //if (id != reservation.Id)
            //{
            //    return NotFound();
            //}

            //if (ModelState.IsValid)
            //{
            //    try
            //    {
            //        _context.Update(reservation);
            //        await _context.SaveChangesAsync();
            //    }
            //    catch (DbUpdateConcurrencyException)
            //    {
            //        if (!ReservationExists(reservation.Id))
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

            _context.Update(reservation);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

            //ViewData["ReservationTypeId"] = new SelectList(_context.ReservationType, "Id", "Name", reservation.ReservationTypeId);
            //return View(reservation);
        }

        // GET: Reservation/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Reservation == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservation
                .Include(r => r.ReserveType)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reservation == null)
            {
                return NotFound();
            }

            return View(reservation);
        }

        // POST: Reservation/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Reservation == null)
            {
                return Problem("Entity set 'ApplicationDBContext.Reservation'  is null.");
            }
            var reservation = await _context.Reservation.FindAsync(id);
            if (reservation != null)
            {
                _context.Reservation.Remove(reservation);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReservationExists(int id)
        {
          return (_context.Reservation?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
