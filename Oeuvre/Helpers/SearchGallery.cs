using Oeuvre.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Oeuvre.Helpers
{
    public class SearchGallery
    {

        public List<Image> getGalleryItemsUsingSearchType(dbo_OeuvreContext _context, string searchType)
        {
            int sTy = Int32.Parse(searchType);

            var artPieces = (from img in _context.Image
                             join tl in _context.ThemeLookup on img.ImgId equals tl.ImgId
                             join ith in _context.ImgThemes on tl.ThemeLookupId equals ith.ThemeLookupId
                             join th in _context.Theme on ith.ThemeId equals th.ThemeId
                             join tt in _context.ThemeType on th.ThemeTypeId equals tt.ThemeTypeId
                             where tt.ThemeTypeId == searchType
                             select new
                             {
                                 img.Artist
                                 ,
                                 img.Description
                                 ,
                                 img.ImgLocation

                             }).Distinct().ToList();


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

            return imageList;
        }

        public List<Image> getGalleryItemsUsingUserInput(dbo_OeuvreContext _context, string searchTheme)
        {
            string sTh = searchTheme;

            var artPieces = (from img in _context.Image
                             join tl in _context.ThemeLookup on img.ImgId equals tl.ImgId
                             join ith in _context.ImgThemes on tl.ThemeLookupId equals ith.ThemeLookupId
                             join th in _context.Theme on ith.ThemeId equals th.ThemeId
                             where th.ThemeName.Contains(sTh)
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
                //string imgLocation = artPieces[i].ImgLocation;
                //Console.WriteLine("THIS IS THE IMG LOCATION " + imgLocation + "FOR ART PIECE" + (i + 1));
            }

            return imageList;
        }

        public List<Image> getGalleryItemsUsingQuickSearch(dbo_OeuvreContext _context)
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

            return imageList;
        }

    }

    
}
