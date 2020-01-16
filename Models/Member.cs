using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NancyApplication.Models
{
    public class Member
    {
        public int MemberID { get; set; }
        public string LastName { get; set; }
        public string FirstMidName { get; set; }

        public virtual ICollection<Loan> Loans { get; set; }
    }
}
