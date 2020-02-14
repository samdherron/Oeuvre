using System;
using System.Collections.Generic;

namespace Oeuvre.Models
{
    public partial class ImgThemes
    {
        public int Id { get; set; }
        public int ThemeLookupId { get; set; }
        public string ThemeId { get; set; }

        public virtual Theme Theme { get; set; }
        public virtual ThemeLookup ThemeLookup { get; set; }
    }
}
