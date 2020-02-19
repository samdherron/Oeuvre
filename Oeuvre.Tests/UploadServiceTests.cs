using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using Oeuvre.Models;
using Oeuvre.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Oeuvre.Tests
{

    class UploadServiceTests
    {


        public IConfiguration connectionString;

        private dbo_OeuvreContext _context;
        IWebHostEnvironment _envir;
        UploadService service;


        public UploadServiceTests()
        {
            var options = new DbContextOptionsBuilder<dbo_OeuvreContext>()
              .UseInMemoryDatabase(Guid.NewGuid().ToString())
              .EnableSensitiveDataLogging()
              .Options;
            _context = new dbo_OeuvreContext(options);
            service = new UploadService(_context);
        }

        [OneTimeSetUp]
        public void Prepare()
        {
            string[] type_id = { "1", "2", "3", "4" };
            string[] type_name = { "Color", "Other", "Age", "Genre" };

            for (var i = 0; i < 4; i++)
            {
                ThemeType _tt = new ThemeType()
                {
                    ThemeTypeId = type_id[i],
                    ThemeTypeName = type_name[i]
                };

                _context.ThemeType.Add(_tt);
            }

            string[] themeId = { "1", "2" };
            string[] themeTypeId = { "1", "1" };
            string[] themeName = { "Orange", "Blue" };

            for (var i = 0; i < 2; i++)
            {
                Theme _th = new Theme()
                {
                    ThemeId = themeId[i],
                    ThemeTypeId = themeTypeId[i],
                    ThemeName = themeName[i]
                };
                _context.Theme.Add(_th);
            }

            Gallery _gl = new Gallery()
            {
                GalleryId = "1",
                GalleryName = "Test Gallery",
                Address = "Test 1 Ave",
                City = "Test",
                PostalCode = "Test",
                Province = "Test",
                AuthUserId = "1"
            };

            _context.Gallery.Add(_gl);

            string[] imgId = { "1", "2" };
            string[] galleryId = { "1", "1" };
            //string[] dateUploaded = { DateTime.Today.ToString(), DateTime.Today.ToString()};
            string[] imageThemeId = { "1", "1" };
            string[] description = { "Testing", "Testing" };
            string[] imgLocation = { "Testing", "Testing" };
            string[] artist = { "A_1", "A_2" };
            string[] name = { "Paint 1", "Paint 2" };

            for (var i = 0; i < 2; i++)
            {
                Image _img = new Image()
                {
                    ImgId = imgId[i],
                    GalleryId = galleryId[i],
                    DateUploaded = DateTime.Today,
                    ThemeId = imageThemeId[i],
                    Description = description[i],
                    ImgLocation = imgLocation[i],
                    Artist = artist[i],
                    Name = name[i]
                };

                _context.Image.Add(_img);
            }


            int[] themeLookupId = { 1, 2 };
            string[] themelookupImgId = { "1", "2" };

            for (var i = 0; i < 2; i++)
            {
                ThemeLookup _tl = new ThemeLookup()
                {
                    ThemeLookupId = themeLookupId[i],
                    ImgId = themelookupImgId[i]
                };

                _context.ThemeLookup.Add(_tl);
            }

            int[] imgThemeLookupId = { 1, 2, 2 };
            string[] imgThemeId = { "1", "2", "1" };

            for (var i = 0; i < 2; i++)
            {
                ImgThemes _it = new ImgThemes()
                {
                    ThemeLookupId = themeLookupId[i],
                    ThemeId = imgThemeId[i]
                };

                _context.ImgThemes.Add(_it);
            }


            _context.SaveChanges();

            

        }


        [Test]
        public void Check_SaveDatabase()
        {
            FormDataModel testForm = new FormDataModel();

            List<FormThemeModel> themes = new List<FormThemeModel>();
            FormThemeModel themeEntry = new FormThemeModel();
            themeEntry.ThemeType = "Colour";
            themeEntry.ThemeValue = "Red";
            themes.Add(themeEntry);

            testForm.ArtistName = "TestName";
            testForm.ImageName = "TestImage";
            testForm.ImageDescription = "TestDescription";
            testForm.Themes = themes;

            //Arrange

            string databaseSavedResult = asyncCallHelper(testForm).Result;
            

            //Assert
            Assert.AreEqual("True", databaseSavedResult);


        }

        public async Task<string> async_SaveDatabaseCall(FormDataModel testForm)
        {
            //Act
            bool databaseSaved = await service.SaveDatabase(testForm, "2", "https://res.cloudinary.com/oeuvre/image/upload/v1582071344/bnhqgpmxc5fuztm88ajj.jpg");
            string result = databaseSaved.ToString();

            return result;
        }

    }
}
