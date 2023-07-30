using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalConsole
{
    internal class User
    {
        public string Username { get; set; }
        public string Password { get; set; }


      public  static void RegisterNewUser()
        {
            Console.Clear();
            Console.WriteLine("----- Register New User -----");

            Console.Write("Enter new username: ");
            string newUsername = Console.ReadLine();

            if (!ValidateUsername(newUsername))
            {
                Console.WriteLine("Invalid username. Username should contain only letters and digits.");
                Console.ReadLine();

            }
            else
            {
                Console.Write("Enter new password: ");
                string newPassword = Console.ReadLine();
                Console.Write("Enter new Email: ");
                string newEmail = Console.ReadLine();
                //DB------------------------------------------------------
                SqlConnection conn = SqlConnections.GetConnections();
                try
                {
                  
                SqlCommand cmd = conn.CreateCommand();

                cmd.Connection = conn;
                cmd.CommandType = CommandType.Text; // change
                  
                    cmd.CommandText = "INSERT INTO Users (Username,password,email) VALUES (@newUsername, @newPassword, @newEmail)";     //    change
               ;
                    
               
                cmd.Parameters.AddWithValue("@newUsername", newUsername);
                cmd.Parameters.AddWithValue("@newPassword", newPassword);
                cmd.Parameters.AddWithValue("@newEmail", newEmail);
        

                    cmd.ExecuteNonQuery();
                 
                    Console.WriteLine("User registration successful!");
                }
                catch (Exception Ex)
                {
                    Console.WriteLine(Ex.Message);
                }
                finally
                {

                    conn.Close();
                }
            }
            
        }

        static bool ValidateUsername(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                return false;

            foreach (char c in username)
            {
                if (!char.IsLetterOrDigit(c))
                    return false;
            }

            return true;
        }

    }
    
}