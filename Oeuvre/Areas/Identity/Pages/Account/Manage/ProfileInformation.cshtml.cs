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
    public class ProfileInformationModel : PageModel
    {

        private dbo_OeuvreContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public ProfileInformationModel(dbo_OeuvreContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [BindProperty]
        public ProfileModel profile { get; set; }

        public class ProfileModel
        {
            public Gallery galleryInformation;
        }

        public string galleryEmail { get; set; }

        private async Task LoadAsync(IdentityUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);

            var userId = user.Id;

            galleryEmail = user.Email;

            var galleryInformation = (from gl in _context.Gallery
                                      where gl.AuthUserId == userId
                                      select new
                                      {
                                          gl.GalleryName
                                          ,
                                          gl.Address
                                          ,
                                          gl.City
                                          ,
                                          gl.PostalCode
                                          ,
                                          gl.Province
                                          ,
                                          gl.GalleryDescription
                                          
                                      }).Distinct().ToList();

            Gallery galInf = new Gallery();
            galInf.GalleryName = galleryInformation[0].GalleryName;
            galInf.Address = galleryInformation[0].Address;
            galInf.City = galleryInformation[0].City;
            galInf.PostalCode = galleryInformation[0].PostalCode;
            galInf.Province = galleryInformation[0].Province;
            galInf.GalleryDescription = galleryInformation[0].GalleryDescription;


            profile = new ProfileModel
            {
                galleryInformation = galInf
            };

           
            //for (var i = 0; i < artPieces.Count; i++)
            //{
            //    Image newImage = new Image();

            //    newImage.Name = artPieces[i].Name;
            //    newImage.ImgId = artPieces[i].ImgId;
            //    newImage.Description = artPieces[i].Description;
            //    newImage.Artist = artPieces[i].Artist;
            //    newImage.ImgLocation = artPieces[i].ImgLocation;

            //    newImageList.Add(newImage);
            //}


            //Art = new ArtModel
            //{
            //    galleryCollection = newImageList

            //};


            //galleryArtPieces = newImageList;

            //ViewData["galleryImages"] = newImageList;

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
