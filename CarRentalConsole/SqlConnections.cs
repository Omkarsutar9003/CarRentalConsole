using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalConsole
{
    internal class SqlConnections
    {
        public static SqlConnection GetConnections()
        {
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = @"Data Source =(localdb)\MSSQLLocalDB; Initial Catalog = Omkar_MS; Integrated Security = True; Connect Timeout = 30; Encrypt = False";
            conn.Open();
            return conn;

        }
    }
}
