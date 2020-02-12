using System;
using System.Collections.Generic;

namespace Oeuvre.Models
{
    public partial class GamingInfo
    {
        public GamingInfo()
        {
            CartItems = new HashSet<CartItems>();
        }

        public int GameId { get; set; }
        public string GameTitle { get; set; }
        public string GameDescription { get; set; }
        public DateTime ReleaseDate { get; set; }
        public int GamePlatform { get; set; }
        public int GameCategory { get; set; }
        public long GamePrice { get; set; }

        public virtual GamingCategory GameCategoryNavigation { get; set; }
        public virtual GamingPlatform GamePlatformNavigation { get; set; }
        public virtual BusinessGames BusinessGames { get; set; }
        public virtual ICollection<CartItems> CartItems { get; set; }
    }
}
