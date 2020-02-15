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

        [HttpPost]
        public async Task<IActionResult> UploadImageToLocal(IFormFile image)
        {
            if (image != null && image.Length > 0)
            {
                var filePath = @"\Images";
                var uploadPath = _envir.WebRootPath + filePath;

                if (!Directory.Exists(uploadPath))
                {
                    Directory.CreateDirectory(uploadPath);
                }

                var newUniqueID = Guid.NewGuid().ToString();
                var updatedFileName = Path.GetFileName(newUniqueID + "." + image.FileName.Split(".")[1].ToLower());

                string fullNewPath = uploadPath + updatedFileName;

                filePath = filePath + @"\";
                var imagePath = @".." + Path.Combine(filePath, updatedFileName);

                using (var myFileStream = new FileStream(fullNewPath, FileMode.Create))
                {
                    await image.CopyToAsync(myFileStream);
                }

                ViewData["ImageLocation"] = "Image" + imagePath;

                CloudUpload_LocalDelete(updatedFileName);

            }



            return View("UploadImage");

        }


        public IActionResult CloudUpload_LocalDelete(string fileName)
        {
            string filePath = @"wwwroot\Images" + fileName;

            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(filePath)
            };

            var uploadResult = _cloudinary.Upload(uploadParams);

            return Ok();
        }

        //    var uploadParams = new ImageUploadParams()
        //{
        //    File = new FileDescription(@"C:\Users\samdh\Pictures\New folder\Anti-Grav-Single.jpg")
        //};

        //var uploadResult = _cloudinary.Upload(uploadParams);

        //return View();

    }
}