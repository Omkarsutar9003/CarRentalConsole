using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CarRentalConsole
{
    internal class Methods
    {

        public static void AddCarToInventory()
        {
            Console.Clear();
            Console.WriteLine("Add Car to Inventory");
            Console.WriteLine("---------------------");
            Console.Write("Enter car brand: ");
            string brand = Console.ReadLine();
            Console.Write("Enter car model: ");
            string model = Console.ReadLine();
            Console.Write("Enter car model year: ");
            string year = Console.ReadLine();
            SqlConnection conn = SqlConnections.GetConnections();

            try
            {
                SqlCommand cmd = conn.CreateCommand();

                cmd.Connection = conn;
                cmd.CommandType = CommandType.Text; // change
                cmd.CommandText = "INSERT INTO Car (Car_Name, Model, Year, IsAvailable) VALUES (@Car_Name, @Model, @Year, 1)";     //    change

                int Id = 100;
                //cmd.Parameters.AddWithValue("@Car_Id", CarId);
                cmd.Parameters.AddWithValue("@Car_Name", brand);
                cmd.Parameters.AddWithValue("@Model", model);
                cmd.Parameters.AddWithValue("@Year", year);

                cmd.ExecuteNonQuery();
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.Message);
            }
            finally
            {

                conn.Close();
            }



            // CarRentalSystem.AddCar(new Car { Brand = brand, Model = model });
            Console.WriteLine("Car added to inventory successfully. Press any key to continue.");
            Console.ReadKey();
        }

        public static void RemoveCarFromInventory()
        {
            Console.Clear();
            Console.WriteLine("Remove Car from Inventory");
            Console.WriteLine("--------------------------");
            Console.Write("Enter car ID to remove: ");
            if (int.TryParse(Console.ReadLine(), out int carId))
            {
                // CarRentalSystem.RemoveCar(carId);

                

                SqlConnection conn = SqlConnections.GetConnections();

                try
                {
                    SqlCommand cmd = conn.CreateCommand();

                    cmd.Connection = conn;
                    cmd.CommandType = CommandType.Text; // change
                    cmd.CommandText = "DELETE FROM Car WHERE Car_Id = @Car_Id";     //    change

                    int Id = 100;
                    cmd.Parameters.AddWithValue("@Car_Id", carId);
                    cmd.ExecuteNonQuery();
                }
                
            
                catch (Exception Ex)
                {
                Console.WriteLine(Ex.Message);
                }
            finally
            {

                conn.Close();
            }






            Console.WriteLine("Car removed from inventory successfully. Press any key to continue.");
                Console.ReadKey();
            }
            else
            {
                Console.WriteLine("Invalid car ID. Press any key to continue.");
                Console.ReadKey();
            }
        }

        public static void ListAvailableCars()
        {
            Console.Clear();
            Console.WriteLine("Available Cars");
            Console.WriteLine("--------------");





            SqlConnection conn = SqlConnections.GetConnections();

            List<Car> Elist = new List<Car>();
            try
            {
                //  SqlCommand cmd = conn.CreateCommand();

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.Text; // change
                cmd.CommandText = "Select * from Car ";     //    change

                // cmd.Parameters.AddWithValue("@EmpNo", EmpNo);

                SqlDataReader dr = cmd.ExecuteReader();


                while (dr.Read())
                {
                    Car c1 = new Car();
                    //c1.Car_Id= dr.GetInt32("Car_Id");
                    c1.CarId = Convert.ToInt32(dr["Car_Id"]);
                    c1.Brand = dr.GetString("Car_Name");
                    c1.Model = dr.GetString("Model");
                    //c1.Year = dr.GetInt32("Year");
                    c1.Year = Convert.ToInt32(dr["Year"]);
                    c1.IsAvailable = dr.GetBoolean("isAvailable");
                    Elist.Add(c1);
                }
                Console.WriteLine("fetched");
                // return Elist;
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.Message);
            }
            finally
            {

                conn.Close();

            }
            // return null;

            Console.WriteLine("\nCar Inventory:");
            // for (int i = 0; i < carInventory.Count; i++)
            //foreach (Car c in Elist)
            //{
            //    Console.WriteLine($"{c.CarId} {c.Brand} {c.Model} {c.Year} - {(c.IsAvailable ? "Available" : "Rented")}");
            //}
            Console.WriteLine("+------+--------------------------------+----------------------+------------+------------+");
            Console.WriteLine("|  ID  |      Brand Name                |    Model             | Year       | IsAvailable|");
            Console.WriteLine("+------+--------------------------------+----------------------+------------+------------+");
            foreach (Car c in Elist)
            {
                Console.WriteLine($"| {c.CarId,-5}| {c.Brand,-5} | {c.Model,-10} | {c.Year,-10} | {(c.IsAvailable ? "Available" : "Rented"),-10} |");
            }

            Console.WriteLine("+------+--------------------------------+----------------------+------------+------------+");


            Console.WriteLine("Press any key to continue.");
            Console.ReadKey();
        }
    

        public static void RentCar()
        {
            Console.Clear();
            Console.WriteLine("Rent a Car");
            Console.WriteLine("-----------");
            ListAvailableCars();

            Console.Write("Enter car ID to rent: ");
            if (int.TryParse(Console.ReadLine(), out int carId))
            {
                if (CarRentalSystem.RentCar(carId))
                {
                    Console.WriteLine("Car rented successfully. Press any key to continue.");
                }
                else
                {
                    Console.WriteLine("Invalid car ID or the car is not available. Press any key to continue.");
                }
                Console.ReadKey();
            }
            else
            {
                Console.WriteLine("Invalid car ID. Press any key to continue.");
                Console.ReadKey();
            }
        }

       public static void ReturnCar()
        {
            Console.Clear();
            Console.WriteLine("Return a Car");
            Console.WriteLine("------------");
            Console.Write("Enter car ID to return: ");
            if (int.TryParse(Console.ReadLine(), out int carId))
            {
                if (CarRentalSystem.ReturnCar(carId))
                {
                    Console.WriteLine("Car returned successfully. Press any key to continue.");
                }
                else
                {
                    Console.WriteLine("Invalid car ID or the car is not rented by you. Press any key to continue.");
                }
                Console.ReadKey();


            }
            else
            {
                Console.WriteLine("Invalid car ID. Press any key to continue.");
                Console.ReadKey();

            }
           
       

        }

        public static void ListRentedCars()
        {
            Console.Clear();
            Console.WriteLine("Rented Cars");
            Console.WriteLine("--------------------------------------");





            SqlConnection conn = SqlConnections.GetConnections();

            List<RentDetailes> Elist = new List<RentDetailes>();
            try
            {
                //  SqlCommand cmd = conn.CreateCommand();

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.Text; // change
                cmd.CommandText = "Select * from Rent_tbl ";     //    change

                // cmd.Parameters.AddWithValue("@EmpNo", EmpNo);

                SqlDataReader dr = cmd.ExecuteReader();


                while (dr.Read())
                {
                    RentDetailes rent = new RentDetailes();
                    
                    rent.CarId= Convert.ToInt32(dr["Car_Id"]);
                    rent.CustmerName = dr.GetString("CustomerName");
                    rent.Address = dr.GetString("Address");
                    rent.MobileNo = dr.GetString("MobileNo");
                    rent.RentalDate = dr.GetDateTime("RentDate");
                   
                    rent.RentalFee= Convert.ToInt32(dr["RentalFee"]); 

                    Elist.Add(rent);
                }
                Console.WriteLine("fetched");
                
                // return Elist;
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.Message);
            }
            finally
            {

                conn.Close();

            }
            // return null;

            Console.WriteLine("\nCar Inventory:");
            // for (int i = 0; i < carInventory.Count; i++)
            //foreach (Car c in Elist)
            //{
            //    Console.WriteLine($"{c.CarId} {c.Brand} {c.Model} {c.Year} - {(c.IsAvailable ? "Available" : "Rented")}");
            //}
            Console.WriteLine("+------+-----------------+-----------------+------------+---------------------+------------+");
            Console.WriteLine("|  ID  | Customer Name   |    Address      | Mobile No  | Rent Date           | RentalFee  |");
            Console.WriteLine("+------+-----------------+-----------------+------------+---------------------+------------+");
            foreach (RentDetailes c in Elist)
            {
                Console.WriteLine($"| {c.CarId,-5}| {c.CustmerName,0} | {c.Address,-10} | {c.MobileNo,-10} | {c.RentalDate,-10} | {c.RentalFee,-10} |");
               // Console.WriteLine(String.Format("{0,-5} {1,-20} {2, -10} {3, -10}{4, -10}{5, -10} \n", c.CarId, c.CustmerName, c.Address, c.MobileNo, c.RentalDate, c.RentalFee)); 
            }
            Console.WriteLine("+------+-----------------+-----------------+------------+---------------------+------------+");


            Console.WriteLine("Press any key to continue.");
            Console.ReadKey();
        }
    }
}
