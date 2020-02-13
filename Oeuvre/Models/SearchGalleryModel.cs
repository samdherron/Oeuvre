using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Oeuvre.Models
{
    public class SearchGalleryModel
    {

        public SearchModel Search { get; set; }

        public string userInput { get; set; }
        public class SearchModel { public string UserInput { get; set; } }

    }
}
