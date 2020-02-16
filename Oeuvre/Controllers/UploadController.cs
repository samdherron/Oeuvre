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

namespace Oeuvre.Controllers
{
    public class UploadController : Controller
    {

        private readonly ILogger<UploadController> _logger;
        private dbo_OeuvreContext _context;
        Account _account;
        Cloudinary _cloudinary;
        IWebHostEnvironment _envir;

        public UploadController(dbo_OeuvreContext context, ILogger<UploadController> logger, IWebHostEnvironment environment)
        {
            _account = new Account(
                "oeuvre",
                "591857667739764",
                "tVwlzrfSYIFs8gxwIKnMW_OfRd0");

            _cloudinary = new Cloudinary(_account);

            _logger = logger;
            _context = context;
            _envir = environment;
        }

        public IActionResult UploadImage()
        {
            return View();
        }


        /// <summary>
        /// This method grabs the image from the front end
        /// and temporarily saves it locally in the wwwroot folder.
        /// </summary>
        /// <param name="image">The image the user chose before submitting the form.</param>
        [HttpPost]
        public async Task<IActionResult> UploadImageToLocal(IFormFile image, string[] formData)
        {

            //Check for empty object
            if (image != null && image.Length > 0)
            {
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


                await CloudUpload_LocalDelete(updatedFileName, formData);

            }

            return View("UploadImage");

        }


        /// <summary>
        /// This method will upload the image to the Cloudinary repository and then delete it from local storage.
        /// </summary>
        public async Task<IActionResult> CloudUpload_LocalDelete(string fileName, string[] formData)
        {
            string filePath = @"wwwroot\Images" + fileName;

            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(filePath)
            };

            var uploadResult = _cloudinary.Upload(uploadParams);

            await SaveInfo_ToDatabase(uploadResult, formData);

            System.IO.File.Delete(filePath);

            return Ok();
        }

        public async Task<IActionResult> SaveInfo_ToDatabase(ImageUploadResult uploadResult, string[] formData)
        {

            Image newImage = new Image();
            

            return Ok();
        }

    }
}