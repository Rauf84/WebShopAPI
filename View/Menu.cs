using System;
using System.Collections.Generic;
using System.Text;
using WebShopRauf.Models;

namespace WebShopRauf.View
{
    /// <summary>
    /// Klassen innehåller 3 menyer
    /// </summary>
    public class Menu
    {
        public static string choise = "";

        /// <summary>
        /// Skriver ut första menyn för inloggnong och registrering
        /// </summary>
        public static void ShowLoginRegMenu()
        {
            do
            {
                Console.WriteLine("Välkommen till webbshop");
                Console.WriteLine("1. Logga in");
                Console.WriteLine("2. Registrera");
                Console.WriteLine("q. Avsluta programmet");

                Console.Write("Ditt val: ");
                choise = Console.ReadLine();
                switch (choise)
                {
                    case "1":
                        {
                            WebbShopAPI.Login();
                            break;
                        }
                    case "2":
                        {
                            WebbShopAPI.Register();
                            break;
                        }
                    case "q":
                        {
                            Console.WriteLine("Tack för besöket och välkommen tillbaka. Du loggas ut.");
                            break;
                        }
                    default:
                        {
                            Console.WriteLine("Fel val, vänligen försök igen.");
                            break;
                        }
                }
            }
            while (choise != "q" && WebbShopAPI.status == false);
        }

        /// <summary>
        /// Skriver ut menyn för användaren
        /// </summary>
        public static void ShowUserMenu()
        {
            choise = "";
            do
            {
                Console.WriteLine("1. Hitta en bok");
                Console.WriteLine("2. Lista alla tillgängliga böcker");
                Console.WriteLine("3. Sök efter författare");
                Console.WriteLine("4. Lista alla kategorier");
                Console.WriteLine("5. Lista alla böker med samma kategori");
                Console.WriteLine("6. Köp en bok");
                Console.WriteLine("q. Logga ut");

                Console.Write("Ditt val: ");
                choise = Console.ReadLine();

                switch (choise)
                {
                    case "1":
                        {
                            Console.WriteLine("1. Sök efter titeln");
                            Console.WriteLine("2. Sök efter BookId");
                            Console.WriteLine("3. Tillbaka till huvudmenyn");
                            Console.Write("Ditt val: ");
                            string searchChoise = Console.ReadLine();
                            if (searchChoise == "1")
                            {
                                WebbShopAPI.FindBookByName();
                            }else if (searchChoise == "2")
                            {
                                WebbShopAPI.FindBookById();
                            }
                            Helpers.HelpFunction.Pause();
                            break;
                        }
                    case "2":
                        {
                            var listofbooks = WebbShopAPI.GetAvailableBooks();
                            foreach (var book in listofbooks)
                            {
                                Console.WriteLine($"{book.Id}. {book.Title} | {book.Author} | {book.Price}");
                            }
                            Helpers.HelpFunction.Pause();
                            break;
                        }
                    case "3":
                        {
                            WebbShopAPI.GetAuthors();
                            break;
                        }
                    case "4":
                        {
                            WebbShopAPI.SearchCategory();
                            break;
                        }
                    case "5":
                        {
                            WebbShopAPI.ShowBooksByCategory();
                            break;
                        }
                    case "6":
                        {
                            WebbShopAPI.BuyABook();
                            break;
                        }
                    case "q":
                        {
                            break;
                        }
                    default:
                        {
                            Console.WriteLine("Fel val, vänligen försök igen.");
                            break;
                        }
                }
            } while (choise != "q" && WebbShopAPI.status == true);
        }

        /// <summary>
        /// Skriver ut menyn för administratörer
        /// </summary>
        public static void ShowAdminMenu()
        {

            do
            {
                Console.WriteLine("1. Lägg till en bok");
                Console.WriteLine("2. Ändra antal böcker");
                Console.WriteLine("3. Lista alla användare");
                Console.WriteLine("4. Hitta användare");
                Console.WriteLine("5. Uppdatera en bok");
                Console.WriteLine("6. Radera en bok");
                Console.WriteLine("7. Lägg till en ny kategori");
                Console.WriteLine("8. Ändra kategori i en bok");
                Console.WriteLine("9. Uppdatera kategorier");
                Console.WriteLine("10. Radera en kategori");
                Console.WriteLine("11. Skapa en användare");
                Console.WriteLine("12. Redigera en användare");
                Console.WriteLine("q. Logga ut");
                Console.Write("Ditt val: ");
                choise = Console.ReadLine();

                switch (choise)
                {
                    case "1":
                        {
                            WebbShopAPI.AddBook();
                            Helpers.HelpFunction.Pause();
                            break;
                        }
                    case "2":
                        {
                            List<Book> books = WebbShopAPI.GetAllBooks();
                            WebbShopAPI.ShowBooks(books);
                            Console.Write("Välj en bok i listan (ID): ");
                            int bokId = Convert.ToInt32(Console.ReadLine());
                            WebbShopAPI.SetAmount(bokId);
                            Helpers.HelpFunction.Pause();
                            break;
                        }
                    case "3":
                        {
                            WebbShopAPI.ListUsers();
                            Helpers.HelpFunction.Pause();
                            break;
                        }
                    case "4":
                        {
                            WebbShopAPI.FindUser();
                            Helpers.HelpFunction.Pause();
                            break;
                        }
                    case "5":
                        {
                            WebbShopAPI.UpdateBook();
                            break;
                        }
                    case "6":
                        {
                            WebbShopAPI.DeleteBook();
                            break;
                        }
                    case "7":
                        {
                            WebbShopAPI.AddCategory();
                            break;
                        }
                    case "8":
                        {
                            WebbShopAPI.ChangeCategory();
                            break;
                        }
                    case "9":
                        {
                            WebbShopAPI.UpdateCategory();
                            break;
                        }
                    case "10":
                        {
                            WebbShopAPI.DeleteCategory();
                            break;
                        }
                    case "11":
                        {
                            WebbShopAPI.Register();
                            break;
                        }
                    case "12":
                        {
                            WebbShopAPI.ChangeUser();
                            break;
                        }
                    case "q":
                        {
                            break;
                        }
                    default:
                        {
                            Console.WriteLine("Fel val. Var vänlig försök igen.");
                            break;
                        }
                }

            } while (choise != "q");

        }
    }
}
