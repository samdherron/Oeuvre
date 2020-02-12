using System;
using System.Collections.Generic;

namespace Oeuvre.Models
{
    public partial class MemberLogin
    {
        public int MemberId { get; set; }
        public string MemberUsername { get; set; }
        public string MemberPassword { get; set; }
        public bool IsValid { get; set; }
        public string MemberName { get; set; }
        public string Token { get; set; }
        public string Salt { get; set; }

        public virtual Member Member { get; set; }
    }
}
