using System;
using Oeuvre.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Oeuvre.Helpers
{
    public class GalleryShowArt
    {

        public List<Image> GetGalleryImages(dbo_OeuvreContext _dbContext, string id) {
            List<Image> myList = new List<Image>();

            

            var images = (from image in _dbContext.Image
                          where image.GalleryId == int.Parse(id)
                          select new
                          {
                              image.ImgId,
                              image.ImgLocation,
                              image.GalleryId,
                              image.ThemeId,
                              image.DateUploaded,
                              image.Artist,
                              image.Description,
                              image.Name

                          }).ToList();

            for (int i = 0; i < images.Count; i++)
            {
                Image tempImage = new Image();
                tempImage.ImgLocation = images.ElementAt(i).ImgLocation;
                myList.Add(tempImage);
            }
            return (myList);
        }

}
}
