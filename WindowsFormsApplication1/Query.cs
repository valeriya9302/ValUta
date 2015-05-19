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
        //test
        //private static string _connectionString = ConfigurationManager.ConnectionStrings["elementsConnectionString"].ToString();
        //private static string _connectionString = "Data Source=VALERIA-PC\\SQLEXPRESS;Initial Catalog=basic_elements;Integrated Security=True";
        private static string _connectionString = ConfigurationManager.ConnectionStrings["basic_elementsConnectionString"].ToString();
        
        public static List<List<string>> SendQuerySelect(string querySring)
        {
            var response = new List<List<string>>();
            using (var connection = new SqlConnection(_connectionString))
            {
                var comand = new SqlCommand(querySring, connection);
                try
                {
                    connection.Open();
                    var reader = comand.ExecuteReader();
                    while (reader.Read())
                    {
                        List<string> tresponse = new List<string>();
                        for (var i = 0; i < reader.FieldCount; i++)
                        {
                            //Console.Write(reader[i]+"\t");
                            //response.Add(reader[i].ToString());
                            tresponse.Add(reader[i].ToString());
                        }
                        if (tresponse.Count > 0)
                            response.Add(tresponse);
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

        public static List<string> getColNames(string tname)
        {
            var response = new List<string>();
            string querySring = "SELECT TOP 1 * FROM [" + tname + "]";
            using (var connection = new SqlConnection(_connectionString))
            {
                var comand = new SqlCommand(querySring, connection);
                try
                {
                    connection.Open();
                    var reader = comand.ExecuteReader();
                    if (reader.Read())
                    {
                        for (var i = 0; i < reader.FieldCount; i++)
                        {
                            //Console.Write(reader[i]+"\t");
                            //response.Add(reader[i].ToString());
                            response.Add(reader.GetName(i));
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
