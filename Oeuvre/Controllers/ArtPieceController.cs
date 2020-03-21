using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Oeuvre.Models;

namespace Oeuvre.Controllers
{
    public class ArtPieceController : Controller
    {
        private readonly dbo_OeuvreContext _context;


        public ArtPieceController(dbo_OeuvreContext context)
        {
            _context = context;
        }

        // GET: ArtPiece
        public async Task<IActionResult> Index(string imageID)
        {

            var image = await _context.Image.Where(i => i.ImgId == imageID.Trim()).ToListAsync();
            ViewData["Image"] = image;

            Image returnImage = fillImageObject(image);

            return View(returnImage);
        }

        public Image fillImageObject(List<Image> image)
        {
            Image imageData = new Image();

            if (image != null && image.Count > 0)
            {
                imageData.Artist = image.ElementAt(0).Artist;
                imageData.CollectionType = image.ElementAt(0).CollectionType;
                imageData.CuratorName = image.ElementAt(0).CuratorName;
                imageData.DateUploaded = image.ElementAt(0).DateUploaded;
                imageData.Description = image.ElementAt(0).Description;
                imageData.Gallery = image.ElementAt(0).Gallery;
                imageData.ImgId = image.ElementAt(0).ImgId;
                imageData.ImgLocation = image.ElementAt(0).ImgLocation;
                imageData.Medium = image.ElementAt(0).Medium;
                imageData.Name = image.ElementAt(0).Name;
                imageData.PieceDimensions = image.ElementAt(0).PieceDimensions;
                imageData.Theme = image.ElementAt(0).Theme;
             
            }

            return imageData;
        }


    }

}
