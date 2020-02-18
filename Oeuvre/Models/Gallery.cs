using System;
using System.Collections.Generic;

namespace Oeuvre.Models
{
    public partial class Gallery
    {
        public Gallery()
        {
            Image = new HashSet<Image>();
        }

        public string GalleryId { get; set; }
        public string GalleryName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string Province { get; set; }
        public string AuthUserId { get; set; }

        public virtual ICollection<Image> Image { get; set; }
    }
}
