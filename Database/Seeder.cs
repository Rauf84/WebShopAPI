using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebShopRauf.Database
{
    /// <summary>
    /// Klassen används bara för att mata in mokdata
    /// </summary>
    public static class Seeder
    {
        public static void Seed()
        {

            var db = new MyContext();

            //Kategorier
            if (db.Categories.Count() == 0)
            {
                db.Categories.Add(new Models.Category { Name = "Horror" });
                db.Categories.Add(new Models.Category { Name = "Comedy" });
                db.Categories.Add(new Models.Category { Name = "Science Fiction" });
                db.Categories.Add(new Models.Category { Name = "Romance" });
                db.Categories.Add(new Models.Category { Name = "Biography" });
                db.SaveChanges();
            }

            //Böcker
            if (db.Books.Count() == 0)
            {
                db.Books.Add(new Models.Book { Title = "Cabal (Nightbreed)", Author = "Clive Barker", Price = 250, Amount = 3, CategoryId = 1 });
                db.Books.Add(new Models.Book { Title = "The Shinng", Author = "Stephen King", Price = 200, Amount = 2, CategoryId = 1 });
                db.Books.Add(new Models.Book { Title = "Doctor Sleep", Author = "Stephen King", Price = 200, Amount = 1, CategoryId = 1 });
                db.Books.Add(new Models.Book { Title = "I Robot", Author = "Isaac Asimov", Price = 150, Amount = 4, CategoryId = 3 });
                db.SaveChanges();
            }

            //Användare & Admin
            if (db.Users.Count() == 0)
            {
                db.Users.Add(new Models.User { Name = "Rauf", Password = "123456789", LastLogin = DateTime.Now, SessionTime = DateTime.Now, IsActive = true, IsAdmin = true });
                db.Users.Add(new Models.User { Name = "TestUser", Password = "123456789", LastLogin = DateTime.Now, SessionTime = DateTime.Now, IsActive = true, IsAdmin = false });
                db.SaveChanges();
            }


        }
    }
}
