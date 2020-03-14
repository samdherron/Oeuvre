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

            var artPieces = (from img in _context.Image
                             join gl in _context.Gallery on img.GalleryId equals gl.GalleryId
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



    }
}
