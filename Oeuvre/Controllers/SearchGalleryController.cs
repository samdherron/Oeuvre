﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Oeuvre.Models;


namespace Oeuvre.Controllers
{
    public class SearchGalleryController : Controller
    {

        private readonly ILogger<SearchGalleryController> _logger;
        private dbo_OeuvreContext _context;

        public SearchGalleryController(dbo_OeuvreContext context)
        {
            _context = context;
        }


        public IActionResult SearchGallery()
        {
            return View();
        }

        public ViewResult getList(string searchTheme, string searchType)
        {
            string sTh= searchTheme;
            int sTy = Int32.Parse(searchType);

            if(sTy == 0)
            {
                ViewData["error"] = "You have to pick a valid Theme Type!";
            }


            else
            {
                if (ModelState.IsValid)
                {

                    var artPieces = (from img in _context.Image
                             join th in _context.Theme on img.ThemeId equals th.ThemeId
                             join tt in _context.ThemeType on th.ThemeTypeId equals tt.ThemeTypeId
                             where th.ThemeName == sTh
                             select new 
                             {
                                 img.Artist
                                 ,img.Description
                                 ,img.ImgLocation

                             }).ToList();


                    List<Image> imageList = new List<Image>();

                    for (var i = 0; i < artPieces.Count; i++)
                    {
                        Image newImg = new Image();

                        newImg.ImgLocation = artPieces[i].ImgLocation;
                        newImg.Description = artPieces[i].Description;
                        newImg.Artist = artPieces[i].Artist;

                        imageList.Add(newImg);
                        //string imgLocation = artPieces[i].ImgLocation;
                        //Console.WriteLine("THIS IS THE IMG LOCATION " + imgLocation + "FOR ART PIECE" + (i + 1));
                    }


                    ViewData["Pieces"] = imageList;

                }
            }
            //if (ModelState.IsValid)
            //{
            //    //var Galleries = _context.Gallery.Where(p => p.GalleryId == "1");
               

            //    //The below code uses a join and a where clause to get the theme name from the Theme table
            //    /* var q = (from pd in _context.Theme
            //              join od in _context.ThemeType on pd.ThemeTypeId equals od.ThemeTypeId
            //              where od.ThemeTypeName == "Color"
            //              select new
            //              {
            //                  pd.ThemeName
            //              }).ToList();*/

            //    var Galleries = _context.Gallery.ToList();

            //    List<Gallery> galleryList = new List<Gallery>();

            //    foreach (Gallery g in Galleries)
            //    {
            //        Gallery myGallery = new Gallery();

            //        myGallery.GalleryId = g.GalleryId;
            //        myGallery.GalleryName = g.GalleryName;
            //        myGallery.Province = g.Province;

            //        galleryList.Add(myGallery);
            //    }

            //    ViewData["Galleries"] = galleryList;
            //    //return RedirectToAction("Index");
            //    return View("Index");
            //}
            //else
                return View("SearchGallery");


        }

        public ViewResult getQuickSearchList()
        {
            //string sTh = searchTheme;
            //int sTy = Int32.Parse(searchType);

            //if (sTy == 0)
            //{
            //    ViewData["error"] = "You have to pick a valid Theme Type!";
            //}


          
            if (ModelState.IsValid)
            {


                var artPieces = (from img in _context.Image
                                 orderby img.DateUploaded
                                 select new
                                 {
                                     img.Artist
                                 ,
                                     img.Description
                                 ,
                                     img.ImgLocation

                                 }).ToList();


              
                List<Image> imageList = new List<Image>();

                for (var i = 0; i < artPieces.Count; i++)
                {
                    Image newImg = new Image();

                    newImg.ImgLocation = artPieces[i].ImgLocation;
                    newImg.Description = artPieces[i].Description;
                    newImg.Artist = artPieces[i].Artist;

                    imageList.Add(newImg);

                }



                ViewData["Pieces"] = imageList;

            }
            

            return View("SearchGallery");


        }
    }
}