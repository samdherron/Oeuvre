using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Oeuvre.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Oeuvre.Areas.Identity.Pages.Account.Manage
{
    public class ArtworkModel : PageModel
    {

        private dbo_OeuvreContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        public ArtworkModel(dbo_OeuvreContext context, UserManager<IdentityUser> userManager) {
            _context = context;
            _userManager = userManager;
        }


        public List<Image> galleryArtPieces { get; set;}


        [BindProperty]
        public ArtModel Art { get; set; }

        public class ArtModel
        {
            public List<Image> galleryCollection;            
            public string PhoneNumber { get; set; }
        }



        private async Task LoadAsync(IdentityUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);

            var userId = user.Id;


            var Gallery = (from gl in _context.Gallery
                           where gl.AuthUserId == user.Id
                           select new
                           {
                               gl.GalleryId
                           }).Distinct().FirstOrDefault();

            int galId = Gallery.GalleryId;



            var artPieces = (from img in _context.Image
                             join gl in _context.Gallery on img.GalleryId equals gl.GalleryId
                             where gl.GalleryId == galId
                             select new
                             {
                                 img.ImgId
                                 ,
                                 img.Artist
                                 ,
                                 img.Description
                                 ,
                                 img.Name
                                 ,
                                 img.ImgLocation
                             }).Distinct().ToList(); 

            List<Image> newImageList = new List<Image>();

            for(var i = 0; i < artPieces.Count; i++)
            {
                Image newImage = new Image();

                newImage.Name = artPieces[i].Name;
                newImage.ImgId = artPieces[i].ImgId;
                newImage.Description = artPieces[i].Description;
                newImage.Artist = artPieces[i].Artist;
                newImage.ImgLocation = artPieces[i].ImgLocation;

                newImageList.Add(newImage);
            }


            Art = new ArtModel
            {
                galleryCollection = newImageList
               
            };


            galleryArtPieces = newImageList;

            ViewData["galleryImages"] = newImageList;

        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var userId = user.Id;

            //step 1 - get artpiece id

            Image artPieces = (from img in _context.Image
                             where img.ImgId == id.ToString()
                             select img).SingleOrDefault();

            string artPieceId = artPieces.ImgId;


            //step 2 - get theme lookup id

            ThemeLookup themeLookup = (from tl in _context.ThemeLookup
                                  where tl.ImgId == artPieceId
                                  select tl).SingleOrDefault();

            int themeLoopupId = themeLookup.ThemeLookupId;



            //step 3 delete all imgthemes that have a the same themelookupid
            _context.ImgThemes.RemoveRange(_context.ImgThemes.Where(x => x.ThemeLookupId == themeLoopupId));
            _context.SaveChanges();

            //step 4 delete the themelookup
            _context.ThemeLookup.Remove(themeLookup);
            _context.SaveChanges();

            //step 5 delete the img
            _context.Image.Remove(artPieces);
            _context.SaveChanges();


            await LoadAsync(user);

            return Page();
        }


    }
}
