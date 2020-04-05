using System;
using System.Collections.Generic;

namespace Oeuvre.Models
{
    public partial class GalleryListing
    {
        //public List<GalleryDisplay> galleryDisplays { get; set; }
       public GalleryListing()
        {
            Image = new HashSet<Image>();
        }

        public List<string> GalleryId { get; set; }
        public List<string> GalleryName { get; set; }
        public List<string> Address { get; set; }
        public List<string> City { get; set; }
        public List<string> PostalCode { get; set; }
        public List<string> Province { get; set; }
        public List<string> GalleryDescription { get; set; }
        public List<List<Image>> Images { get; set; }
        public List<string> AuthUserId { get; set; }
        public string full { get; set; }

        public virtual ICollection<Image> Image { get; set; }
    }
}
