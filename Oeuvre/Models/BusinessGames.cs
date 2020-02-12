using System;
using System.Collections.Generic;

namespace Oeuvre.Models
{
    public partial class BusinessGames
    {
        public int BusinessId { get; set; }
        public int GameId { get; set; }

        public virtual Business Business { get; set; }
        public virtual GamingInfo Game { get; set; }
    }
}
