﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
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

               

        public EditGalleryArtPiece(dbo_OeuvreContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
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

        private async Task<IActionResult> OnPostAsync(IFormCollection form)
        {
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);
            string currentAuthID = currentUser.Id;
            var currentGallery = _context.Gallery.Single(g => g.AuthUserId == currentAuthID);
            string currentGalleryID = currentGallery.GalleryId.ToString();

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

                }
                catch
                {

                }


            return Page();



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
                                       }).Distinct().ToList();

            Image newImage = new Image();
            newImage.ImgId = artPieceInformation[0].ImgId;
            newImage.Artist = artPieceInformation[0].Artist;
            newImage.Description = artPieceInformation[0].Description;
            newImage.Name = artPieceInformation[0].Name;
            newImage.ImgLocation = artPieceInformation[0].ImgLocation;
            newImage.CuratorName = artPieceInformation[0].CuratorName;
            newImage.YearCreated = artPieceInformation[0].YearCreated;
            newImage.Medium = artPieceInformation[0].Medium;
            newImage.CollectionType = artPieceInformation[0].CollectionType;


            newImage.CollectionType = artPieceInformation[0].CollectionType;
            newImage.PieceDimensions = artPieceInformation[0].PieceDimensions;

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
