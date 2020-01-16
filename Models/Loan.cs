using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NancyApplication.Models
{
    public class Loan
    {
        public int LoanID { get; set; }
        public int MemberID { get; set; }
        public int CopyID { get; set; }
        public virtual Copy Copy { get; set; }
        public DateTime LoanDate { get; set; }

        public virtual Member Member { get; set; }
    }

}
