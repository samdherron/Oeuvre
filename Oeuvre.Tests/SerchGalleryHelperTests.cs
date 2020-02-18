using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using NUnit.Framework;
using Oeuvre.Controllers;
using Oeuvre.Helpers;
using Oeuvre.Models;
using System;
using System.Threading.Tasks;

namespace Oeuvre.Tests
{
    public class SearchGalleryHelperTests
    {
        public IConfiguration connectionString;

        private dbo_OeuvreContext _dbContext;

        public SearchGalleryHelperTests()
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
                GalleryId = "1",
                GalleryName = "Test Gallery",
                Address = "Test 1 Ave",
                City = "Test",
                PostalCode = "Test",
                Province = "Test",
                AuthUserId = "1"
            };

            _dbContext.Gallery.Add(_gl);

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
        public void Check_getGalleryItemsUsingQuickSearch_ReturnItemsTrue()
        {
   
            //Arrange
            SearchGallery userSearch = new SearchGallery();

            //Act
            var imageList = userSearch.getGalleryItemsUsingQuickSearch(_dbContext);
            
            //Assert
            Assert.AreEqual(2, imageList.Count);

        }

        [Test]
        public void Check_getGalleryItemsUsingSearchType_Return2Items()
        {

            //Arrange
            SearchGallery userSearch = new SearchGallery();

            //Act
            var imageList = userSearch.getGalleryItemsUsingSearchType(_dbContext, "1");

            //Assert
            Assert.AreEqual(2, imageList.Count);

        }

        [Test]
        public void Check_getGalleryItemsUsingSearchType_Return0Items()
        {

            //Arrange
            SearchGallery userSearch = new SearchGallery();

            //Act
            var imageList = userSearch.getGalleryItemsUsingSearchType(_dbContext, "2");

            //Assert
            Assert.AreEqual(0, imageList.Count);

        }

        [Test]
        public void Check_getGalleryItemsUsingUserInput_Return2Items()
        {

            //Arrange
            SearchGallery userSearch = new SearchGallery();

            //Act
            var imageList = userSearch.getGalleryItemsUsingUserInput(_dbContext, "Orange");

            //Assert
            Assert.AreEqual(1, imageList.Count);

        }


        [Test]
        public void Check_getGalleryItemsUsingUserInput_Return0Items()
        {

            //Arrange
            SearchGallery userSearch = new SearchGallery();

            //Act
            var imageList = userSearch.getGalleryItemsUsingUserInput(_dbContext, "Red");

            //Assert
            Assert.AreEqual(0, imageList.Count);

        }


        [Test]
        public void Check_getGalleryItemsUsingArtist_Return1Item()
        {

            //Arrange
            SearchGallery userSearch = new SearchGallery();

            //Act
            var imageList = userSearch.getGalleryItemsUsingArtist(_dbContext, "1");

            //Assert
            Assert.AreEqual(1, imageList.Count);

        }

        [Test]
        public void Check_getGalleryItemsUsingArtist_Return0Items()
        {

            //Arrange
            SearchGallery userSearch = new SearchGallery();

            //Act
            var imageList = userSearch.getGalleryItemsUsingArtist(_dbContext, "5");

            //Assert
            Assert.AreEqual(0, imageList.Count);

        }


        [Test]
        public void Check_getGalleryItemsUsingGallery_Return1Item()
        {

            //Arrange
            SearchGallery userSearch = new SearchGallery();

            //Act
            var imageList = userSearch.getGalleryItemsUsingGallery(_dbContext, "Test");

            //Assert
            Assert.AreEqual(2, imageList.Count);

        }

        [Test]
        public void Check_getGalleryItemsUsingGallery_Return0Items()
        {

            //Arrange
            SearchGallery userSearch = new SearchGallery();

            //Act
            var imageList = userSearch.getGalleryItemsUsingGallery(_dbContext, "2");

            //Assert
            Assert.AreEqual(0, imageList.Count);

        }

        [Test]
        public void Check_removeSqlInjectionParams_ReturnCleansedThemeTrue()
        {

            //Arrange
            SearchGallery userSearch = new SearchGallery();

            //Act
            string cleansedTheme = userSearch.removeSqlInjectionParams("SELECT * FROM gallery WHERE galleryid = 0");

            //Assert
            Assert.AreEqual("  from gallery where galleryid = 0", cleansedTheme);

        }


    }
}