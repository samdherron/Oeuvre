using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using Oeuvre.Models;
using Oeuvre.Helpers;
using Oeuvre.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Oeuvre.Tests
{
     
    class GalleryViewTests

    {
        public IConfiguration connectionString;

        private dbo_OeuvreContext _dbContext;

        public GalleryViewTests()
        {
            var options = new DbContextOptionsBuilder<dbo_OeuvreContext>()
              .UseInMemoryDatabase(Guid.NewGuid().ToString())
              .EnableSensitiveDataLogging()
              .Options;
            _dbContext = new dbo_OeuvreContext(options);
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

                _dbContext.ThemeType.Add(_tt);
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
                _dbContext.Theme.Add(_th);
            }

            Gallery _gl = new Gallery()
            {
                GalleryId = 1,
                GalleryName = "Test Gallery",
                Address = "Test 1 Ave",
                City = "Test",
                PostalCode = "Test",
                Province = "Test",
                AuthUserId = "1"
            };

            _dbContext.Gallery.Add(_gl);

            string[] imgId = { "1", "2" };
            int[] galleryId = { 1, 1 };
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

                _dbContext.Image.Add(_img);
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

                _dbContext.ThemeLookup.Add(_tl);
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

                _dbContext.ImgThemes.Add(_it);
            }


            _dbContext.SaveChanges();

        }

        [Test]
        public void CheckGalleryRetrival()
        {

            //Arrange
            GalleryShowArt galleryItems = new GalleryShowArt();

            //Act
            var imageList = galleryItems.GetGalleryImages(_dbContext,"1");

            //Assert
            Assert.AreEqual(2, imageList.Count);

        }




    }
}
