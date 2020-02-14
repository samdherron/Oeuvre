using System;
using System.Collections.Generic;

namespace Oeuvre.Models
{
    public partial class Theme
    {
        public Theme()
        {
            Image = new HashSet<Image>();
            ImgThemes = new HashSet<ImgThemes>();
        }

        public string ThemeId { get; set; }
        public string ThemeTypeId { get; set; }
        public string ThemeName { get; set; }

        public virtual ThemeType ThemeType { get; set; }
        public virtual ICollection<Image> Image { get; set; }
        public virtual ICollection<ImgThemes> ImgThemes { get; set; }
    }
}
