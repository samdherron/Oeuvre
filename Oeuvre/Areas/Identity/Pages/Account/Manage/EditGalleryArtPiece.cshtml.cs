using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Oeuvre.Helpers;
using Oeuvre.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Oeuvre.Areas.Identity.Pages.Account.Manage
{
    public class EditGalleryArtPiece : PageModel
    {
        private dbo_OeuvreContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private string imageID = "";
        private string imageURL = "";
        private Gallery gallery;
        private int galleryID = 0;
        SearchGallery securityClass;
               

        public EditGalleryArtPiece(dbo_OeuvreContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
            securityClass = new SearchGallery();
        }

        


        public enum themeTypeValues
        {
            Mediums = 1,
            Movements = 2,
            Locations = 3,
            Colour = 4,
            Period = 5,
            Other = 6
        }

        [BindProperty]
        public ArtPieceGalleryInfoModel ArtPieceInfo { get; set; }
        public class ArtPieceGalleryInfoModel
        {
            public Image imageDetails;
        }


        [BindProperty]
        public ArtPieceGalleryThemeModel ArtPieceGalleryTheme { get; set; }
        public class ArtPieceGalleryThemeModel
        {
            public string imgThemeName { get; set;}
            public string imgThemeId { get; set; }
            public string imgthemeTypeName { get; set; }
            public string imgThemeTypeId { get; set; }
        }

        [BindProperty]
        public ListArtPieceGalleryThemeModel ListArtPieceTheme { get; set; }

        public class ListArtPieceGalleryThemeModel
        {
            public List<ArtPieceGalleryThemeModel> ArtPieceThemes;
        }

        public async Task<IActionResult> OnPostAsync(IFormCollection form, string imgID)
        {
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);
            string currentAuthID = currentUser.Id;
            var currentGallery = _context.Gallery.Single(g => g.AuthUserId == currentAuthID);
            string currentGalleryID = currentGallery.GalleryId.ToString();
            Image updatedImage = new Image();

            List<FormThemeModel> themeList = new List<FormThemeModel>();
            FormDataModel enteredForm = new FormDataModel();


                //Begin form data processing
                try
                {
                    enteredForm.ImageName = form.ElementAt(0).Value.ToString();
                    enteredForm.ArtistName = form.ElementAt(1).Value.ToString();
                    enteredForm.CuratorName = form.ElementAt(2).Value.ToString();
                    enteredForm.YearCreated = form.ElementAt(3).Value.ToString();
                    enteredForm.Medium = form.ElementAt(4).Value.ToString();
                    enteredForm.CollectionType = form.ElementAt(5).Value.ToString();
                    enteredForm.PieceDimensions = form.ElementAt(6).Value.ToString();
                    enteredForm.ImageDescription = form.ElementAt(7).Value.ToString();

                    //Splits all of the theme types and theme values into two seperate string arrays
                    string[] themeTypeSplit = Request.Form.ElementAt(8).Value.ToString().Split(',');
                    string[] themeValueSplit = Request.Form.ElementAt(9).Value.ToString().Split(',');
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

                    await updateDatabase(enteredForm, imgID);

                 }
                catch(Exception e)
                {
                    Console.WriteLine("YOU HAVE HIT AN ERROR");
                    Console.WriteLine(e.ToString());
                }

            var user = await _userManager.GetUserAsync(User);
            var userId = user.Id;
            await LoadAsync(user, imgID);

            return Page();



        }

        public async Task updateDatabase(FormDataModel enteredForm, string imgID)
        {
          

            bool successfullySaved = false;
            string currentType = "";
            string currentValue = "";

            //var oldImageInfo = (from img in _context.Image
            //                    where img.ImgId == imageID
            //                    select new
            //                    {
            //                        img.ImgLocation,
            //                        img.DateUploaded
            //                    }).ToList();

            //Image updatedImage = new Image();
            //updatedImage.Artist = enteredForm.ArtistName;
            //updatedImage.CollectionType = enteredForm.CollectionType;
            //updatedImage.CuratorName = enteredForm.CuratorName;
            //string dateString = oldImageInfo[0].ToString();
            //updatedImage.DateUploaded = DateTime.Parse(dateString);
            //updatedImage.Description = enteredForm.ImageDescription;
            //updatedImage.Gallery = gallery;
            //updatedImage.GalleryId = galleryID;
            //updatedImage.ImgId = imageID;
            //updatedImage.ImgLocation = imageURL;
            //updatedImage.Medium = enteredForm.Medium;
            //updatedImage.Name = enteredForm.ImageName;
            //updatedImage.PieceDimensions = enteredForm.PieceDimensions;
            //updatedImage.YearCreated = enteredForm.YearCreated;
            //updatedImage.ThemeId = "1";

            //_context.Image.Add(updatedImage);


            //step 1 - update image information
            Image oldImageInfo = (from img in _context.Image
                                where img.ImgId == imgID
                                  select img).SingleOrDefault();



            oldImageInfo.Artist = enteredForm.ArtistName;
            oldImageInfo.CollectionType = enteredForm.CollectionType;
            oldImageInfo.CuratorName = enteredForm.CuratorName;
            //string dateString = oldImageInfo.DateUploaded.ToString();
            //oldImageInfo.DateUploaded = DateTime.Parse(dateString);
            oldImageInfo.Description = enteredForm.ImageDescription;
            //oldImageInfo.Gallery = gallery;
            //oldImageInfo.GalleryId = galleryID;
            //oldImageInfo.ImgId = imageID;
            //oldImageInfo.ImgLocation = imageURL;
            oldImageInfo.Medium = enteredForm.Medium;
            oldImageInfo.Name = enteredForm.ImageName;
            oldImageInfo.PieceDimensions = enteredForm.PieceDimensions;
            oldImageInfo.YearCreated = enteredForm.YearCreated;
            oldImageInfo.ThemeId = "1";

            _context.SaveChanges();


            //step 2 - get theme lookup id

            ThemeLookup themeLookup = (from tl in _context.ThemeLookup
                                       where tl.ImgId == imgID
                                       select tl).SingleOrDefault();

            int themeLookupId = themeLookup.ThemeLookupId;

            //step 3 delete all imgthemes that have a the same themelookupid
            _context.ImgThemes.RemoveRange(_context.ImgThemes.Where(x => x.ThemeLookupId == themeLookupId));
            _context.SaveChanges();

            //step 4 add themes to image

            var number = (from i in _context.Image
                          orderby i.ImgId + 0
                          select i.ImgId).ToList();

            List<int> numberList = new List<int>();

            int highestNumber = 0;

            if (number.Count == 0)
            {
                highestNumber = 1;
            }
            else
            {
                numberList = number.Select(s => int.Parse(s)).ToList();
                highestNumber = numberList.Max() + 1;
            }



            for (int i = 0; i < enteredForm.Themes.Count; i++)
            {
                currentType = enteredForm.Themes.ElementAt(i).ThemeType;
                currentValue = securityClass.removeSqlInjectionParams(enteredForm.Themes.ElementAt(i).ThemeValue);

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
                        await saveImgTheme(oldThemeValue, themeLookupId);

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
                        await saveImgTheme(newThemeValue, themeLookupId);

                        successfullySaved = true;

                    }
                }

            }


            //try
            //{
            //    await _context.SaveChangesAsync();
            //}
            //catch (Exception e)
            //{
            //    Console.WriteLine("THIS IS A SQL ERROR");
            //    Console.WriteLine(e);
            //}


            ////Save new entry to ThemeLookup table
            //ThemeLookup newThemeLookupEntry = new ThemeLookup();
            //newThemeLookupEntry.ImgId = oldImageInfo.ImgId;

            //_context.ThemeLookup.Add(newThemeLookupEntry);
            //await _context.SaveChangesAsync();

            /*
                //Returns newly created theme lookup ID from table
                var themeLookupID = newThemeLookupEntry.ThemeLookupId;


                for (int i = 0; i < enteredForm.Themes.Count; i++)
                {
                    currentType = enteredForm.Themes.ElementAt(i).ThemeType;
                    currentValue = securityClass.removeSqlInjectionParams(enteredForm.Themes.ElementAt(i).ThemeValue);

                    //Check for null or empty strings inside of both values
                    if ((!string.IsNullOrEmpty(currentType)) && (!string.IsNullOrEmpty(currentValue)))
                    {

                        int ThemeTypeID = (int)((themeTypeValues)Enum.Parse(typeof(themeTypeValues), currentType));

                        var  = (from theme in _context.Theme

                                           where theme.ThemeName.Contains(currentValue) && theme.ThemeTypeId == ThemeTypeID.ToString()
                                           select new
                                           {
                                               theme.ThemeName,
                                               theme.ThemeId
                                           }).ToList();



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

                            successfullySaved = true;

                        }
                    }



                }

           */
            //await LoadAsync(user, imageID);

            //return Page();
            //await LoadAsync(_userManager.GetUserAsync(), 
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


private async Task LoadAsync(IdentityUser user, string id)
        {
            var userName = await _userManager.GetUserNameAsync(user);

            var userId = user.Id;


            var artPieceInformation = (from img in _context.Image
                                       where img.ImgId == id
                                       select new
                                       {
                                           img.ImgId
                                            ,
                                           img.Artist
                                             ,
                                           img.Description
                                             ,
                                           img.Name
                                             ,
                                           img.ImgLocation
                                           ,
                                           img.CuratorName
                                           ,
                                           img.YearCreated
                                           ,
                                           img.Medium
                                           ,
                                           img.CollectionType
                                           ,
                                           img.PieceDimensions
                                           ,
                                           img.Gallery,
                                           img.GalleryId
                                       }).Distinct().ToList();

            Image newImage = new Image();
            newImage.ImgId = artPieceInformation[0].ImgId.Trim();
            newImage.Artist = artPieceInformation[0].Artist.Trim();
            newImage.Description = artPieceInformation[0].Description.Trim();
            newImage.Name = artPieceInformation[0].Name.Trim();
            newImage.ImgLocation = artPieceInformation[0].ImgLocation.Trim();
            newImage.CuratorName = artPieceInformation[0].CuratorName.Trim();
            newImage.YearCreated = artPieceInformation[0].YearCreated.Trim();
            newImage.Medium = artPieceInformation[0].Medium.Trim();
            newImage.CollectionType = artPieceInformation[0].CollectionType.Trim();

            imageID = artPieceInformation[0].ImgId.Trim();
            imageURL = artPieceInformation[0].ImgLocation.Trim();
            gallery = artPieceInformation[0].Gallery;
            galleryID = artPieceInformation[0].GalleryId;


            newImage.CollectionType = artPieceInformation[0].CollectionType.Trim();
            newImage.PieceDimensions = artPieceInformation[0].PieceDimensions.Trim();

            ArtPieceInfo = new ArtPieceGalleryInfoModel
            {
                imageDetails = newImage
            };

            id = id.Replace(" ", "");

            var artPieceThemes = (from img in _context.Image
                                  join tl in _context.ThemeLookup on img.ImgId equals tl.ImgId
                                  join imgt in _context.ImgThemes on tl.ThemeLookupId equals imgt.ThemeLookupId
                                  join t in _context.Theme on imgt.ThemeId equals t.ThemeId
                                  join tt in _context.ThemeType on t.ThemeTypeId equals tt.ThemeTypeId
                                  where img.ImgId == id
                                  select new
                                      {
                                        t.ThemeName
                                        ,
                                        t.ThemeId
                                        ,
                                        tt.ThemeTypeName
                                        ,
                                        tt.ThemeTypeId
                                      }
                                  ).Distinct().ToList();

            

            List<ArtPieceGalleryThemeModel> themes = new List<ArtPieceGalleryThemeModel>();

            for (var i = 0; i < artPieceThemes.Count; i++) {


                var themeName = cleanWords(artPieceThemes[i].ThemeName);
                var themeId = cleanWords(artPieceThemes[i].ThemeName);
                var themeTypeName = cleanWords(artPieceThemes[i].ThemeTypeName);
                var themeTypeId = cleanWords(artPieceThemes[i].ThemeTypeId);

                ArtPieceGalleryTheme = new ArtPieceGalleryThemeModel();
                ArtPieceGalleryTheme.imgThemeName = themeName;
                ArtPieceGalleryTheme.imgThemeId = themeId;
                ArtPieceGalleryTheme.imgthemeTypeName = themeTypeName;
                ArtPieceGalleryTheme.imgThemeTypeId = themeTypeId;

                themes.Add(ArtPieceGalleryTheme);

            }

            ListArtPieceTheme = new ListArtPieceGalleryThemeModel
            {
                ArtPieceThemes = themes
            };
                     

        }

       


        public string cleanWords(string word)
        {

            word = word.Trim();
            char[] letters = word.ToCharArray();
            string firstLetter = letters[0].ToString();

            word = word.Remove(0, 1);
            word = word.Insert(0, firstLetter.ToUpperInvariant());

            //RegexOptions options = RegexOptions.None;
            //Regex regex = new Regex("[ ]{2,}", options);
            //word = regex.Replace(word, " ");

            return word;
        }

        public async Task<IActionResult> OnGetAsync(string id)
        {


            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user, id);
            return Page();
        }

    }
}
