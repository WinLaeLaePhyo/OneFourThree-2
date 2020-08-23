using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data.SqlClient;

namespace OneFourThree.App_Code
{
    public class Authentication
    {
        int LoginID;
        int UserID;
        int AccessLevel;
        
        public Boolean checkUser(string Phone, string Pin)
        {
            Boolean isUser = false;
            var connectionString = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("SELECT * FROM Login WHERE convert(varchar(MAX),DECRYPTBYPASSPHRASE('root@',Phone))= @Phone and convert(varchar(MAX),DECRYPTBYPASSPHRASE('root@',Pin)) = @Pin and Active='True'", connection))
                {
                    command.Parameters.AddWithValue("@Phone", Phone);
                    command.Parameters.AddWithValue("@Pin", Pin);

                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        if (reader["ID"] != DBNull.Value)
                        {
                            LoginID = Convert.ToInt32(reader["ID"]);
                            UserID = Convert.ToInt32(reader["AllID"]);
                            AccessLevel = Convert.ToInt32(reader["AccessLevel"]);
                            isUser = true;
                        }
                    }
                }
            }
            return isUser;
        }
        public int getLoginID()
        {
            return LoginID;
        }
        public int getUserID()
        {
            return UserID;
        }
        public int getAccessLevel()
        {
            return AccessLevel;
        }
    }
}