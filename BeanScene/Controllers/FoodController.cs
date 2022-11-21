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
    public class FoodController : Controller
    {
        private readonly ApplicationDBContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public FoodController(ApplicationDBContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Food
        public async Task<IActionResult> Index()
        {
            var applicationDBContext = _context.Food.Include(f => f.Catagory);
            return View(await applicationDBContext.ToListAsync());
        }

        // GET: Food/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Food == null)
            {
                return NotFound();
            }

            var food = await _context.Food
                .Include(f => f.Catagory)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (food == null)
            {
                return NotFound();
            }
            var model = new FoodViewModel()
            {
                Food = food,
                Categories = _context.Category.ToList()
            };

            return View(model);
        }

        // GET: Food/Create
        public IActionResult Create()
        {
            var model = new FoodViewModel()
            {
                Categories = _context.Category.ToList()
            };

            return View(model);
        }

        // POST: Food/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Create(FoodViewModel foodVM)
        {
                var food = new Food
                {
                    Name = foodVM.Food.Name,
                    Description = foodVM.Food.Description,
                    Price = foodVM.Food.Price,
                    ImageURL = foodVM.Food.ImageURL,
                    Catagory = foodVM.Food.Catagory,
                    CategoryId = foodVM.Food.CategoryId,
                };
                
            //    if (ModelState.IsValid)
            //{
                _context.Add(food);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            //}
            //ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "Name", food.CategoryId);
            return View(food);
        }

        // GET: Food/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Food == null)
            {
                return NotFound();
            }

            var food = await _context.Food.FindAsync(id);
            if (food == null)
            {
                return NotFound();
            }
            var model = new FoodViewModel()
            {
                Food = food,
                Categories = _context.Category.ToList()
            };
            return View(model);
        }

        // POST: Food/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(FoodViewModel foodVM)
        {
            //if (id != food.Id)
            //{
            //    return NotFound();
            //}

            //if (ModelState.IsValid)
            //{
            //try
            //{
            //    _context.Update(food);
            //    await _context.SaveChangesAsync();
            //}
            //catch (DbUpdateConcurrencyException)
            //{
            //    if (!FoodExists(food.Id))
            //    {
            //        return NotFound();
            //    }
            //    else
            //    {
            //        throw;
            //    }
            ////}
            //return RedirectToAction(nameof(Index));
            foodVM.Categories = _context.Category.ToList();
            _context.Update(foodVM.Food);
            await _context.SaveChangesAsync();
            return RedirectToAction("AllFood", "Home");

            //}
            //ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "Name", food.CategoryId);
            //return View(food);
        }

        // GET: Food/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Food == null)
            {
                return NotFound();
            }

            var food = await _context.Food
                .Include(f => f.Catagory)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (food == null)
            {
                return NotFound();
            }
            var model = new FoodViewModel()
            {
                Food = food,
                Categories = _context.Category.ToList()
            };

            return View(model);
        }

        // POST: Food/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Food == null)
            {
                return Problem("Entity set 'ApplicationDBContext.Food'  is null.");
            }
            var food = await _context.Food.FindAsync(id);
            if (food != null)
            {
                _context.Food.Remove(food);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FoodExists(int id)
        {
          return (_context.Food?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        // POST: Book/ImageUpload
        // Handle image upload for book
        [HttpPost, ActionName("ImageUpload")]
        public async Task<IActionResult> ImageUpload(IFormFile file)  
        {
            // Image save folder within wwwroot
            string imageFolder = "images/food";

            // Create the folder if it doesn't exist
            string dirPath = Path.Combine(_webHostEnvironment.WebRootPath, imageFolder);
            if (!Directory.Exists(dirPath)) Directory.CreateDirectory(dirPath);

            // Create image file (handle the upload)
            string filePath = Path.Combine(dirPath, file.FileName);
            using (FileStream fs = System.IO.File.Create(filePath))
            {
                file.CopyTo(fs);
            }

            // Return 200 + image path
            return StatusCode(200, $"/{imageFolder}/{file.FileName}");

        }
    }
}
