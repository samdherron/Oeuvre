using System;
using System.Collections.Generic;

namespace Oeuvre.Models
{
    public partial class GalleryAuth
    {
        public GalleryAuth()
        {
            Gallery = new HashSet<Gallery>();
        }

        public string GalleryAuthId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public bool Active { get; set; }

        public virtual ICollection<Gallery> Gallery { get; set; }
    }
}
