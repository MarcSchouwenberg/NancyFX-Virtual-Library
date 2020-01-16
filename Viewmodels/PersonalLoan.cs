using NancyApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NancyApplication.Viewmodels
{
    public class PersonalLoan
    {
        public ICollection<Book> Books { get; set; }
        public Member Member{ get; set; }
        public ICollection<Loan> Loans { get; set; }
    }
}
