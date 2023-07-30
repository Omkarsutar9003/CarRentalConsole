using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalConsole
{
    internal class CarRentalSystem
    {
        private static List<Car> cars = new List<Car>();
        private static List<User> users = new List<User>();
        private static User loggedInUser;

        // Admin login credentials
        private const string adminUsername = "admin";
        private const string adminPassword = "admin123";

        // Method to add a car to the inventory
        public static void AddCar(Car car)
        {
            car.CarId = cars.Count + 1;
            car.IsAvailable = true;
            cars.Add(car);
        }

        // Method to remove a car from the inventory
        public static void RemoveCar(int carId)
        {
            
        }

        // Method to list available cars
        public static List<Car> GetAvailableCars()
        {
            return cars.FindAll(car => car.IsAvailable);
        }

        // Method to perform the login process
        public static bool Login(string username, string password)
        {
            string fetchedUsername=null;
            string fetchedPassword=null;
            SqlConnection conn = SqlConnections.GetConnections();
            try
            {
                SqlCommand cmd = conn.CreateCommand();

                cmd.Connection = conn;
                cmd.CommandType = CommandType.Text; // change
                cmd.CommandText = " SELECT * FROM Users WHERE Username = @Username";     //    change

                int Id = 100;
                cmd.Parameters.AddWithValue("@Username", username);
                SqlDataReader dr = cmd.ExecuteReader();


                if (dr.Read())
                {
                     fetchedUsername = dr.GetString("Username");
                     fetchedPassword = dr.GetString("password");

                }

                   // cmd.ExecuteNonQuery();
            }


            catch (Exception Ex)
            {
                Console.WriteLine(Ex.Message);
            }
            finally
            {

                conn.Close();
            }

            if (username == adminUsername && password == adminPassword)
            {
                loggedInUser = null; // Set logged in user as admin
                return true;
            }

            
            if (fetchedUsername == username && fetchedPassword == password)
            {
                var user = new User();
                loggedInUser = user;
                return true;
            }

            return false;
        }

        // Method to log out
        public static void Logout()
        {
            loggedInUser = null;
        }

        // Method to rent a car
        public static bool RentCar(int carId)
        {
            if (loggedInUser != null)
            {
                var car = cars.Find(c => c.CarId == carId);
                if (car != null && car.IsAvailable)
                {
                    car.IsAvailable = false;
                    return true;
                }
            }
            return false;
        }

        // Method to return a rented car
        public static bool ReturnCar(int carId)
        {
            if (loggedInUser != null)
            {
                var car = cars.Find(c => c.CarId == carId);
                if (car != null && !car.IsAvailable)
                {
                    car.IsAvailable = true;
                    return true;
                }
            }
            return false;
        }

        // Method to register a new user
        public static void RegisterUser(User user)
        {
            users.Add(user);
        }
    }


}
