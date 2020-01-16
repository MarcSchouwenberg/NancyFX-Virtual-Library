using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NancyApplication.Models
{
    public class Copy
    {
        public int CopyID { get; set; }
        public int BookID { get; set; }
        public virtual Book Book { get; set; }
        public virtual Loan Loan { get; set; }
        
    }
}
