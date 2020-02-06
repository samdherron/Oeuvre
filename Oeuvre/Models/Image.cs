using System;
using System.Collections.Generic;

namespace Oeuvre.Models
{
    public partial class Image
    {
        public string ImgId { get; set; }
        public string GalleryId { get; set; }
        public DateTime DateUploaded { get; set; }
        public string ThemeId { get; set; }
        public string Description { get; set; }
        public string ImgLocation { get; set; }

        public virtual Gallery Gallery { get; set; }
        public virtual Theme Theme { get; set; }
    }
}
