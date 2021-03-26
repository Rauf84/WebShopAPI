using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Channels;
using WebShopRauf.Database;
using WebShopRauf.Models;

namespace WebShopRauf
{
    public class WebbShopAPI
    {
        public static bool status = false; // true om användaren är inloggad.
        public static User user; // används för att ha en inloggad användare genom hela programmet
        public static MyContext context = new MyContext(); // context anropas i alla metoder som behöver kopplas till databasen

        /// <summary>
        /// Inloggningsmetod
        /// </summary>
        /// <returns>UserID</returns>
        public static int Login()
        {
            Console.Write("Username: ");
            var username = Console.ReadLine();
            Console.Write("Password: ");
            var password = Console.ReadLine();


            user = context.Users.FirstOrDefault(u => u.Name == username && u.Password == password && u.IsActive);

            if (user != null)
            {
                status = true;
                Console.WriteLine("Du är inloggad");
                user.LastLogin = DateTime.Now;
                user.SessionTime = DateTime.Now;
                context.Users.Update(user);
                context.SaveChanges();
                return user.Id;
            }
            else
            {
                Console.WriteLine("Fel användarnamn eller lösenord");
                return 0;
            }
        }

        /// <summary>
        /// Utloggningsmetod
        /// </summary>
        /// <param name="id"></param>
        public void Logout(int id)
        {
            var user = context.Users.FirstOrDefault(u => u.Id == id && u.SessionTime > DateTime.Now.AddMinutes(-15));

            if (user != null)
            {
                user.SessionTime = DateTime.MinValue;
                context.Users.Update(user);
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Deklarera med en variabel för att få en lista med alla kategorier
        /// </summary>
        /// <returns>List of categories</returns>
        public static List<Category> GetCategories()
        {
            return context.Categories.ToList();
        }

        /// <summary>
        /// Visar alla böcker under samma kategori
        /// </summary>
        /// <returns>List of books</returns>
        public static IEnumerable<Book> ShowBooksByCategory()
        {
            var result = new List<Book>();
            var Cat = GetCategories();
            if (result != null)
            {
                Console.WriteLine();
                Console.WriteLine("Id | Kategori");
                Console.WriteLine("___________________");
                foreach (var cat in Cat)
                {
                    Console.WriteLine($"{cat.Id}.  {cat.Name}");
                }
            }
            else Console.WriteLine("Sökningen gav ingen resultat");

            Console.WriteLine();
            Console.Write("Välj en kategori (Id): ");
            var catId = Convert.ToInt32(Console.ReadLine());
            result = context.Books.Where(b=>b.CategoryId == catId).ToList();
            if (result != null)
            {
                Console.WriteLine();
                Console.WriteLine("Id | Bok");
                Console.WriteLine("___________________");
                foreach (var book in result)
                {
                    Console.WriteLine($"{book.Id}.  {book.Title}");
                }
            }
            else Console.WriteLine("Sökningen gav ingen resultat");

            Helpers.HelpFunction.Pause();
            return result;
        }

        /// <summary>
        /// Söker bland kategorier
        /// </summary>
        /// <returns>List of category</returns>
        public static IEnumerable<Category> SearchCategory()
        {
            List<Category> result = new List<Category>();
            Console.Write("Enter keyword: ");
            var keyword = Console.ReadLine();
            result = context.Categories.Where(c => c.Name.Contains(keyword)).OrderBy(c => c.Name).ToList();
            if (result != null)
            {
                foreach (var cat in result)
                {
                    Console.WriteLine($"{cat.Id}. {cat.Name}");
                }
            }
            else Console.WriteLine("Sökningen gav ingen resultat");
            Helpers.HelpFunction.Pause();
            return result;
        }

        /// <summary>
        /// Ska deklareras för att få en lista av befintliga böcker
        /// </summary>
        /// <returns>List of books</returns>
        public static List<Book> GetAvailableBooks()
        {
            return context.Books.Where(b => b.Amount > 0).ToList();
        }

        /// <summary>
        /// Ska deklareras för att få en lista med alla böcker i databasen
        /// </summary>
        /// <returns>List of books</returns>
        public static List<Book> GetAllBooks()
        {
            return context.Books.ToList();
        }
        
        /// <summary>
        /// Metod som skapar en ny användare. OBS! Admin kan skapas bara av en annan admin.
        /// </summary>
        public static void Register()
        {
            Console.WriteLine("Registrerings sida.");
            Console.Write("Användarnamn: ");
            var username = Console.ReadLine();
            Console.Write("Lösenord: ");
            var password = Console.ReadLine();
            Console.Write("Bekräfta lösenord: ");
            var checkPassword = Console.ReadLine();
            if (password == checkPassword)
            {
                Console.WriteLine("Ditt konto har skapats! ");
                context.Users.Add(new Models.User { Name = $"{username}", Password = $"{password}", LastLogin = DateTime.Now, SessionTime = DateTime.Now, IsActive = true, IsAdmin = false });
                context.SaveChanges();
            }
            else 
            {
                Console.WriteLine("Det gick inte att bekräfta lösenordet.");
            }

        }

        /// <summary>
        /// Metod som skriver ut information om en bok. Sökningen använder sig av bok ID.
        /// </summary>
        public static void FindBookById()
        {
            var cat = GetCategories().ToList();
            Console.Write("Sök på bokens id: ");
            int bookId = Convert.ToInt32(Console.ReadLine());
            var result = context.Books.Find(bookId);
            Console.Write("Titel: ");
            Console.WriteLine(result.Title);
            Console.Write("Författare: ");
            Console.WriteLine(result.Author);
            Console.Write("Kategori: ");
            Console.WriteLine(cat[result.CategoryId - 1].Name);
            Console.Write("Pris: ");
            Console.WriteLine(result.Price);
            Console.Write("Antal i lagret: ");
            Console.WriteLine(result.Amount);

        }

        /// <summary>
        /// Metod skapar en lista med böcker som innehåller sökordet
        /// </summary>
        /// <returns>List of books</returns>
        public static IEnumerable<Book> FindBookByName()
        {
            Console.Write("Sök på bokens titel: ");
            var bookName = Console.ReadLine();

            var cat = GetCategories().ToList();
            var result = context.Books.Where(b => b.Title.Contains(bookName)).OrderBy(c => c.Title);

            foreach (var book in result)
            {
                
                Console.WriteLine("=====================");
                Console.Write("Titel: ");
                Console.WriteLine(book.Title);
                Console.Write("Författare: ");
                Console.WriteLine(book.Author);
                Console.Write("Kategori: ");
                Console.WriteLine(cat[book.CategoryId-1].Name);
                Console.Write("Pris: ");
                Console.WriteLine(book.Price);
                Console.Write("Antal i lagret: ");
                Console.WriteLine(book.Amount);
                Console.WriteLine("=====================");
                Console.WriteLine();
            }

            return result;
        }

        /// <summary>
        /// Metoden skapar en lista med böcker som innehåller sökordet som söker bland författarna 
        /// </summary>
        /// <returns>lsit of books</returns>
        public static IEnumerable<Book> GetAuthors()
        {
            Console.Write("Sök på författarens namn: ");
            var autName = Console.ReadLine();
            var cat = GetCategories();
            var result = context.Books.Where(b => b.Author.Contains(autName)).OrderBy(c => c.Title);

            foreach (var book in result)
            {
                Console.Write("Författare: ");
                Console.WriteLine(book.Author);
                Console.Write("Titel: ");
                Console.WriteLine(book.Title);
                Console.Write("Kategori: ");
                Console.WriteLine(cat[book.CategoryId-1].Name);
                Console.Write("Pris: ");
                Console.WriteLine(book.Price);
                Console.Write("Antal i lagret: ");
                Console.WriteLine(book.Amount);
            }
            Helpers.HelpFunction.Pause();
            return result;
        }

        /// <summary>
        /// Metoden listar alla tillgängliga böcker och utför minskning av antalet böcker i databasen 
        /// </summary>
        public static void BuyABook()
        {
            Console.WriteLine("Välj boken du vill köpa i listan: ");
            var avalibleBooks = GetAvailableBooks();
            Console.WriteLine("ID | Title");
            Console.WriteLine("___________");
            foreach (var book in avalibleBooks)
            {
                Console.WriteLine($"{book.Id} | {book.Title}");
            }
            Console.Write("Bok ID: ");
            var bokId = Convert.ToInt32(Console.ReadLine());
            var bok = avalibleBooks.FirstOrDefault(c => c.Id == bokId);


            context.SoldBooks.Add(new Models.SoldBook {
                Title = bok.Title,
                Author = bok.Author,
                CategoryId = bok.CategoryId,
                Price = bok.Price,
                PurchasedDate = DateTime.Now,
                UserId = user.Id });
            bok.Amount -= 1;
            context.SaveChanges();
        }

        /// <summary>
        /// Metoden kontrollerar om inmatad bok fins i databasen, om inte - lägger till en
        /// </summary>
        public static void AddBook()
        {
            Console.Write("Titel: ");
            var titel = Console.ReadLine();
            Console.Write("Author: ");
            var author = Console.ReadLine();
            Console.Write("Price: ");
            int price = Convert.ToInt32(Console.ReadLine());
            Console.Write("Amount: ");
            int amount = Convert.ToInt32(Console.ReadLine());
            Console.Write("CategoryId 1-5: ");
            int categoryId = Convert.ToInt32(Console.ReadLine());

            if (CheckBook(titel, author)) 
            {
                Console.WriteLine("Boken du vill lägga till finns redan.");
                var book = context.Books.FirstOrDefault(b=>b.Title == titel && b.Author==author);
                SetAmount(book.Id);
            };

            context.Books.Add(new Models.Book { Title = titel, Author = author, Price = price, Amount = amount, CategoryId = categoryId });
            context.SaveChanges();
        }

        /// <summary>
        /// Metoden kontrollerar om boken med angivet titel och författare fins redan i databasen
        /// </summary>
        /// <param name="titel"></param>
        /// <param name="author"></param>
        /// <returns>true if exists</returns>
        public static bool CheckBook (string titel, string author)
        {
            bool result = false;
            var search = context.Books.Where(b => b.Author.Contains(author) && b.Title.Contains(titel)).ToList();
            if (search.Count != 0)
            {
                result = true;
            }

            return result;
        }

        /// <summary>
        /// Lägger till angivet antal böcker till en vald bok ID.
        /// </summary>
        /// <param name="bookId"></param>
        public static void SetAmount(int bookId)
        {
            Console.Write("Hur många böker vill du lägga till: ");
            int addAmount = Convert.ToInt32(Console.ReadLine());
            var bok = context.Books.FirstOrDefault(b=>b.Id == bookId);
            bok.Amount += addAmount;
            context.SaveChanges();
        }

        /// <summary>
        /// skriver ut alla böcker i en lista
        /// </summary>
        /// <param name="books"></param>
        public static void ShowBooks(List<Book> books)
        {
            foreach (var bok in books)
            {
                Console.WriteLine($"{bok.Id}.  {bok.Title}, Author: {bok.Author}, Pris: {bok.Price}");
            }
        }

        /// <summary>
        /// Skriver ut detaljer från varje användare i databasen
        /// </summary>
        public static void ListUsers()
        {
            List<User> users = new List<User>();
            users = context.Users.ToList();
            Console.WriteLine("Information om användarna:");
            foreach (var user in users)
            {
                Console.WriteLine("====================================");
                Console.WriteLine($"UserID: {user.Id}");
                Console.WriteLine($"Username: {user.Name}");
                Console.WriteLine($"Password: {user.Password}");
                Console.WriteLine($"Admin: {user.IsAdmin.ToString()}");
                Console.WriteLine($"Active: {user.IsActive.ToString()}");
                Console.WriteLine($"Last login: {user.LastLogin.ToString()}");
                Console.WriteLine("====================================");
            }
        }

        /// <summary>
        /// Metoden skapar och skriver ut en lista av användare sominnehåller sökord
        /// </summary>
        /// <returns>List of users</returns>
        public static IEnumerable<User> FindUser()
        {
            Console.Write("Sökord: ");
            var userName = Console.ReadLine();

            var cat = GetCategories().ToList();
            var result = context.Users.Where(u => u.Name.Contains(userName)).OrderBy(c => c.Id);
            if (result.Count() != 0)
            {
                foreach (var user in result)
                {
                    Console.WriteLine("====================================");
                    Console.WriteLine($"UserID: {user.Id}");
                    Console.WriteLine($"Username: {user.Name}");
                    Console.WriteLine($"Password: {user.Password}");
                    Console.WriteLine($"Admin: {user.IsAdmin.ToString()}");
                    Console.WriteLine($"Active: {user.IsActive.ToString()}");
                    Console.WriteLine($"Last login: {user.LastLogin.ToString()}");
                    Console.WriteLine("====================================");
                }
            }

            return result;
        }

        /// <summary>
        /// Metoden skrivet ut alla böker i databasen, användaren väljer en bok som ska ändras och sedan väljer i menyn vilka parametrar ska ändras.
        /// </summary>
        public static void UpdateBook()
        {
            string select = "";
            var books = context.Books.ToList();
            ShowBooks(books);
            Console.Write("Vilken bok vill du ändra (ID): ");
            int bookId = Convert.ToInt32(Console.ReadLine());
            var book = books.FirstOrDefault(b=>b.Id == bookId);
            do
            {
                Console.WriteLine($"Du har valt: {book.Id}. {book.Title}  |  {book.Author}");
                Console.WriteLine("Välj vad du vill ändra på:");
                Console.WriteLine("1. Titel");
                Console.WriteLine("2. Författare");
                Console.WriteLine("3. Kategori");
                Console.WriteLine("4. Pris");
                Console.WriteLine("5. Antal på lagret");
                Console.WriteLine("q. Gå till huvudmenyn");
                Console.Write("Ditt val: ");
                select = Console.ReadLine();
                switch (select)
                {
                    case "1":
                        {
                            Console.Write("Ny titel: ");
                            book.Title = Console.ReadLine();

                            Console.Write("Vill du spara ändringar (Y/N:)");
                            var ans = Console.ReadLine();
                            if (ans == "Y" || ans == "y")
                            {
                                context.SaveChanges();
                                Console.WriteLine("Informationen om boken har sparats.");
                                Helpers.HelpFunction.Pause();
                            }
                            break;
                        }
                    case "2":
                        {
                            Console.Write("Ny författare: ");
                            book.Author = Console.ReadLine();

                            Console.Write("Vill du spara ändringar (Y/N:)");
                            var ans = Console.ReadLine();
                            if (ans == "Y" || ans == "y")
                            {
                                context.SaveChanges();
                                Console.WriteLine("Informationen om boken har sparats.");
                                Helpers.HelpFunction.Pause();
                            }
                            break;
                        }
                    case "3":
                        {
                            GetCategories();
                            Console.Write("Ny Kategori ID: ");
                            book.CategoryId = Convert.ToInt32(Console.ReadLine());

                            Console.Write("Vill du spara ändringar (Y/N:)");
                            var ans = Console.ReadLine();
                            if (ans == "Y" || ans == "y")
                            {
                                context.SaveChanges();
                                Console.WriteLine("Informationen om boken har sparats.");
                                Helpers.HelpFunction.Pause();
                            }
                            break;
                        }
                    case "4":
                        {
                            Console.Write("Nytt pris: ");
                            book.Price = Convert.ToInt32(Console.ReadLine());

                            Console.Write("Vill du spara ändringar (Y/N:)");
                            var ans = Console.ReadLine();
                            if (ans == "Y" || ans == "y")
                            {
                                context.SaveChanges();
                                Console.WriteLine("Informationen om boken har sparats.");
                                Helpers.HelpFunction.Pause();
                            }
                            break;
                        }
                    case "5":
                        {
                            Console.Write("Nytt antal: ");
                            book.Amount = Convert.ToInt32(Console.ReadLine());

                            Console.Write("Vill du spara ändringar (Y/N:)");
                            var ans = Console.ReadLine();
                            if (ans == "Y" || ans == "y")
                            {
                                context.SaveChanges();
                                Console.WriteLine("Informationen om boken har sparats.");
                                Helpers.HelpFunction.Pause();
                            }
                            break;
                        }
                    case "q":
                        {
                            Console.Clear();
                            break;
                        }
                }
            } while (select != "q");
        }

        /// <summary>
        /// Metoden miskar antalet böcker med 1. Om antalet är 0 ska boker raderas från databasen.
        /// </summary>
        public static void DeleteBook()
        {
            int bookId;
            var books = GetAllBooks();
            ShowBooks(books);
            Console.Write("Välj bok i listan du vill radera: ");
            bookId = Convert.ToInt32(Console.ReadLine());
            var book = context.Books.FirstOrDefault(b=>b.Id == bookId);
            if (book.Amount > 0)
            {
                book.Amount -= 1;
                Console.WriteLine($"En bok med titeln {book.Title} tas bort från lagret. Antal böcker kvar: {book.Amount}");
            }else
            {
                Console.WriteLine("Det finns inga böcker kvar. Boken tas bort från registret.");
                context.Books.Remove(book);
                context.SaveChanges();
                Console.WriteLine("Boken har raderats.");
            }
            Helpers.HelpFunction.Pause();
        }

        /// <summary>
        /// Metod lägger till en ny kategori i databasen.
        /// </summary>
        public static void AddCategory()
        {
            Console.Write("Ny kategori: ");
            var cat = Console.ReadLine();
            context.Categories.Add(new Models.Category { Name = cat });
            context.SaveChanges();
        }

        /// <summary>
        /// Metoden ändrar kategorin i en bok
        /// </summary>
        public static void ChangeCategory()
        {
            var books = GetAllBooks();
            ShowBooks(books);
            Console.Write("Vilken bok vill du kategorisera (Id):");
            var bookId = Convert.ToInt32(Console.ReadLine());
            var book = context.Books.FirstOrDefault(b=>b.Id == bookId);
            var categories = GetCategories();
            Console.WriteLine();
            foreach (var cat in categories)
            {
                Console.WriteLine($"{cat.Id}. {cat.Name}");
            }
            Console.Write("Vilken kategori vill du lägga boken till (Id): ");
            Console.WriteLine();
            book.CategoryId = Convert.ToInt32(Console.ReadLine());
            context.SaveChanges();
        }

        /// <summary>
        /// Metoden ändrar namnet på en kategori
        /// </summary>
        public static void UpdateCategory()
        {
            var categoreis = GetCategories();
            foreach (var cat in categoreis)
            {
                Console.WriteLine($"{cat.Id}. {cat.Name}");
            }
            Console.Write("Välj kategorin du vill ändra namnet på (Id):");
            int ans = Convert.ToInt32(Console.ReadLine());
            var category = context.Categories.FirstOrDefault(c=>c.Id==ans);
            Console.Write("Nytt namn på kategorin: ");
            category.Name = Console.ReadLine();
            context.SaveChanges();
        }

        /// <summary>
        /// Metoden raderar en angivet kategori från databasen
        /// </summary>
        public static void DeleteCategory() 
        {
            var categoreis = GetCategories();
            foreach (var cat in categoreis)
            {
                Console.WriteLine($"{cat.Id}. {cat.Name}");
            }
            Console.Write("Välj kategorin du vill radera (Id):");
            int ans = Convert.ToInt32(Console.ReadLine());
            var category = context.Categories.FirstOrDefault(c => c.Id == ans);
            if (category.Name != null)
            {
                Console.Write("Kategorin är inte tom. Vill du radera ändå (Y/N): ");
                var yon = Console.ReadLine();
                if (yon == "y" || yon == "Y")
                {
                    context.Categories.Remove(category);
                    context.SaveChanges();
                    Console.WriteLine("Kategorin har raderats.");
                }
                else { Console.WriteLine("Ändringarna har inte sparats."); }
            }else 
            { 
                Console.WriteLine("Kategorin är tom och kommer att raderas.");
                context.Categories.Remove(category);
                context.SaveChanges();
                Console.WriteLine("Kategorin har raderats.");
                Helpers.HelpFunction.Pause();
            }
        }

        /// <summary>
        /// Metoden öppnar upp en meny med val av vilka parametrar ska ändras av en angiven användare.
        /// </summary>
        public static void ChangeUser()
        {
            string ans = "";
            ListUsers();
            Console.Write("Välj användare du vill redigera (ID): ");
            int uId = Convert.ToInt32(Console.ReadLine());
            var user = context.Users.FirstOrDefault(u=>u.Id== uId);
            if (user != null)
            {
                do
                {
                    Console.WriteLine("====================================");
                    Console.WriteLine($"1. Username: {user.Name}");
                    Console.WriteLine($"2. Password: {user.Password}");
                    Console.WriteLine($"3. Admin: {user.IsAdmin.ToString()}");
                    Console.WriteLine($"4. Active: {user.IsActive.ToString()}");
                    Console.WriteLine($"q. Gå till huvudmenyn");
                    Console.WriteLine("====================================");
                    Console.Write("Vad vill du ändra på:");
                    ans = Console.ReadLine();
                    switch (ans)
                    {
                        case "1":
                            {
                                Console.Write("Nytt användarnamn: ");
                                user.Name = Console.ReadLine();
                                context.SaveChanges();
                                break;
                            }
                        case "2":
                            {
                                Console.Write("Nytt lösenord: ");
                                user.Password = Console.ReadLine();
                                context.SaveChanges();
                                break;
                            }
                        case "3":
                            {
                                Console.Write("Ändra adminstatus (true/false): ");
                                user.IsAdmin = Convert.ToBoolean(Console.ReadLine());
                                context.SaveChanges();
                                break;
                            }
                        case "4":
                            {
                                Console.Write("Ändra aktivitetsstatus (true/false): ");
                                user.IsActive = Convert.ToBoolean(Console.ReadLine());
                                context.SaveChanges();
                                break;
                            }
                        case "q":
                            {
                                break;
                            }
                        default:
                            {
                                Console.WriteLine("Fel val i menyn. Var vänlig försök igen.");
                                break;
                            }
                    }

                } while (ans != "q");
            }

        }
    }
}