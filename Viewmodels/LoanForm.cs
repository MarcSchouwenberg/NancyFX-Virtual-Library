using NancyApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NancyApplication.Viewmodels
{
    public class LoanForm
    {
        public ICollection<Copy> Copies { get; set; }
        public ICollection<Member> Members { get; set; }
        public Loan Loan { get; set; }
    }
}
