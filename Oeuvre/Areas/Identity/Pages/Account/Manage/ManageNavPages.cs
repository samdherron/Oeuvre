﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Oeuvre.Areas.Identity.Pages.Account.Manage
{
    public static class ManageNavPages
    {

        public static string EditGalleryArtPiece => "EditGalleryArtPiece";
        public static string Artwork => "Artwork";
        public static string addWork => "addWork";
        public static string ProfileInformation => "ProfileInformation";
        public static string Index => "Index";

        public static string Email => "Email";

        public static string ChangePassword => "ChangePassword";

        public static string ExternalLogins => "ExternalLogins";

        public static string PersonalData => "PersonalData";

        public static string TwoFactorAuthentication => "TwoFactorAuthentication";

        public static string EditGalleryArtPieceNavClass(ViewContext viewContext) => PageNavClass(viewContext, EditGalleryArtPiece);

        public static string ArtworkNavClass(ViewContext viewContext) => PageNavClass(viewContext, Artwork);
        public static string addWorkNavClass(ViewContext viewContext) => PageNavClass(viewContext, addWork);

        public static string ProfileInformationNavClass(ViewContext viewContext) => PageNavClass(viewContext, ProfileInformation);

        public static string IndexNavClass(ViewContext viewContext) => PageNavClass(viewContext, Index);

        public static string EmailNavClass(ViewContext viewContext) => PageNavClass(viewContext, Email);

        public static string ChangePasswordNavClass(ViewContext viewContext) => PageNavClass(viewContext, ChangePassword);

        public static string ExternalLoginsNavClass(ViewContext viewContext) => PageNavClass(viewContext, ExternalLogins);

        public static string PersonalDataNavClass(ViewContext viewContext) => PageNavClass(viewContext, PersonalData);

        public static string TwoFactorAuthenticationNavClass(ViewContext viewContext) => PageNavClass(viewContext, TwoFactorAuthentication);

        private static string PageNavClass(ViewContext viewContext, string page)
        {
            var activePage = viewContext.ViewData["ActivePage"] as string
                ?? System.IO.Path.GetFileNameWithoutExtension(viewContext.ActionDescriptor.DisplayName);
            return string.Equals(activePage, page, StringComparison.OrdinalIgnoreCase) ? "active" : null;
        }
    }
}
