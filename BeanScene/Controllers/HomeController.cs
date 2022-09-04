using BeanScene.Extensions;
using BeanScene.Models;
using BeanScene.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using BeanScene.Areas.Identity.Data;

namespace BeanSceneWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly ApplicationDBContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDBContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult AllFood(string search)
        {
            var thumbnails = new List<ThumbnailModel>().GetFoodThumbnail(_context, search);
            var count = thumbnails.Count();
            var model = new List<ThumbnailboxViewModel>();
            //for (int i = 0; i < count; i++)
            //{
                model.Add(new ThumbnailboxViewModel
                {
                    Thumbnails = thumbnails
                });
            //};


            //var thumbnails = new List<ThumbnailModel>().GetFoodThumbnail(_context, search);
            //var count = thumbnails.Count() / 4;
            //var model = new List<ThumbnailboxViewModel>();
            //for (int i = 0; i <= count; i++)
            //{
            //    model.Add(new ThumbnailboxViewModel
            //    {
            //        Thumbnails = thumbnails.Skip(i * 4).Take(4)
            //    });
            //};

            return View(model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}