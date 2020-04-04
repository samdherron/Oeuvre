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
        public string CuratorName { get; set; }
        public string YearCreated { get; set; }
        public string Medium { get; set; }
        public string CollectionType { get; set; }
        public string PieceDimensions { get; set; }
        public string ImageDescription { get; set; }
        public List<FormThemeModel> Themes { get; set; }



    }
}
