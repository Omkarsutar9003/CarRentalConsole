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
        public static bool RentCar(int Car_Id)
        {
            //if (loggedInUser != null)
            //{
            //    var car = cars.Find(c => c.CarId == carId);
            //    if (car != null && car.IsAvailable)
            //    {
            //        car.IsAvailable = false;
            //        return true;
            //    }
            //}
            //return false;

            SqlConnection conn = SqlConnections.GetConnections();
            //conn.ConnectionString = @"Data Source =(localdb)\MSSQLLocalDB; Initial Catalog = Omkar_MS; Integrated Security = True; Connect Timeout = 30; Encrypt = False";
            //conn.Open();
            List<Car> Elist = new List<Car>();
            try
            {
                //  SqlCommand cmd = conn.CreateCommand();

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.Text; // change
                cmd.CommandText = "Select * from Car where Car_Id=@Car_Id";     //    change

                cmd.Parameters.AddWithValue("@Car_Id", Car_Id);

                SqlDataReader dr = cmd.ExecuteReader();

                Car c1 = new Car();
                if (dr.Read())
                {

                    //c1.Car_Id= dr.GetInt32("Car_Id");
                    c1.CarId = Convert.ToInt32(dr["Car_Id"]);
                    c1.Brand = dr.GetString("Car_Name");
                    c1.Model = dr.GetString("Model");
                    //c1.Year = dr.GetInt32("Year");
                    c1.Year = Convert.ToInt32(dr["Year"]);
                    c1.IsAvailable = dr.GetBoolean("isAvailable");
                    //Console.WriteLine($"{c1.Car_Id} {c1.Name} {c1.Model} {c1.Year} - {(c1.IsAvailable ? "Available" : "Rented")}");
                    Console.WriteLine("+------+--------------------------------+----------------------+------------+------------+");
                    Console.WriteLine("|  ID  |      Brand Name                |    Model             | Year       | IsAvailable|");
                    Console.WriteLine("+------+--------------------------------+----------------------+------------+------------+");
                   
                    
                        Console.WriteLine($"| {c1.CarId,-5}| {c1.Brand,-5} | {c1.Model,-10} | {c1.Year,-10} | {(c1.IsAvailable ? "Available" : "Rented"),-10} |");
                    

                    Console.WriteLine("+------+--------------------------------+----------------------+------------+------------+");

                    Console.WriteLine("press 1 to book");
                    int b = int.Parse(Console.ReadLine());
                    if (b == 1 && c1.IsAvailable)
                    {
                        // c1.IsAvailable = false;
                        dr.Close();
                        cmd.CommandText = "UPDATE Car SET isAvailable = 0 WHERE Car_Id = @Car_Id";
                        //cmd.Parameters.AddWithValue("@Car_Id", Car_Id);


                        cmd.ExecuteNonQuery();
                        RentDetailes rent = GetCustDetails();

                        dr.Close();
                        cmd.CommandText = "INSERT INTO Rent_tbl (Car_Id, RentDate, ReturnDate, RentalFee, isPaid, CustomerName, Address, MobileNo) VALUES (@Car_Id, @RentDate, @ReturnDate, @RentalFee, @isPaid, @CustomerName, @Address, @MobileNo)";
                        cmd.Parameters.Clear(); // Clear the previous parameters
                        cmd.Parameters.AddWithValue("@Car_Id", c1.CarId);
                        cmd.Parameters.AddWithValue("@RentDate", DateTime.Now);
                        cmd.Parameters.AddWithValue("@ReturnDate", DateTime.Now.AddDays(7));
                        cmd.Parameters.AddWithValue("@RentalFee", 150m);
                        cmd.Parameters.AddWithValue("@isPaid", false);
                        cmd.Parameters.AddWithValue("@CustomerName", rent.CustmerName); // Corrected parameter name
                        cmd.Parameters.AddWithValue("@Address", rent.Address);
                        cmd.Parameters.AddWithValue("@MobileNo", rent.MobileNo);
                        cmd.ExecuteNonQuery();
                        dr.Close();
                        Console.WriteLine("Car rented successfully.");
                        Methods.ListAvailableCars();
                        
                    }
                    else
                    {
                        Console.WriteLine("Sorry, the selected car is not available for rent.");
                       
                    }
                    
                }

                return true;  
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.Message);
            }
            finally
            {

                conn.Close();
               

            }

            return false;

        }

        static RentDetailes GetCustDetails()
        {
            RentDetailes rent = new RentDetailes();

            Console.WriteLine("Enter name : ");
            rent.CustmerName = Console.ReadLine();

            Console.WriteLine("Enter Address : ");
            rent.Address = Console.ReadLine();

            Console.WriteLine("Enter Mobile No : ");
            rent.MobileNo = Console.ReadLine();

            return rent;
        }
        // Method to return a rented car
        public static bool ReturnCar(int CarId)
        {
            //if (loggedInUser != null)
            //{
            //    var car = cars.Find(c => c.CarId == carId);
            //    if (car != null && !car.IsAvailable)
            //    {
            //        car.IsAvailable = true;
            //        return true;
            //    }
            //}

            SqlConnection conn = SqlConnections.GetConnections();
            //conn.ConnectionString = @"Data Source =(localdb)\MSSQLLocalDB; Initial Catalog = Omkar_MS; Integrated Security = True; Connect Timeout = 30; Encrypt = False";
            //conn.Open();
            List<Car> Elist = new List<Car>();
            try
            {
                //  SqlCommand cmd = conn.CreateCommand();

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.Text; // change
                cmd.CommandText = "Select * from Rent_tbl where Car_Id=@Car_Id";

                cmd.Parameters.AddWithValue("@Car_Id", CarId);

                SqlDataReader dr = cmd.ExecuteReader();

                RentDetailes rent = new RentDetailes();
                if (dr.Read())
                {

                    rent.CarId = Convert.ToInt32(dr["Car_Id"]);





                    dr.Close();
                    cmd.CommandText = "UPDATE Car SET isAvailable = 1 WHERE Car_Id = @Car_Id";
                    //cmd.Parameters.AddWithValue("@Car_Id", rent.CarId);


                    cmd.ExecuteNonQuery();


                    dr.Close();

                    //cmd.CommandText = "INSERT INTO Rent_tbl (Car_Id, RentDate, ReturnDate, RentalFee, isPaid, CustomerName, Address, MobileNo) VALUES (@Car_Id, @RentDate, @ReturnDate, @RentalFee, @isPaid, @CustomerName, @Address, @MobileNo)";
                    //cmd.Parameters.Clear(); // Clear the previous parameters
                    //cmd.Parameters.AddWithValue("@Car_Id", c1.CarId);
                    //cmd.Parameters.AddWithValue("@RentDate", DateTime.Now);
                    //cmd.Parameters.AddWithValue("@ReturnDate", DateTime.Now.AddDays(7));
                    //cmd.Parameters.AddWithValue("@RentalFee", 150m);
                    //cmd.Parameters.AddWithValue("@isPaid", false);
                    //cmd.Parameters.AddWithValue("@CustomerName", rent.CustmerName); // Corrected parameter name
                    //cmd.Parameters.AddWithValue("@Address", rent.Address);
                    //cmd.Parameters.AddWithValue("@MobileNo", rent.MobileNo);
                    //cmd.ExecuteNonQuery();
                    // dr.Close();
                    //Console.WriteLine("Car rented successfully.");
                    Methods.ListAvailableCars();

                }
                //else
                //{
                //    //Console.WriteLine("Sorry, the selected car is not available for rent.");

                //}

            
            
                return true;
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.Message);
            }
            finally
            {

                conn.Close();


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
