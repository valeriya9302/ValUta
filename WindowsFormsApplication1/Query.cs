using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using System.Data.SqlClient;

namespace MainWindow
{
    class Query
    {
        //private static string _connectionString = ConfigurationManager.ConnectionStrings["elementsConnectionString"].ToString();
        private static string _connectionString = "Data Source=VALERIA-PC\\SQLEXPRESS;Initial Catalog=basic_elements;Integrated Security=True";
        
        public static List<string> SendQuerySelect(string querySring)
        {
            var response = new List<string>();
            using (var connection = new SqlConnection(_connectionString))
            {
                var comand = new SqlCommand(querySring, connection);
                try
                {
                    connection.Open();
                    var reader = comand.ExecuteReader();
                    while (reader.Read())
                    {
                        for (var i = 0; i < reader.FieldCount; i++)
                        {
                            //Console.Write(reader[i]+"\t");
                            response.Add(reader[i].ToString());
                        }
                        //Console.WriteLine();
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message + querySring);
                    //throw;
                }
                return response;
            }
        }

        public static int SendQueryInsert(string querySring)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var response = 0;
                var comand = new SqlCommand(querySring, connection);
                response = comand.ExecuteNonQuery();
                return response;
            }
        }
    }
}
