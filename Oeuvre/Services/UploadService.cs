using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Oeuvre.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Oeuvre.Services
{
    public class UploadService
    {

        Account _account;
        Cloudinary _cloudinary;
        private dbo_OeuvreContext _context;

        public UploadService(dbo_OeuvreContext context)
        {
            _account = new Account(
                "oeuvre",
                "591857667739764",
                "tVwlzrfSYIFs8gxwIKnMW_OfRd0");

            _cloudinary = new Cloudinary(_account);

            _context = context;
        }

        public async Task<IActionResult> UploadCloud_DeleteLocal(string fileName, FormDataModel enteredForm, string currentUserID)
        {

            //Setup upload requirements
            string filePath = @"wwwroot\Images" + fileName;

            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(filePath)
            };

            //Uploads file to Cloudinary Repository
            var uploadResult = _cloudinary.Upload(uploadParams);

            //Gets newly uploaded image URL to save into database
            string imageURL = uploadResult.Uri.ToString();

            //Deletes file from local project folder
            File.Delete(filePath);

            //Saves all image & theme information into database
            await SaveDatabase(enteredForm, currentUserID, imageURL);

            return null;
        }

        public async Task<IActionResult> SaveDatabase(FormDataModel enteredForm, string galleryID, string imageURL)
        {

            string currentType = "";
            string currentValue = "";

            for (int i = 0; i < enteredForm.Themes.Count; i++)
            {
                currentType = enteredForm.Themes.ElementAt(i).ThemeType;
                currentValue = enteredForm.Themes.ElementAt(i).ThemeValue;

                //Check for null or empty strings inside of both values
                if ((!string.IsNullOrEmpty(currentType)) && (!string.IsNullOrEmpty(currentValue)))
                {

                         var typeExists = (from theme in _context.ThemeType
                                          
                                          where theme.ThemeTypeName.Contains(currentType)
                                          select new
                                          {
                                              theme.ThemeTypeName

                                          }).ToString();

                    //Check if the call returned with a valid type match
                    if (typeExists != null)
                    {
                        Console.WriteLine("theme type exists");
                    }
                }

            }

            return null;
        }

    }
}
