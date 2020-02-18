using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Oeuvre.Models;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;


namespace Oeuvre.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private dbo_OeuvreContext _context;
        Account _account;
        Cloudinary _cloudinary;

        public HomeController(ILogger<HomeController> logger, dbo_OeuvreContext context)
        {
            _account = new Account(
                "oeuvre",
                "591857667739764",
                "tVwlzrfSYIFs8gxwIKnMW_OfRd0");

            _cloudinary = new Cloudinary(_account);
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

        //public ViewResult getList()
        //{
        //    if (ModelState.IsValid)
        //    {
        //        //var Galleries = _context.Gallery.Where(p => p.GalleryId == "1");

        //        var Galleries = _context.Gallery.ToList();               

        //        List<Gallery> galleryList = new List<Gallery>();

        //        foreach(Gallery g in Galleries)
        //        {
        //            Gallery myGallery = new Gallery();

        //            myGallery.GalleryId = g.GalleryId;
        //            myGallery.GalleryName = g.GalleryName;
        //            myGallery.Province = g.Province;

        //            galleryList.Add(myGallery);
        //        }

        //        ViewData["Galleries"] = galleryList;
        //        //return RedirectToAction("Index");
        //        return View("Index");
        //    }
        //    else
        //        return View();


        //}

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
