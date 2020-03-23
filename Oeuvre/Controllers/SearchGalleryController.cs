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

        private string themeNumber = "";

        public SearchGalleryController(dbo_OeuvreContext context)
        {
            _context = context;
        }


        public IActionResult SearchGallery()
        {
            return View();
        }

        public void setThemeType(string id)
        {
            Console.WriteLine("The old Theme number is: " + themeNumber);
            themeNumber = id;

            Console.WriteLine("The new Theme number is: " + themeNumber);
        }


        public ViewResult getQuickSearchList(string searchBarInput)
        {
            userSearch = new SearchGallery();
            if (ModelState.IsValid)
            {

                searchBarInput = userSearch.removeSqlInjectionParams(searchBarInput);

                if (searchBarInput == null || searchBarInput == "")
                {
                    //userSearch = new SearchGallery();

                    List<CollectionsImg> imageList = userSearch.getGalleryItemsUsingQuickSearch(_context);

                    ViewData["Pieces"] = imageList;
                }
                else
                {
                    //userSearch = new SearchGallery();

                    List<CollectionsImg> imageList = userSearch.getGalleryItemsUsingUserInput(_context, searchBarInput);

                    if (imageList.Count == 0)
                    {
                        ViewData["Count"] = "No Art Found";
                        ViewData["Input"] = searchBarInput;
                    }

                    ViewData["Pieces"] = imageList;
                }


              

            }


            return View("SearchGallery");


        }

        public ViewResult getList(string searchTheme, string searchType)
        {
            userSearch = new SearchGallery();

            string sTh;

            if (searchTheme != null && searchTheme != "")
            {
                sTh = userSearch.removeSqlInjectionParams(searchTheme);
            }
            else
            {
                sTh = "";
            }

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

                    List<CollectionsImg> imageList = userSearch.getGalleryItemsUsingGallery(_context, sTh);

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

                    List<CollectionsImg> imageList = userSearch.getGalleryItemsUsingArtist(_context, sTh);

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

                    List<CollectionsImg> imageList = userSearch.getGalleryItemsUsingSearchType(_context, searchType);
 
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

                    List<CollectionsImg> imageList = userSearch.getGalleryItemsUsingUserInput(_context, searchTheme);


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

        //public ViewResult getQuickSearchList()
        //{
          
        //    if (ModelState.IsValid)
        //    {

        //        userSearch = new SearchGallery();

        //        List<CollectionsImg> imageList = userSearch.getGalleryItemsUsingQuickSearch(_context);

        //        ViewData["Pieces"] = imageList;

        //    }
            

        //    return View("SearchGallery");


        //}

        public ViewResult testImgId(int id)
        {

            Console.WriteLine("THE ART ID IS " + id);
            return View("SearchGallery");

        }
    }
}