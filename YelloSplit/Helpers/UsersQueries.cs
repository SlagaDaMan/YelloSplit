using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;

using Newtonsoft.Json;
using System.IO;
using System.Net;
using System.Data.SqlClient;

namespace YelloSplit.Helpers
{
    public class UsersQueries
    {
        public DataTable ExecuteQueryFunction(string Query)
        {
            string connectionString = "Data Source=ACEENDS\\SQLEXPRESS;Initial Catalog=YelloSplit;Integrated Security=True";
           // string dd = Properties.Settings.Default.OfficeManagementSystemConnectionString.ToString();

           // string connectionString = "Data Source=SQL6010.site4now.net;Initial Catalog=DB_A62B58_eazybill;User Id=DB_A62B58_eazybill_admin;Password=Password@1;";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(Query, connection);
                //command.Parameters.AddWithValue("@tPatSName", "Your-Parm-Value");
                connection.Open();
                var dataReader = command.ExecuteReader();
                var dataTable = new DataTable();
                dataTable.Load(dataReader);

                return dataTable;
            }
        }
    }
}
