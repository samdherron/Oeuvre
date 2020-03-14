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

    }
}
