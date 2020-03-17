using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Oeuvre.Models;

namespace Oeuvre.Services
{
    public class HomeDataService
    {

        private dbo_OeuvreContext _context;


        public HomeDataService(dbo_OeuvreContext context)
        {

            _context = context;

        }

        public List<GalleryDisplay> RetrieveGalleryInfo()
        {
            List<GalleryDisplay> homepageGalleries = new List<GalleryDisplay>();


            var threeGalleries = (from g in _context.Gallery
                                  orderby g.GalleryId
                                  select g).Take(3).ToList();

            

            for (int i = 0; i < threeGalleries.Count; i++)
            {
                GalleryDisplay currentGallery = new GalleryDisplay();
                currentGallery.GalleryName = threeGalleries.ElementAt(i).GalleryName;
                currentGallery.GalleryId = threeGalleries.ElementAt(i).GalleryId.ToString();
                homepageGalleries.Add(currentGallery);
            }


            return homepageGalleries;
        }

        public List<string> RetrieveGalleryImages(List<GalleryDisplay> galleryNames)
        {
            List<string> imageURLS = new List<string>();


            for (int i = 0; i < galleryNames.Count; i++)
            {
                var galleryRandomImage = (from b in _context.Image
                                          orderby b.ImgId
                                          where b.GalleryId == int.Parse(galleryNames.ElementAt(i).GalleryId)
                                          select b.ImgLocation).Take(1).ToList();
                imageURLS.Add(galleryRandomImage.FirstOrDefault());

            }


            return imageURLS;
        }

        public Tuple<string, int> RetrieveGalleryName_ThemeCards()
        {

            string galleryName;
            int galleryID;
            var randomGallery = (from g in _context.Gallery
                                 where g.GalleryId == 1
                                 select g).ToList();


            galleryName = randomGallery[0].GalleryName;
            galleryID = randomGallery[0].GalleryId;

            var galleryTuple = new Tuple<string, int>(galleryName, galleryID);

            return galleryTuple;
        }


        public List<Image> RetrieveImages_ThemeCards(int galleryID)
        {

            List<Image> imageList = new List<Image>();
           


                var images = (from i in _context.Image
                              where i.GalleryId == galleryID
                              select i).Take(3).ToList();

                imageList = images;
            

            return imageList;
        }

        public List<string> RetrieveThemeValues_ThemeCards(List<Image> images)
        {
            List<string> themeIDs = new List<string>();
            List<string> themeValues = new List<string>();


            foreach(Image i in images)
            {
                themeIDs.Add(i.ThemeId.Trim());
            }


            for (int i = 0; i < themeIDs.Count; i++)
            {
                var themeValue = (from t in _context.Theme
                                  where t.ThemeId == themeIDs.ElementAt(i)
                                  select t.ThemeName);

                themeValues.Add(themeValue.First().Trim());
            }

            return themeValues;

        }

    }
}
