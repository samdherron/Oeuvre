using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Oeuvre.Models
{
    public class CollectionsImg
    {

        public string ImgId { get; set; }
        public string GalleryId { get; set; }
        public DateTime DateUploaded { get; set; }
        public string ThemeId { get; set; }
        public string Description { get; set; }
        public string ImgLocation { get; set; }
        public string Artist { get; set; }
        public string Name { get; set; }

        public string GalleryName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string Province { get; set; }
        public string AuthUserId { get; set; }

    }
}
