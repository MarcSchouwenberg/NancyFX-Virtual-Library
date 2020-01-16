namespace NancyApplication
{
    using Microsoft.AspNetCore.Builder;
    using Nancy.Owin;
    using NancyApplication.DAL;
    using NancyApplication.Models;
    using System;
    using System.Linq;

    public class Startup
    {
        public void Configure(IApplicationBuilder app)
        {
            app.UseOwin(x => x.UseNancy());
            using (var db = new LibraryContext())
            {
                Console.WriteLine("NancyLib!");
                Console.WriteLine("{0} books in database", db.Books.Count());
                Console.WriteLine("{0} copies in database", db.Copy.Count());
                Console.WriteLine("{0} members in database", db.Members.Count());
                Console.WriteLine("{0} loans in database\n", db.Loans.Count());
            }
        }
    }
}
