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
using Microsoft.AspNetCore.Authorization;

namespace BeanScene.Controllers
{
    [Authorize(Roles = "Manager, Staff")]
    public class TableAvailabilityController : Controller
    {
        private readonly ApplicationDBContext _context;

        public TableAvailabilityController(ApplicationDBContext context)
        {
            _context = context;
        }

        // GET: TableAvailability
        public async Task<IActionResult> Index()
        {
            var applicationDBContext = _context.TableAvailability.Include(t => t.Reservation).Include(t => t.Table);
            return View(await applicationDBContext.ToListAsync());
        }

        // GET: TableAvailability/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.TableAvailability == null)
            {
                return NotFound();
            }

            var tableAvailability = await _context.TableAvailability
                .Include(t => t.Reservation)
                .Include(t => t.Table)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tableAvailability == null)
            {
                return NotFound();
            }

            return View(tableAvailability);
        }

        // GET: TableAvailability/Create
        public IActionResult Create()
        {
            ViewData["ReservationId"] = new SelectList(_context.Reservation, "Id", "FirstName");
            ViewData["TableId"] = new SelectList(_context.Table, "Id", "Name");
            //ViewData["TableAvailability"] = new TableAvailability { Availability = TableAvailability.StatusEnum.Reserved };

            return View();
        }

        // POST: TableAvailability/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create (TableAvailabilityViewModel tableAvailabilityVM)
        {
            tableAvailabilityVM.TableAvailability.Availability = TableAvailability.StatusEnum.Reserved;
            
            //if (ModelState.IsValid)
            //{
                _context.Add(tableAvailabilityVM.TableAvailability);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            //}
            ViewData["ReservationId"] = new SelectList(_context.Reservation, "Id", "ContactNumber", tableAvailabilityVM.TableAvailability.ReservationId);
            ViewData["TableId"] = new SelectList(_context.Table, "Id", "Name", tableAvailabilityVM.TableAvailability.TableId);
            return RedirectToAction(nameof(Index));
        }

        // GET: TableAvailability/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.TableAvailability == null)
            {
                return NotFound();
            }

            var tableAvailability = await _context.TableAvailability.FindAsync(id);
            if (tableAvailability == null)
            {
                return NotFound();
            }
            ViewData["ReservationId"] = new SelectList(_context.Reservation, "Id", "ContactNumber", tableAvailability.ReservationId);
            ViewData["TableId"] = new SelectList(_context.Table, "Id", "Name", tableAvailability.TableId);
            return View(tableAvailability);
        }

        // POST: TableAvailability/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ReservationId,TableId,Availability")] TableAvailability tableAvailability)
        {
            if (id != tableAvailability.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tableAvailability);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TableAvailabilityExists(tableAvailability.Id))
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
            ViewData["ReservationId"] = new SelectList(_context.Reservation, "Id", "ContactNumber", tableAvailability.ReservationId);
            ViewData["TableId"] = new SelectList(_context.Table, "Id", "Name", tableAvailability.TableId);
            return View(tableAvailability);
        }

        // GET: TableAvailability/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.TableAvailability == null)
            {
                return NotFound();
            }

            var tableAvailability = await _context.TableAvailability
                .Include(t => t.Reservation)
                .Include(t => t.Table)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tableAvailability == null)
            {
                return NotFound();
            }

            return View(tableAvailability);
        }

        // POST: TableAvailability/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.TableAvailability == null)
            {
                return Problem("Entity set 'ApplicationDBContext.TableAvailability'  is null.");
            }
            var tableAvailability = await _context.TableAvailability.FindAsync(id);
            if (tableAvailability != null)
            {
                _context.TableAvailability.Remove(tableAvailability);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TableAvailabilityExists(int id)
        {
          return _context.TableAvailability.Any(e => e.Id == id);
        }
    }
}
