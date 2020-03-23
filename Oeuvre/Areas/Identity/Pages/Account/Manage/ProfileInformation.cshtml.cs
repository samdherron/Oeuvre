using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Oeuvre.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

        //[BindProperty]
        //public updateProfileModel updateProfile { get; set; }

        public class updateProfileModel
        {

            [Required]
            [DataType(DataType.Text)]
            [Display(Name = "Gallery Name")]
            public string galleryName { get; set; }

            [Required]
            [DataType(DataType.Text)]
            [Display(Name = "Gallery Type")]
            public string galleryType { get; set; }

            [Required]
            [DataType(DataType.PhoneNumber)]
            [Display(Name = "Phone Number")]
            public string phoneNumber { get; set; }

            [Required]
            [DataType(DataType.Text)]
            [Display(Name = "Address")]
            public string address { get; set; }

            [Required]
            [DataType(DataType.PostalCode)]
            [Display(Name = "Postal Code")]
            public string postalCode { get; set; }

            [Required]
            [DataType(DataType.Text)]
            [Display(Name = "City")]
            public string city { get; set; }

            [Required]
            [DataType(DataType.Text)]
            [Display(Name = "Province")]
            public string province { get; set; }

            [Required]
            [DataType(DataType.Text)]
            [Display(Name = "Gallery Description")]
            public string galleryDescription { get; set; }

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
                                          ,
                                          gl.Type
                                          ,
                                          gl.PhoneNumber

                                      }).Distinct().ToList();

            Gallery galInf = new Gallery();
            galInf.GalleryName = galleryInformation[0].GalleryName;
            galInf.Address = galleryInformation[0].Address;
            galInf.City = galleryInformation[0].City;
            galInf.PostalCode = galleryInformation[0].PostalCode;
            galInf.Province = galleryInformation[0].Province;
            galInf.GalleryDescription = galleryInformation[0].GalleryDescription;
            galInf.Type = galleryInformation[0].Type;
            galInf.PhoneNumber = galleryInformation[0].PhoneNumber;

            profile = new ProfileModel
            {
                galleryInformation = galInf
            };

 
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

        public async Task<IActionResult> OnPostAsync(updateProfileModel updateProfile)
        {

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var userName = await _userManager.GetUserNameAsync(user);

            var userId = user.Id;

            galleryEmail = user.Email;

            Gallery galInf = (from gl in _context.Gallery
                                      where gl.AuthUserId == userId
                                      select gl
                                    ).SingleOrDefault();

            galInf.GalleryName = updateProfile.galleryName;
            galInf.Address = updateProfile.address;
            galInf.City = updateProfile.city;
            galInf.PostalCode = updateProfile.postalCode;
            galInf.Province = updateProfile.province;
            galInf.GalleryDescription = updateProfile.galleryDescription;
            galInf.Type = updateProfile.galleryType;
            galInf.PhoneNumber = updateProfile.phoneNumber;


            _context.SaveChanges();
            await LoadAsync(user);


            return Page();

        }


    }
}
