namespace WebShopRauf.Database
{
    using Microsoft.EntityFrameworkCore;
    using WebShopRauf.Models;
    public class MyContext : DbContext
    {
        private const string DatabaseName = "WebbShopDBRauf"; //Databasnamn som ska skapas
        public DbSet<User> Users { get; set; } // Skapar en tabel för användarna
        public DbSet<Category> Categories { get; set; } // Skapar en tabel för kategorier
        public DbSet<Book> Books { get; set; } // Skapar en tabel för böcker
        public DbSet<SoldBook> SoldBooks { get; set; } // Skapar en tabel för sålda böcker

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) // Konfigurerar ConnectionString
        {
            optionsBuilder.UseSqlServer($@"Server=.\SQLEXPRESS; Database={DatabaseName};Trusted_connection = true;");
        }

    }
}
