﻿using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        public enum themeTypeValues
        {
            Colour = 1,
            Age = 2,
            Artist = 3,
            ArtType = 4,
            Genre = 5,
            Other = 6,
            Gallery = 7
        }

        public async Task<IActionResult> UploadCloud_DeleteLocal(string fileName, FormDataModel enteredForm, string currentGalleryID)
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
            string imageURL = uploadResult.SecureUri.ToString();

            //Deletes file from local project folder
            File.Delete(filePath);

            //Saves all image & theme information into database
            await SaveDatabase(enteredForm, currentGalleryID, imageURL);

            return null;
        }

        public async Task<IActionResult> SaveDatabase(FormDataModel enteredForm, string galleryID, string imageURL)
        {

            string currentType = "";
            string currentValue = "";

            Image newDatabaseEntry = new Image();


            var number = (from i in _context.Image
                          orderby i.ImgId + 0
                          select i.ImgId).ToList();

            List<int> numberList = number.Select(s => int.Parse(s)).ToList();

            int highestNumber = numberList.Max() + 1;

           
            newDatabaseEntry.ImgId = highestNumber.ToString();
            newDatabaseEntry.GalleryId = galleryID;
            newDatabaseEntry.DateUploaded = DateTime.UtcNow;
            newDatabaseEntry.Description = enteredForm.ImageDescription;
            newDatabaseEntry.ImgLocation = imageURL;
            newDatabaseEntry.Artist = enteredForm.ArtistName;
            newDatabaseEntry.Name = enteredForm.ImageName;
            newDatabaseEntry.ThemeId = "1";
            

            //Save new entry to Image table
            _context.Image.Add(newDatabaseEntry);
            await _context.SaveChangesAsync();

            //Save new entry to ThemeLookup table
            ThemeLookup newThemeLookupEntry = new ThemeLookup();
            newThemeLookupEntry.ImgId = newDatabaseEntry.ImgId;

            _context.ThemeLookup.Add(newThemeLookupEntry);
            await _context.SaveChangesAsync();

            //Returns newly created theme lookup ID from table
            var themeLookupID = newThemeLookupEntry.ThemeLookupId;


            for (int i = 0; i < enteredForm.Themes.Count; i++)
            {
                currentType = enteredForm.Themes.ElementAt(i).ThemeType;
                currentValue = enteredForm.Themes.ElementAt(i).ThemeValue;

                //Check for null or empty strings inside of both values
                if ((!string.IsNullOrEmpty(currentType)) && (!string.IsNullOrEmpty(currentValue)))
                {

                    int ThemeTypeID = (int)((themeTypeValues)Enum.Parse(typeof(themeTypeValues), currentType));

                    var valueExists = (from theme in _context.Theme
                                          
                                          where theme.ThemeName.Contains(currentValue) && theme.ThemeTypeId == ThemeTypeID.ToString()
                                          select new
                                          {
                                              theme.ThemeName,
                                              theme.ThemeId

                                          }).ToList();

                    //Check if the call returned with a valid type match
                    if (valueExists.Count > 0)
                    {
                        string oldThemeValue = valueExists.ElementAt(0).ThemeId.Trim();
                        await saveImgTheme(oldThemeValue, themeLookupID);

                    }

                    //If no match, add to theme table &
                    else
                    {
                        Theme newThemeEntry = new Theme();

                        number = (from a in _context.Theme
                                      orderby a.ThemeId + 0
                                      select a.ThemeId).ToList();

                        numberList = number.Select(s => int.Parse(s)).ToList();

                        highestNumber = numberList.Max() + 1;

                        newThemeEntry.ThemeId = highestNumber.ToString();
                        newThemeEntry.ThemeTypeId = ThemeTypeID.ToString();
                        newThemeEntry.ThemeName = currentValue;

                        _context.Theme.Add(newThemeEntry);
                        await _context.SaveChangesAsync();


                        string newThemeValue = newThemeEntry.ThemeId;
                        await saveImgTheme(newThemeValue, themeLookupID);

                    }
                }

            }

            return null;
        }


        public async Task<bool> saveImgTheme(string ThemeId, int themeLookupID)
        {
            bool retVal = true;

            try
            {
                ImgThemes newImgTheme = new ImgThemes();
                newImgTheme.ThemeId = ThemeId;
                newImgTheme.ThemeLookupId = themeLookupID;

                _context.ImgThemes.Add(newImgTheme);
                await _context.SaveChangesAsync();

            }
            catch
            {
                retVal = false;

            }

            return retVal;
        }

    }
}
