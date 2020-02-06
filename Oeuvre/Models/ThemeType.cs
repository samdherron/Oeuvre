using System;
using System.Collections.Generic;

namespace Oeuvre.Models
{
    public partial class ThemeType
    {
        public ThemeType()
        {
            Theme = new HashSet<Theme>();
        }

        public string ThemeTypeId { get; set; }
        public string ThemeTypeName { get; set; }

        public virtual ICollection<Theme> Theme { get; set; }
    }
}
