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
using Microsoft.AspNetCore.Identity;

namespace BeanScene.Controllers
{
    public class ReservationController : Controller
    {
        private readonly ApplicationDBContext _context;

        public ReservationController(ApplicationDBContext context)
        {
            _context = context;
        }

        // GET: Reservations

        [Authorize(Roles = "Manager, Staff")]
        public async Task<IActionResult> Index()
        {
            var applicationDBContext = _context.Reservation.Include(r => r.ReserveTime).Where(s => s.Status == Reservation.StatusEnum.Requested);
            return View(await applicationDBContext.ToListAsync());
        }


        [Authorize(Roles = "Manager, Staff")]
        private async Task<int> GetTotalAvailableTables(DateTime? reservationDate, ReservationTime reservationTime)
        {
            int totalTables = await _context.Table.CountAsync();

            int totalUnavailableTables = GetUnavailableTables(reservationDate, reservationTime).Count();

            return totalTables - totalUnavailableTables;
        }


        [Authorize(Roles = "Manager, Staff")]
        private List<Table> GetUnavailableTables(DateTime? reservationDate, ReservationTime reservationTime)
        {
            // Get a list of unavailable tables
            List<Table> unavailableTables = _context.TableAvailability
                .Where(ta =>
                    ta.Reservation.ReservationDate == reservationDate &&
                    ta.Reservation.ReserveTime.Id == reservationTime.Id &&
                    ta.Availability != TableAvailability.StatusEnum.Available
                )
                // Convert TableAvailability to Table
                .Select<TableAvailability, Table>(ta => ta.Table).ToList();

            return unavailableTables;
        }

        [Authorize(Roles = "Manager, Staff")]
        private List<Table> GetAvailableTables(DateTime? reservationDate, ReservationTime reservationTime)
        {
            // Get a list of unavailable tables
            List<Table> unavailableTables = GetUnavailableTables(reservationDate, reservationTime);

            return _context.Table.ToList().Except(unavailableTables).ToList();
        }

        [Authorize(Roles = "Manager, Staff")]
        private List<Table> GetAssignedTables(Reservation reservation)
        {
            // Get a list of tables assigned to the reservation
            return _context.TableAvailability.Where(ta => ta.ReservationId == reservation.Id)
                // Convert TableAvailability to Table
                .Select<TableAvailability, Table>(ta => ta.Table).ToList();
        }

        [Authorize(Roles = "Manager, Staff")]
        // POST: Reservations/AddTable
        [HttpPost]
        public async Task<IActionResult> AddTable(int tableId, int reservationId)
        {

            // Get reservation by ID
            Reservation reservation = await _context.Reservation.FindAsync(reservationId);

            if (reservation == null) return NotFound();

            // Get table by ID
            Table table = await _context.Table.FindAsync(tableId);

            if (table == null) return NotFound();

            // Assign table to reservation
            _context.TableAvailability.Add(new TableAvailability
            {
                ReservationId = reservationId,
                TableId = tableId,
                Availability = TableAvailability.StatusEnum.Reserved
            });
            await _context.SaveChangesAsync();

            // Redirect to reservation confirmation page
            return RedirectToAction(nameof(Confirmed), new { id = reservationId });
        }

        [Authorize(Roles = "Manager, Staff")]
        // POST: Reservations/RemoveTable
        [HttpPost]
        public async Task<IActionResult> RemoveTable(int tableId, int reservationId)
        {

            // Get reservation by ID
            Reservation reservation = await _context.Reservation.FindAsync(reservationId);

            if (reservation == null) return NotFound();

            // Get table by ID
            Table table = await _context.Table.FindAsync(tableId);

            if (table == null) return NotFound();

            // Find tableavailability
            TableAvailability tableAvailability = await _context.TableAvailability.FirstAsync(ta => ta.ReservationId == reservationId && ta.TableId == tableId);

            if (tableAvailability == null) return NotFound();

            _context.TableAvailability.Remove(tableAvailability);
            
            await _context.SaveChangesAsync();

            // Redirect to reservation confirmation page
            return RedirectToAction(nameof(Confirmed), new { id = reservationId });
        }

        [Authorize(Roles = "Manager, Staff")]
        // GET: Requested Reservations
        public async Task<IActionResult> Confirmed(int? id)
        {
            if (id == null || _context.Reservation == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservation
                .Include(r => r.ReserveTime)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reservation == null)
            {
                return NotFound();
            }

            // Get available tables
            // TODO: need to store in the view model...
            int totalAvailableTables = await GetTotalAvailableTables(reservation.ReservationDate, reservation.ReserveTime);

            var reservationVM = new ReservationViewModel
            {
                Reservation = reservation,
                TotalAvailableTables = totalAvailableTables,
                ReservationTimes = _context.ReservationTime.ToList(),
                ReservationTypes = _context.ReservationType.ToList(),
                TablesAssigned = GetAssignedTables(reservation),
                TablesAvailable = GetAvailableTables(reservation.ReservationDate, reservation.ReserveTime)
            };




            return View(reservationVM);
        }

        [Authorize(Roles = "Manager, Staff")]
        // Set the reservation Status
        public async Task<IActionResult> SetStatus(int id, Reservation.StatusEnum status)
        {
            Reservation reservation = await _context.Reservation.FindAsync(id);

            if (reservation == null) return NotFound();

            reservation.Status = status;

            _context.Reservation.Update(reservation);
            await _context.SaveChangesAsync();



            return RedirectToAction(nameof(AllRsvt));
        }


        [Authorize(Roles = "Manager, Staff")]
        // GET: All Reservations
        public async Task<IActionResult> AllRsvt()
        {
            var applicationDBContext = _context.Reservation.Include(r => r.ReserveTime);
            return View(await applicationDBContext.ToListAsync());
        }

        [Authorize(Roles = "Manager")]
        // GET: Confirmed Reservations
        public async Task<IActionResult> ConfirmedRsvt()
        {
            var applicationDBContext = _context.Reservation.Include(r => r.ReserveTime).Where(s => s.Status == Reservation.StatusEnum.Comfirmed);
            return View(await applicationDBContext.ToListAsync());
        }

        [Authorize(Roles = "Manager")]
        // GET: Requested Reservations
        public async Task<IActionResult> RequestedRsvt()
        {
            var applicationDBContext = _context.Reservation.Include(r => r.ReserveTime).Where(s => s.Status == Reservation.StatusEnum.Requested);
            return View(await applicationDBContext.ToListAsync());
        }

        [Authorize(Roles = "Manager")]
        // GET: Need Response Reservations
        public async Task<IActionResult> NeedResponseRsvt()
        {
            var day = DateTime.Today.AddDays(14);

            var applicationDBContext = _context.Reservation.Include(r => r.ReserveTime).Where(d => d.ReservationDate <= day && d.Status == Reservation.StatusEnum.Requested);
            return View(await applicationDBContext.ToListAsync());
        }

        [Authorize(Roles = "Manager")]
        // GET: Rejected Reservations
        public async Task<IActionResult> RejectedRsvt()
        {
            var applicationDBContext = _context.Reservation.Include(r => r.ReserveTime).Where(s => s.Status == Reservation.StatusEnum.Rejected);
            return View(await applicationDBContext.ToListAsync());
        }

        [Authorize(Roles = "Manager")]
        // GET: Seated Reservations
        public async Task<IActionResult> SeatedRsvt()
        {
            var applicationDBContext = _context.Reservation.Include(r => r.ReserveTime).Where(s => s.Status == Reservation.StatusEnum.Seated);
            return View(await applicationDBContext.ToListAsync());
        }

        [Authorize(Roles = "Manager")]
        // GET: Ordered Reservations
        public async Task<IActionResult> OrderedRsvt()
        {
            var applicationDBContext = _context.Reservation.Include(r => r.ReserveTime).Where(s => s.Status == Reservation.StatusEnum.Ordered);
            return View(await applicationDBContext.ToListAsync());
        }

        [Authorize(Roles = "Manager")]
        // GET: Paid Reservations
        public async Task<IActionResult> PaidRsvt()
        {
            var applicationDBContext = _context.Reservation.Include(r => r.ReserveTime).Where(s => s.Status == Reservation.StatusEnum.Paid);
            return View(await applicationDBContext.ToListAsync());
        }

        [Authorize(Roles = "Manager")]
        // GET: Completed Reservations
        public async Task<IActionResult> CompletedRsvt()
        {
            var applicationDBContext = _context.Reservation.Include(r => r.ReserveTime).Where(s => s.Status == Reservation.StatusEnum.Completed);
            return View(await applicationDBContext.ToListAsync());
        }

        [Authorize(Roles = "Member")]
        // GET: Reservation History
        public async Task<IActionResult> MyRsvt(string email)
        {
            var applicationDBContext = _context.Reservation.Include(r => r.ReserveTime).Where(s => s.Email == email);
            return View(await applicationDBContext.ToListAsync());
        }

        [Authorize]
        // GET: Reservations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Reservation == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservation
                .Include(r => r.ReserveTime)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reservation == null)
            {
                return NotFound();
            }

            return View(reservation);
        }

        // GET: Reservations/Create
        public IActionResult Create()
        {
            var model = new ReservationViewModel()
            {
                
                ReservationTimes = _context.ReservationTime.ToList(),
                ReservationTypes = _context.ReservationType.ToList()
            };
            return View(model);
            
        }

        // POST: Reservations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ReservationViewModel ReservationVM)
        {
            //// Override some properties of the model
            //ReservationVM.Reservation.ReservationMadeTime = DateTime.Now;
            //ReservationVM.Reservation.Status = Reservation.StatusEnum.Requested;

            //// Add model
            //_context.Add(ReservationVM.Reservation);

            var reservation = new Reservation
            {
                ReservationMadeTime = DateTime.Now,
                MemberId = User.Identity.Name,
                ReservationDate = ReservationVM.Reservation.ReservationDate,
                ReservationTimeId = ReservationVM.Reservation.ReservationTimeId,
                ReserveTime = ReservationVM.Reservation.ReserveTime,
                NumberOfGuest = ReservationVM.Reservation.NumberOfGuest,
                Requirement = ReservationVM.Reservation.Requirement,
                FirstName = ReservationVM.Reservation.FirstName,
                LastName = ReservationVM.Reservation.LastName,
                Email = ReservationVM.Reservation.Email,
                ContactNumber = ReservationVM.Reservation.ContactNumber,
                Status = Reservation.StatusEnum.Requested,
                RequestSource = ReservationVM.Reservation.RequestSource,
            };

            //    if (ModelState.IsValid)
            //{
            _context.Add(reservation);
            await _context.SaveChangesAsync();
            if (User.IsInRole("Manager") || User.IsInRole("Staff"))
            {
                return RedirectToAction("AllRsvt", "Reservation");
            }
            else if (User.IsInRole("Member"))
            {
                return RedirectToAction("MyRsvt", "Reservation", new {email=User.Identity.Name });
            }
            else
            {
                return RedirectToAction("GotRequest", "Home");
            };
            
            //}
            //ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "Name", food.CategoryId);
            return View(reservation);
        }

        [Authorize]
        // GET: Reservations/Create
        public IActionResult MyCreate()
        {
            var model = new ReservationViewModel()
            {

                ReservationTimes = _context.ReservationTime.ToList(),
                ReservationTypes = _context.ReservationType.ToList()
            };
            return View(model);

        }


        [Authorize]
        // POST: Reservations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MyCreate(ReservationViewModel ReservationVM)
        {
            //// Override some properties of the model
            //ReservationVM.Reservation.ReservationMadeTime = DateTime.Now;
            //ReservationVM.Reservation.Status = Reservation.StatusEnum.Requested;

            //// Add model
            //_context.Add(ReservationVM.Reservation);

            var reservation = new Reservation
            {
                ReservationMadeTime = DateTime.Now,
                MemberId = ReservationVM.Reservation.MemberId,
                ReservationDate = ReservationVM.Reservation.ReservationDate,
                ReservationTimeId = ReservationVM.Reservation.ReservationTimeId,
                ReserveTime = ReservationVM.Reservation.ReserveTime,
                NumberOfGuest = ReservationVM.Reservation.NumberOfGuest,
                Requirement = ReservationVM.Reservation.Requirement,
                FirstName = ReservationVM.Reservation.FirstName,
                LastName = ReservationVM.Reservation.LastName,
                Email = ReservationVM.Reservation.Email,
                ContactNumber = ReservationVM.Reservation.ContactNumber,
                Status = Reservation.StatusEnum.Requested,
                RequestSource = ReservationVM.Reservation.RequestSource,
            };

            //    if (ModelState.IsValid)
            //{
            _context.Add(reservation);
            await _context.SaveChangesAsync();
            return RedirectToAction("MyRsvt", new { email = @User.Identity.Name });
            //}
            //ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "Name", food.CategoryId);
            return View(reservation);
        }

        [Authorize]
        // GET: Reservations/Edit/5
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
            ViewData["ReservationTimeId"] = new SelectList(_context.ReservationTime, "Id", "Name", reservation.ReservationTimeId);
            return View(reservation);
        }

        [Authorize]
        // POST: Reservations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ReservationMadeTime,MemberId,ReservationDate,ReservationTimeId,NumberOfGuest,Requirement,FirstName,LastName,Email,ContactNumber,Status")] Reservation reservation)
        {
            if (id != reservation.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(reservation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReservationExists(reservation.Id))
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
            ViewData["ReservationTimeId"] = new SelectList(_context.ReservationTime, "Id", "Name", reservation.ReservationTimeId);
            return View(reservation);
        }


        [Authorize]
        // GET: Reservations/Delete/5
        public async Task<IActionResult> CancelMyRsvt(int? id)
        {
            if (id == null || _context.Reservation == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservation
                .Include(r => r.ReserveTime)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reservation == null)
            {
                return NotFound();
            }
            _context.Remove(reservation);
            await _context.SaveChangesAsync();
            return RedirectToAction("MyRsvt", new { email = @User.Identity.Name });

           
        }

        [Authorize]
        // POST: Reservations/Delete/5
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

        [Authorize]
        private bool ReservationExists(int id)
        {
          return _context.Reservation.Any(e => e.Id == id);
        }

        [Authorize]
        public async Task<IActionResult> Welcome()
        {
            return View();
        }
    }
}
