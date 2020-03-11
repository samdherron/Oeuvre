using System;
using System.Collections.Generic;

namespace Oeuvre.Models
{
    public partial class Image
    {
        public Image()
        {
            ThemeLookup = new HashSet<ThemeLookup>();
        }

        public string ImgId { get; set; }
        public string GalleryId { get; set; }
        public DateTime DateUploaded { get; set; }
        public string ThemeId { get; set; }
        public string Description { get; set; }
        public string ImgLocation { get; set; }
        public string Artist { get; set; }
        public string Name { get; set; }
        public string CuratorName { get; set; }
        public string YearCreated { get; set; }
        public string Medium { get; set; }
        public string CollectionType { get; set; }
        public string PieceDimensions { get; set; }

        public virtual Gallery Gallery { get; set; }
        public virtual Theme Theme { get; set; }
        public virtual ICollection<ThemeLookup> ThemeLookup { get; set; }
    }
}
