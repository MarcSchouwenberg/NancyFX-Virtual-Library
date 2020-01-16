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
    using System.Collections;

    public class BookModule : NancyModule
    {
        public BookModule()
        {
            Get("/books", args => 
            {
                var db = new LibraryContext();
                var bookz = from b in db.Books select b;
                string searchQ = (string)Request.Query["searchquery"];
                if (Request.Query["searchquery"]) {
                    bookz = bookz.Where(b => b.Title.ToLower().Contains(searchQ.ToLower()));
                }
                string sortOrder = (string)Request.Query["sortorder"];
                switch (sortOrder)
                {
                    case "id":
                        bookz = bookz.OrderBy(b => b.BookID);
                        break;
                    case "id_desc":
                        bookz = bookz.OrderByDescending(b => b.BookID);
                        break;
                    case "title":
                        bookz = bookz.OrderBy(b => b.Title);
                        break;
                    case "title_desc":
                        bookz = bookz.OrderByDescending(b => b.Title);
                        break;
                    case "author":
                        bookz = bookz.OrderBy(b => b.Author);
                        break;
                    case "author_desc":
                        bookz = bookz.OrderByDescending(b => b.Author);
                        break;
                    default:
                        bookz = bookz.OrderBy(b => b.Title);
                        break;
                }
                return View["BookIndex", bookz.ToList()];
            });

            Get("/books/page/{size}/{pg}", args =>
            {
                var db = new LibraryContext();
                var numbooks = db.Books.Count();
                var bookz = db.Books.ToArray();
                ArrayList subset = new ArrayList();
                if (args.size * (args.pg-1) < numbooks)
                {
                    if (args.size * args.pg < numbooks)
                    {
                        for (var i = args.size * (args.pg - 1); i < args.size * args.pg; i++)
                        {
                            subset.Add(bookz[i]);
                        }
                    } else
                    {
                        for (var i = args.size * (args.pg - 1); i < numbooks; i++)
                        {
                            subset.Add(bookz[i]);
                        }
                    }
                } else
                {
                    subset.Add(new Book { Title = null, Author = null });
                }
                return View["BookIndex", subset];
            });

            Get("/books/new", args => View["NewBookForm"]);

            Get("/books/new/confirmnewbook/{ttl}/{aut}", args =>
            {
                Book newB = new Book()
                {
                    Title = args.ttl,
                    Author = args.aut,
                };
                return View["NewBook", newB];
            });

            Get("/books/{id}", args => {
                var db = new LibraryContext();
                int id = args.id;
                var book = db.Books.Include(b => b.Copies).ThenInclude(c => c.Loan).SingleOrDefault(b => b.BookID == id);
                Context.ViewBag.avCopies = book.Copies.Count(c => c.Loan == null);
                return View["SingleBook", book];
                });

            Get("/books/{id}/confirmDelete", args => {
                var db = new LibraryContext();
                int id = args.id;
                var book = db.Books.Find(id);
                return View["DeleteBook", book];
            });
            Get("/deletebook/{id}", args =>
            {
                var db = new LibraryContext();
                int id = args.id;
                Book bookToBeDeleted = db.Books.Find(id);
                db.Books.Remove(bookToBeDeleted);
                db.SaveChanges();
                var bookz = db.Books.ToList();
                return View["BookIndex", bookz];
            });
            Delete("/books/{id}", args =>
            {
                var db = new LibraryContext();
                int id = args.id;
                Book bookToBeDeleted = db.Books.Find(id);
                db.Books.Remove(bookToBeDeleted);
                db.SaveChanges();
                return this.Context.GetRedirect("/books/");
            });

            Post("/books", args => 
            {
                var db = new LibraryContext();
                Book newB = new Book()
                {
                    Title = this.Request.Form.newTitle,
                    Author = this.Request.Form.newAuthor,
                    YearPublished = this.Request.Form.newYearPublished,
                };
                db.Books.Add(newB);
                db.SaveChanges();
                return this.Context.GetRedirect("/books/" + newB.BookID);
            });

            Get("/addtestbook", args => {
                var db = new LibraryContext();
                Book newB = new Book{ Title = "testTitle_" + new Random().Next(1000, 9999), Author = "testAuthor_" + new Random().Next(1000, 9999), YearPublished = 3333 };
                db.Books.Add(newB);
                db.SaveChanges();
                var bookz = db.Books.ToList();
                bookz.Reverse();
                return this.Context.GetRedirect("/books/" + newB.BookID);
            });

            Get("/addbook/{ttl}/{aut}", args =>
            {
                var db = new LibraryContext();
                Book newB = new Book()
                {
                    Title = args.ttl,
                    Author = args.aut,
                };
                db.Books.Add(newB);
                db.SaveChanges();
                return View["singlebook", newB];
            });

            Get("/books/{id}/edit", args =>
            {
                var db = new LibraryContext();
                int id = args.id;
                Book bookToBeEdited = db.Books.Find(id);
                return View["EditBookForm", bookToBeEdited];
            });

            Put("/books/{id}", args =>
            {
                var db = new LibraryContext();
                int id = args.id;
                var book = db.Books.Find(id);
                book.Title = this.Request.Form.newTitle;
                book.Author = this.Request.Form.newAuthor;
                book.YearPublished = this.Request.Form.newYearPublished;
                db.Books.Update(book);
                db.SaveChanges();
                return this.Context.GetRedirect("/books/" + book.BookID);
            });
        }
    }
}
