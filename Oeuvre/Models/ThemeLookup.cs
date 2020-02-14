using System;
using System.Collections.Generic;

namespace Oeuvre.Models
{
    public partial class ThemeLookup
    {
        public ThemeLookup()
        {
            ImgThemes = new HashSet<ImgThemes>();
        }

        public int ThemeLookupId { get; set; }
        public string ImgId { get; set; }

        public virtual Image Img { get; set; }
        public virtual ICollection<ImgThemes> ImgThemes { get; set; }
    }
}
