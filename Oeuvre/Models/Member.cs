using System;
using System.Collections.Generic;

namespace Oeuvre.Models
{
    public partial class Member
    {
        public Member()
        {
            Cart = new HashSet<Cart>();
        }

        public int MemberId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MemberAddress { get; set; }
        public string MemberCity { get; set; }
        public string MemberCountry { get; set; }
        public string MemberPostalCode { get; set; }
        public string MemberPhoneNumber { get; set; }
        public string MemberEmail { get; set; }

        public virtual MemberLogin MemberLogin { get; set; }
        public virtual ICollection<Cart> Cart { get; set; }
    }
}
