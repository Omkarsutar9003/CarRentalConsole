using CarRentalConsole;
internal class Program
{
    private static void Main(string[] args)
    {
        
        
        CarRentalSystem.RegisterUser(new User { Username = "user1", Password = "user123" });

        while (true)
        {
            Console.Clear();
            Console.WriteLine("Welcome to the Car Rental System!");
            Console.WriteLine("--------------------------------");
            Console.WriteLine("1. Admin Login");
            Console.WriteLine("2. User Login");
            Console.WriteLine("3. Register New User");
            Console.WriteLine("4. Exit");
            Console.Write("Enter your choice: ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    Login.AdminLogin();
                    break;
                case "2":
                    Login.UserLogin();
                    break;

                case "3":
                    User.RegisterNewUser();
                    break;
    
                case "4":
                    Console.WriteLine("Goodbye!");
                    return;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
        }
    }
    
}
