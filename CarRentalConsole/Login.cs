using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalConsole
{
    internal class Login
    {
        public static void AdminLogin()
        {
            Console.Clear();
            Console.Write("Enter admin username: ");
            string adminUsername = Console.ReadLine();
            Console.Write("Enter admin password: ");
            string adminPassword = Console.ReadLine();

            if (CarRentalSystem.Login(adminUsername, adminPassword))
            {
                AdminMenu();
            }
            else
            {
                Console.WriteLine("Invalid credentials. Press any key to continue.");
                Console.ReadKey();
            }
        }


        public static void UserLogin()
        {
            Console.Clear();
            Console.Write("Enter your username: ");
            string username = Console.ReadLine();
            Console.Write("Enter your password: ");
            string password = Console.ReadLine();

            if (CarRentalSystem.Login(username, password))
            {
                Console.WriteLine("Log in sucessfull");
                Console.WriteLine();
                UserMenu();
            }
            else
            {
                Console.WriteLine("Invalid credentials. Press any key to continue.");
                Console.ReadKey();
            }
        }

        private static void UserMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("User Menu");
                Console.WriteLine("---------");
                Console.WriteLine("1. Rent a Car");
                Console.WriteLine("2. Return a Car");
                Console.WriteLine("3. Logout");
                Console.Write("Enter your choice: ");
                string choice = Console.ReadLine();
               
                switch (choice)
                {
                    case "1":
                        Methods.RentCar();
                        break;
                    case "2":
                        Methods.ReturnCar();
                        break;
                    case "3":
                        CarRentalSystem.Logout();
                        return;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
        }




        private static void AdminMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Admin Menu");
                Console.WriteLine("----------");
                Console.WriteLine("1. Add Car to Inventory");
                Console.WriteLine("2. Remove Car from Inventory");
                Console.WriteLine("3. List Available Cars");
                Console.WriteLine("4. Logout");
                Console.Write("Enter your choice: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        Methods.AddCarToInventory();
                        break;
                    case "2":
                        Methods.RemoveCarFromInventory();
                        break;
                    case "3":
                        Methods.ListAvailableCars();
                        break;
                    case "4":
                        CarRentalSystem.Logout();
                        return;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
        }

    }
}
