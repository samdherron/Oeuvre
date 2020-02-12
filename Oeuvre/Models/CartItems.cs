using System;
using System.Collections.Generic;

namespace Oeuvre.Models
{
    public partial class CartItems
    {
        public int CartItemdId { get; set; }
        public int CartId { get; set; }
        public int GameId { get; set; }

        public virtual Cart Cart { get; set; }
        public virtual GamingInfo Game { get; set; }
    }
}
