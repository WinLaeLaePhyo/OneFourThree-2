using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;

namespace OneFourThree.App_Code
{
    public class DBFront
    {
        string connectionString = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
        DataTable dt;

        public Boolean CheckByQuery(string query)
        {
            Boolean name = false;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        if (reader["ID"] != DBNull.Value)
                        {
                            name = true;
                        }
                    }
                }
            }
            return name;
        }
        public Boolean CheckByQueryParameter(string query, int Count, ParameterBack p)
        {
            string Variable = "@Parameter";
            Boolean name = false;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    for (int i = 1; i <= Count; i++)
                    {
                        command.Parameters.AddWithValue(Variable + i, p.getParameter(i));
                    }
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        if (reader["ID"] != DBNull.Value)
                        {
                            name = true;
                        }
                    }
                }
            }
            return name;
        }
        public void ChangeByQuery(string query)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
        }
        public void ChangeByQueryParameter(string query, int Count, ParameterBack p)
        {
            string Variable = "@Parameter";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    con.Open();
                    for (int i = 1; i <= Count; i++)
                    {
                        cmd.Parameters.AddWithValue(Variable + i, p.getParameter(i));
                    }
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
        }

        public int getIntByQueryParameter(string query, string Column, int Count, ParameterBack p)
        {
            int name = 0; string Variable = "@Parameter";
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    for (int i = 1; i <= Count; i++)
                    {
                        cmd.Parameters.AddWithValue(Variable + i, p.getParameter(i));
                    }
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        cmd.Connection = con;
                        sda.SelectCommand = cmd;
                        using (dt = new DataTable())
                        {
                            sda.Fill(dt);
                        }
                    }
                }
            }
            foreach (DataRow row in dt.Rows)
            {
                name = Convert.ToInt32(row[Column]);
            }
            return name;
        }

        public DataTable getAllByQuery(string query)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        cmd.Connection = con;
                        sda.SelectCommand = cmd;
                        using (DataTable dt = new DataTable())
                        {
                            sda.Fill(dt);
                            return dt;
                        }
                    }
                }
            }
        }


        public string getStringByQuery(string query, string Column)
        {
            string name = "";
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        cmd.Connection = con;
                        sda.SelectCommand = cmd;
                        using (dt = new DataTable())
                        {
                            sda.Fill(dt);
                        }
                    }
                }
            }
            foreach (DataRow row in dt.Rows)
            {
                name = row[Column].ToString();
            }
            return name;
        }

        public DateTime getDateByQuery(string query, string Column)
        {
            DateTime name = DateTime.Now;
            DataTable dt;
            var connectionString = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        cmd.Connection = con;
                        sda.SelectCommand = cmd;
                        using (dt = new DataTable())
                        {
                            sda.Fill(dt);
                        }
                    }
                }
            }
            foreach (DataRow row in dt.Rows)
            {
                name = Convert.ToDateTime(row[Column]);
            }
            return name;
        }

        public int getIntByQuery(string query, string Column)
        {
            int name = 0;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        cmd.Connection = con;
                        sda.SelectCommand = cmd;
                        using (dt = new DataTable())
                        {
                            sda.Fill(dt);
                        }
                    }
                }
            }
            foreach (DataRow row in dt.Rows)
            {
                name = Convert.ToInt32(row[Column]);
            }
            return name;
        }
        public float getFloatByQuery(string query, string Column)
        {
            float name = 0;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        cmd.Connection = con;
                        sda.SelectCommand = cmd;
                        using (dt = new DataTable())
                        {
                            sda.Fill(dt);
                        }
                    }
                }
            }
            foreach (DataRow row in dt.Rows)
            {
                name = Convert.ToInt32(row[Column]);
            }
            return name;
        }

        public Boolean getBooleanByQuery(string query, string Column)
        {
            Boolean name = false;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        cmd.Connection = con;
                        sda.SelectCommand = cmd;
                        using (dt = new DataTable())
                        {
                            sda.Fill(dt);
                        }
                    }
                }
            }
            foreach (DataRow row in dt.Rows)
            {
                name = Convert.ToBoolean(row[Column]);
            }
            return name;
        }

        //For total Number of a column
        public int getIntTotalByQuery(string query, string Column)
        {
            int name = 0;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        cmd.Connection = con;
                        sda.SelectCommand = cmd;
                        using (dt = new DataTable())
                        {
                            sda.Fill(dt);
                        }
                    }
                }
            }
            foreach (DataRow row in dt.Rows)
            {
                name = name + Convert.ToInt32(row[Column]);
            }
            return name;
        }

        public decimal getDecimalTotalByQuery(string query, string Column)
        {
            decimal name = 0;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        cmd.Connection = con;
                        sda.SelectCommand = cmd;
                        using (dt = new DataTable())
                        {
                            sda.Fill(dt);
                        }
                    }
                }
            }
            foreach (DataRow row in dt.Rows)
            {
                name = name + Convert.ToDecimal(row[Column]);
            }
            return name;
        }

        public int getCountByQuery(string query)
        {
            int name = 0;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        cmd.Connection = con;
                        sda.SelectCommand = cmd;
                        using (dt = new DataTable())
                        {
                            sda.Fill(dt);
                        }
                    }
                }
            }
            foreach (DataRow row in dt.Rows)
            {
                name = name + 1;
            }
            return name;
        }
    }
}