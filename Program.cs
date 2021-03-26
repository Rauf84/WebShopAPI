using System;
using System.Linq;
using WebShopRauf;

namespace WebShopRauf
{
    class Program
    {
        static void Main(string[] args)
        {
            //Database.Seeder.Seed(); // Inmatnng av mokdata - körs 1 gång 
            
            View.Menu.ShowLoginRegMenu();

            if (WebbShopAPI.status == true && WebbShopAPI.user.IsAdmin == true)
            {
                View.Menu.ShowAdminMenu();
            }else if (WebbShopAPI.status == true && WebbShopAPI.user.IsAdmin == false) View.Menu.ShowUserMenu();
            

           
        }
    }
}
