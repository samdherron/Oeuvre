using System;
using System.Collections.Generic;

namespace Oeuvre.Models
{
    public partial class Cart
    {
        public Cart()
        {
            CartItems = new HashSet<CartItems>();
        }

        public int CartId { get; set; }
        public int MemberId { get; set; }

        public virtual Member Member { get; set; }
        public virtual ICollection<CartItems> CartItems { get; set; }
    }
}
