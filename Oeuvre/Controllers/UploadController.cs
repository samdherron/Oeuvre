using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Oeuvre.Models;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Oeuvre.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Oeuvre.Services;

namespace Oeuvre.Controllers
{
    public class UploadController : Controller
    {

        private readonly ILogger<UploadController> _logger;
        private dbo_OeuvreContext _context;
        IWebHostEnvironment _envir;
        private readonly UserManager<IdentityUser> _users;
        UploadService uploadService;

        public UploadController(dbo_OeuvreContext context, ILogger<UploadController> logger, IWebHostEnvironment environment, UserManager<IdentityUser> userManager)
        {
            _logger = logger;
            _context = context;
            _envir = environment;
            _users = userManager;
            uploadService = new UploadService(_context);
        }

        //Default action of returning a view
        public IActionResult UploadImage()
        {
            return View();
        }

        /// <summary>
        /// This method grabs the image from the front end,
        /// temporarily saves it locally in the wwwroot folder and gets the form information.
        /// </summary>
        /// <param name="image">The image the user chose before submitting the form.</param>
        /// <param name="form">The form the user entered data into associated with the image.</param>
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> SaveImage_GetData(IFormFile image, IFormCollection form)
        {
            var currentUser = await _users.GetUserAsync(HttpContext.User);
            string currentAuthID = currentUser.Id;
            var currentGallery = _context.Gallery.Single(g => g.AuthUserId == currentAuthID);
            string currentGalleryID = currentGallery.GalleryId.Trim();

            List<FormThemeModel> themeList = new List<FormThemeModel>();
            FormDataModel enteredForm = new FormDataModel();

            //Check for empty object
            if (image != null && image.Length > 0)
            {

                //Setting up variables and directory to save locally
                var filePath = @"\Images";
                var imageDirectory = _envir.WebRootPath + filePath;
                var uniqueID = Guid.NewGuid().ToString();
                var updatedFileName = Path.GetFileName(uniqueID + "." + image.FileName.Split(".")[1].ToLower());

                if (!Directory.Exists(imageDirectory))
                {
                    Directory.CreateDirectory(imageDirectory);
                }

                string newImagePath = imageDirectory + updatedFileName;

                filePath = filePath + @"\";

                var imagePath = @".." + Path.Combine(filePath, updatedFileName);

                //Temporarily saves the image locally within the project folder
                using (var myFileStream = new FileStream(newImagePath, FileMode.Create))
                {
                    await image.CopyToAsync(myFileStream);
                }


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



            return View("UploadImage");

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

            return Ok();
        }

    }
}