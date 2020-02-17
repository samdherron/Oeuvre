using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Oeuvre.Models
{
    public class FormDataModel
    {

        public string ImageName { get; set; }
        public string ArtistName { get; set; }
        public List<FormThemeModel> Themes { get; set; }
        public string ImageDescription { get; set; }

    }
}
