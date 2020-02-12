using System;
using System.Collections.Generic;

namespace Oeuvre.Models
{
    public partial class GamingPlatform
    {
        public GamingPlatform()
        {
            GamingInfo = new HashSet<GamingInfo>();
        }

        public int PlatformId { get; set; }
        public string PlatformName { get; set; }

        public virtual ICollection<GamingInfo> GamingInfo { get; set; }
    }
}
