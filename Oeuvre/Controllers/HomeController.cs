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
using Oeuvre.Services;

namespace Oeuvre.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private dbo_OeuvreContext _context;
        Account _account;
        Cloudinary _cloudinary;
        HomeDataService _dataService;

        public HomeController(ILogger<HomeController> logger, dbo_OeuvreContext context)
        {
            _account = new Account(
                "oeuvre",
                "591857667739764",
                "tVwlzrfSYIFs8gxwIKnMW_OfRd0");

            _cloudinary = new Cloudinary(_account);
            _logger = logger;
            _context = context;

            _dataService = new HomeDataService(_context);

        }

        public IActionResult Index()
        {
            OrchestratePageData();
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public async void OrchestratePageData()
        {
            await GetGalleryInfo();
            await GetExhibitionInfo();
            await GetThemeInfo();
            
        }

        /// <summary>
        /// Calls Multiple Methods inside HomeDataService to get all gallery images and info
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetGalleryInfo()
        {

            List<GalleryDisplay> galleryList = _dataService.RetrieveGalleryInfo();


            //Setting up ViewData for frontend
            if (galleryList.Count == 3)
            {
                for (int i = 0; i < galleryList.Count; i++)
                {
                    string indexKey = "galleryName" + (i + 1);
                    ViewData[indexKey] = galleryList.ElementAt(i).GalleryName;
                }

            }

            List<string> imageURLS = _dataService.RetrieveGalleryImages(galleryList);

            if (imageURLS.Count == 3)
            {
                for (int i = 0; i < galleryList.Count; i++)
                {
                    string indexKey = "galleryImage" + (i + 1);
                    ViewData[indexKey] = imageURLS.ElementAt(i);
                }
            }

            return Ok();
            
        }

        /// <summary>
        /// Calls Multiple Methods inside HomeDataService to get all exhibition images and info
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetExhibitionInfo()
        {

            Tuple<string, int> galleryInfo = _dataService.RetrieveGalleryName_ExhibitionCards();

            if (galleryInfo != null)
            {

                string galleryName = galleryInfo.Item1;
                int galleryID = galleryInfo.Item2;

                ViewData["exhibitionCard_galleryName"] = galleryName;

                List<Image> images = _dataService.RetrieveImages_ThemeCards(galleryID);


                List<string> imageCollectionTypes = _dataService.RetrieveCollectionTypes_ExhibitionCards(images);


                //Setting up ViewData for frontend
                for (int i = 0; i < images.Count; i++)
                {
                    string indexKey = "exhibitionsCard" + (i + 1) + "_SRC";
                    ViewData[indexKey] = images.ElementAt(i).ImgLocation;

                    indexKey = "exhibitionsCard" + (i + 1) + "_CollectionType";
                    ViewData[indexKey] = imageCollectionTypes.ElementAt(i);



                    indexKey = "exhibitionsCard" + (i + 1) + "_Description";

                    //Will cap the description at 30 characters and throw an ellipsis at the end.
                    if (images.ElementAt(i).Description.Length > 30)
                    {
                        string descriptionCut = images.ElementAt(i).Description.Substring(0, 30);
                        descriptionCut += "...";
                        ViewData[indexKey] = descriptionCut;
                    }

                    else
                    {
                        ViewData[indexKey] = images.ElementAt(i).Description;


                    }

                }

            }


            return Ok();

        }

        /// <summary>
        /// Calls Multiple Methods inside HomeDataService to get all theme images and info
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetThemeInfo()
        {

            Tuple<string, int> galleryInfo = _dataService.RetrieveGalleryName_ThemeCards();

            if (galleryInfo != null)
            {

                string galleryName = galleryInfo.Item1;
                int galleryID = galleryInfo.Item2;

                ViewData["themeCards_galleryName"] = galleryName;

                List<Image> images = _dataService.RetrieveImages_ThemeCards(galleryID);


                List<string> imageThemeValues = _dataService.RetrieveThemeValues_ThemeCards(images);


                //Setting up ViewData for frontend
                for (int i = 0; i < images.Count; i++)
                {
                    string indexKey = "themeCards_Gallery" + (i + 1) + "SRC";
                    ViewData[indexKey] = images.ElementAt(i).ImgLocation;

                    indexKey = "themeCards_Gallery" + (i + 1) + "Theme";
                    ViewData[indexKey] = imageThemeValues.ElementAt(i);



                    indexKey = "themeCards_Gallery" + (i + 1) + "Description";

                    //Will cap the description at 60 characters and throw an ellipsis at the end.
                    if (images.ElementAt(i).Description.Length > 60)
                    {
                        string descriptionCut = images.ElementAt(i).Description.Substring(0, 60);
                        descriptionCut += "...";
                        ViewData[indexKey] = descriptionCut;
                    }

                    else
                    {
                        ViewData[indexKey] = images.ElementAt(i).Description;


                    }

                }

            }
           

            return Ok();

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
