using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Oeuvre.Models
{
    public class GalleryRequestModel
    {


        public string galleryEmail { get; set; }


        public string galleryName { get; set; }


        public string galleryType { get; set; }

        public string phoneNumber { get; set; }


        public string address { get; set; }

        public string province { get; set; }

        public string city { get; set; }


        public string postalCode { get; set; }

        public string galleryDesc { get; set; }
    }
}
