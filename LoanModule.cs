using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nancy.Extensions;
using NancyApplication.Models;

namespace NancyApplication
{
    using Microsoft.EntityFrameworkCore;
    using Nancy;
    using NancyApplication.DAL;
    using NancyApplication.Viewmodels;
    using System.Collections;

    public class LoanModule : NancyModule
    {
        public LoanModule()
        {
            Get("/loans", args => 
            {
                var db = new LibraryContext();
                var loanz = db.Loans.Include(Loan => Loan.Copy.Book).Include(loan => loan.Member).ToList();

                return View["LoanIndex", loanz];
            });

             Get("/loans/new", args =>
             {
                 var db = new LibraryContext();
                 var copies = db.Copy.Include(c => c.Loan).Include(c => c.Book).Where(c => c.Loan == null).ToList();
                 LoanForm model = new LoanForm { Members = db.Members.ToList(), Copies = copies };
                 return View["NewLoanForm", model];
             });
             Get("/loans/{id}/edit", args =>
             {
                 var db = new LibraryContext();
                 var copies = db.Copy.Include(c => c.Loan).Include(c => c.Book).Where(c => c.Loan == null).ToList();
                 var id = (int)args.id;
                 LoanForm model = new LoanForm
                 {
                    Loan = db.Loans.Include(l => l.Copy.Book).SingleOrDefault(l => l.LoanID==id),
                    Members = db.Members.ToList(),
                    Copies = copies
                 };
                 return View["EditLoanForm", model];
             });
            Get("/loans/{id}", args => {
                var db = new LibraryContext();
                var loan = db.Loans.Find((int)args.id);

                db.Loans.Include(l => l.Copy.Book).Include(l => l.Member).ToList();
                return View["SingleLoan", loan];
            });

            Get("/loans/{id}/confirmDelete", args => {
                var db = new LibraryContext();
                int id = (int)args.id;
                var loan = db.Loans.Include(l => l.Copy.Book).Include(l => l.Member).Where(l=> l.LoanID==id).First();
                
                return View["DeleteLoan", loan];
            });
            Delete("/loans/{id}", args =>
            {
                var db = new LibraryContext();
                int id = args.id;
                Loan loanToBeDeleted = db.Loans.Find(id);
                db.Loans.Remove(loanToBeDeleted);
                db.SaveChanges();
                return this.Context.GetRedirect("/loans/");
            });

             Post("/loans", args => 
             {
                 var db = new LibraryContext();
                 Loan newL = new Loan()
                 {
                     CopyID = this.Request.Form.newCopyID,
                     MemberID = this.Request.Form.newMemberID,
                     LoanDate = DateTime.Now
                 };
                 db.Loans.Add(newL);
                 db.SaveChanges();
                 return this.Context.GetRedirect("/loans/" + newL.LoanID);
             });

            Post("/updateloan/", args =>
            {
                var db = new LibraryContext();
                int id = (int)this.Request.Form.IDtobeupdated;
                var loan = db.Loans.Find(id);
                loan.MemberID = this.Request.Form.newMemberID;
                loan.CopyID = this.Request.Form.newCopyID;
                db.Loans.Update(loan);
                db.SaveChanges();
                return this.Context.GetRedirect("/loans/" + loan.LoanID);
            });

        }
    }
}
