using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Oeuvre.Controllers;
using Oeuvre.Models;
using Oeuvre.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Oeuvre.Areas.Identity.Pages.Account.Manage
{
    public class addWorkModel : PageModel
    {
        private readonly ILogger<UploadController> _logger;
        private dbo_OeuvreContext _context;
        IWebHostEnvironment _envir;
        private readonly UserManager<IdentityUser> _users;
        UploadService uploadService;

        public addWorkModel(dbo_OeuvreContext context, ILogger<UploadController> logger, IWebHostEnvironment environment, UserManager<IdentityUser> userManager)
        {
            _logger = logger;
            _context = context;
            _envir = environment;
            _users = userManager;
            uploadService = new UploadService(_context);

        }
        
        public async Task<IActionResult> OnPostAsync(IFormFile image, IFormCollection form)
        {
            var currentUser = await _users.GetUserAsync(HttpContext.User);
            string currentAuthID = currentUser.Id;
            var currentGallery = _context.Gallery.Single(g => g.AuthUserId == currentAuthID);
            string currentGalleryID = currentGallery.GalleryId.ToString();

            List<FormThemeModel> themeList = new List<FormThemeModel>();
            FormDataModel enteredForm = new FormDataModel();

            //Check for empty object
            if (image != null && image.Length > 0)
            {

                string updatedFileName = await uploadService.SaveLocal(image, _envir);

                //Begin form data processing
                try
                {
                    enteredForm.ImageName = form.ElementAt(0).Value.ToString();
                    enteredForm.ArtistName = form.ElementAt(1).Value.ToString();
                    enteredForm.ImageDescription = form.ElementAt(2).Value.ToString();

                    //Splits all of the theme types and theme values into two seperate string arrays
                    string[] themeTypeSplit = Request.Form.ElementAt(3).Value.ToString().Split(',');
                    string[] themeValueSplit = Request.Form.ElementAt(4).Value.ToString().Split(',');
                    int numberThemes = themeTypeSplit.Length;

                    //Generates a List of FormThemeModel objects
                    //Each list entry will have a FormThemeModel with a ThemeType and a ThemeValue
                    for (int i = 0; i < numberThemes; i++)
                    {
                        FormThemeModel themes = new FormThemeModel();
                        themes.ThemeType = themeTypeSplit[i];
                        themes.ThemeValue = themeValueSplit[i];
                        themeList.Add(themes);
                    }

                    enteredForm.Themes = themeList;

                }
                catch
                {

                }

                //Next controller method to upload and save to DB
                await ProcessImage(updatedFileName, image, enteredForm, form, currentGalleryID);

            }



            return Page();

        }


        /// <summary>
        /// This method will upload the image to the Cloudinary repository and then delete it from local storage.
        /// </summary>
        public async Task<IActionResult> ProcessImage(string fileName, IFormFile image, FormDataModel formData, IFormCollection form, string galleryID)
        {

            //Uploads to cloud and gets the uploaded imageURL
            string imageURL = uploadService.UploadCloud_DeleteLocal(fileName);

            //Saves all image & theme information into database
            await uploadService.SaveDatabase(formData, galleryID, imageURL);

            return Page();
        }
    }

    
}
