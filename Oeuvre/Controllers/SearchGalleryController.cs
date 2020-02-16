using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Oeuvre.Helpers;
using Oeuvre.Models;


namespace Oeuvre.Controllers
{
    public class SearchGalleryController : Controller
    {

        private readonly ILogger<SearchGalleryController> _logger;
        private dbo_OeuvreContext _context;
        private SearchGallery userSearch;

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
            userSearch = new SearchGallery();

            string sTh= userSearch.removeSqlInjectionParams(searchTheme);

            int sTy = Int32.Parse(searchType);


            if(sTy == 0 && (sTh == null || sTh == ""))
            {
                ViewData["error"] = "You have to pick either Theme Type or put something in the search param!";
            }
            else if(sTy == 7 && sTh != "")
            {
                if (ModelState.IsValid)
                {

                    userSearch = new SearchGallery();

                    List<Image> imageList = userSearch.getGalleryItemsUsingGallery(_context, sTh);

                    if (imageList.Count == 0)
                    {
                        ViewData["Count"] = "No Art Found";
                        ViewData["Input"] = sTh;
                    }


                    ViewData["Pieces"] = imageList;

                }
            }
            else if(sTy == 3 && sTh != ""){
                if (ModelState.IsValid)
                {

                    userSearch = new SearchGallery();

                    List<Image> imageList = userSearch.getGalleryItemsUsingArtist(_context, sTh);

                    if (imageList.Count == 0)
                    {
                        ViewData["Count"] = "No Art Found";
                        ViewData["Input"] = sTh;
                    }


                    ViewData["Pieces"] = imageList;

                }
            }

            else if(sTh == "" || sTh == null)
            {
                if (ModelState.IsValid)
                {

                    userSearch = new SearchGallery();

                    List<Image> imageList = userSearch.getGalleryItemsUsingSearchType(_context, searchType);
 
                    if (imageList.Count == 0)
                    {
                        ViewData["Count"] = "No Art Found";
                        ViewData["Input"] = sTh;
                    }


                    ViewData["Pieces"] = imageList;

                }
            }


            else
            {
                if (ModelState.IsValid)
                {

                    userSearch = new SearchGallery();

                    List<Image> imageList = userSearch.getGalleryItemsUsingUserInput(_context, searchTheme);


                    if (imageList.Count == 0)
                    {
                        ViewData["Count"] = "No Art Found";
                        ViewData["Input"] = sTh;
                    }

                    ViewData["Pieces"] = imageList;

                }
            }

                return View("SearchGallery");


        }

        public ViewResult getQuickSearchList()
        {
          
            if (ModelState.IsValid)
            {

                userSearch = new SearchGallery();

                List<Image> imageList = userSearch.getGalleryItemsUsingQuickSearch(_context);

                ViewData["Pieces"] = imageList;

            }
            

            return View("SearchGallery");


        }

        public ViewResult testImgId(int id)
        {

            Console.WriteLine("THE ART ID IS " + id);
            return View("SearchGallery");

        }
    }
}