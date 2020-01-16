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

    public class MemberModule : NancyModule
    {
        public MemberModule()
        {
            Get("/members", args => 
            {
                var db = new LibraryContext();
                var memberz = db.Members.Include(m => m.Loans).ToList();
                return View["MemberIndex", memberz];
            });

            Get("/members/page/{size}/{pg}", args =>
            {
                var db = new LibraryContext();
                var nummembers = db.Members.Count();
                var memberz = db.Members.ToArray();
                ArrayList subset = new ArrayList();
                if (args.size * (args.pg-1) < nummembers)
                {
                    if (args.size * args.pg < nummembers)
                    {
                        for (var i = args.size * (args.pg - 1); i < args.size * args.pg; i++)
                        {
                            subset.Add(memberz[i]);
                        }
                    } else
                    {
                        for (var i = args.size * (args.pg - 1); i < nummembers; i++)
                        {
                            subset.Add(memberz[i]);
                        }
                    }
                } else
                {
                    subset.Add(new Member { FirstMidName = null, LastName = null });
                }
                return View["MemberIndex", subset];
            });

            Get("/members/new", args => View["NewMemberForm"]);

            Get("/members/new/confirmnewmember/{fname}/{lname}", args =>
            {
                Member newM = new Member()
                {
                    FirstMidName = args.fname,
                    LastName = args.lname,
                };
                return View["NewMember", newM];
            });

            Get("/members/{id}", args => {
                var db = new LibraryContext();
                var member = db.Members.Find((int)args.id);
                return View["SingleMember", member];
                });

            Get("/members/{id}/confirmDelete", args => {
                var db = new LibraryContext();
                var member = db.Members.Find((int)args.id);
                return View["DeleteMember", member];
            });
            Get("/deletemember/{id}", args =>
            {
                var db = new LibraryContext();
                Member memberToBeDeleted = db.Members.Find((int)args.id);
                db.Members.Remove(memberToBeDeleted);
                db.SaveChanges();
                var memberz = db.Members.ToList();
                return View["MemberIndex", memberz];
            });
            Post("/deletemember", args =>
            {
                var db = new LibraryContext();
                Member memberToBeDeleted = db.Members.Find((int)args.id);
                db.Members.Remove(memberToBeDeleted);
                db.SaveChanges();
                return this.Context.GetRedirect("/members/");
            });

            Post("/members", args => 
            {
                var db = new LibraryContext();
                Member newM = new Member()
                {
                    FirstMidName = this.Request.Form.newFirstMidName,
                    LastName = this.Request.Form.newLastName,
                };
                db.Members.Add(newM);
                db.SaveChanges();
                return this.Context.GetRedirect("/members/" + newM.MemberID);
            });


            Get("/addtestmember", args => {
                var db = new LibraryContext();
                ArrayList names = new ArrayList{ "Jack", "John", "Peter", "Nils", "Will", "Harris", "Adam", "William", "Johan" };
                Member newM = new Member
                {
                    FirstMidName = names[new Random().Next(0, names.Count)].ToString(),
                    LastName = names[new Random().Next(0, names.Count)].ToString()+"son",
                };
                db.Members.Add(newM);
                db.SaveChanges();
                var memberz = db.Members.ToList();
                return this.Context.GetRedirect("/members/" + newM.MemberID);
            });

            Get("/addmember/{fname}/{lname}", args =>
            {
                var db = new LibraryContext();
                Member newM = new Member()
                {
                    FirstMidName = args.fname,
                    LastName = args.lname,
                };
                db.Members.Add(newM);
                db.SaveChanges();
                return View["singlemember", newM];
            });

            Get("/members/{id}/edit", args =>
            {
                var db = new LibraryContext();
                int id = args.id;
                Member memberToBeEdited = db.Members.Find(id);
                return View["EditMemberForm", memberToBeEdited];
            });

            Post("/updatemember/", args =>
            {
                var db = new LibraryContext();
                int id = this.Request.Form.IDtobeupdated;
                var member = db.Members.Find(id);
                member.FirstMidName = this.Request.Form.newFirstMidName;
                member.LastName = this.Request.Form.newLastName;
                db.Members.Update(member);
                db.SaveChanges();
                return this.Context.GetRedirect("/members/" + member.MemberID);
            });

            Get("/members/{id}/editpersonalloans", args =>
            {
                var db = new LibraryContext();
                var member = db.Members.Find((int)args.id);
                var bookz = db.Books.ToList();
                var loanz = db.Loans.Where(l => l.Member.Equals(member)).ToList();
                PersonalLoan newList = new PersonalLoan {Books = bookz, Member = member, Loans = loanz};
                db.Loans.Include(l => l.Copy.Book).ToList();
                return View["EditPersonalLoans", newList];
            });

            Delete("/members/{id}/removepersonalloan/{bookid}", args =>
            {
                var db = new LibraryContext();
                var bookz = db.Books.ToList();
                var member = db.Members.Find((int)args.id);
                ArrayList newList = new ArrayList { bookz, member };
                return View["EditPersonalLoans", newList[0]];
            });

        }
    }
}
