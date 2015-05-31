using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//..
using System.Data;
using System.Data.SqlClient;
using ETS.logic;
using ETS.logic.Domain;

namespace ETS.data
{
    public class LoginDao
    {
        // Set connection string
        private static SqlConnection myConnection;
        public static void SetCon(string conn)
        {
            myConnection = new SqlConnection(conn);
        }

        // Example of SQL Injection
        public static int LoginSQLInjection(string userName, string password)
        {
            try
            {
                // Open the SQL connection
                myConnection.Open();

                // Create a new SQL command using a REALLY BAD SQL query
                SqlCommand search = new SqlCommand("SELECT * FROM Passwords WHERE UserName = '"
                                                    + userName + "' and Password = '"
                                                    + password + "'", myConnection);

                // Execture query
                SqlDataReader myReader = search.ExecuteReader();

                // Check myReader has login info
                if (myReader.HasRows)
                {
                    myConnection.Close();
                    return (int)SuccessEnum.Success;
                }
                else
                {
                    myConnection.Close();
                    return (int)SuccessEnum.Fail;
                }
            }
            catch (Exception)
            {
                myConnection.Close();
                return (int)SuccessEnum.Fail;
            }
        }
        public static int NewLogin(LoginUser user)
        {
            // Open the SQL connection
            myConnection.Open();

            // Create a new SQL command using the stored procedure
            SqlCommand add = new SqlCommand("dbo.sp_Passwords_AddNewLogin", myConnection);
            // Set commandtype to store procedure
            add.CommandType = CommandType.StoredProcedure;
            // Add parameters to command
            add.Parameters.AddWithValue("@userName", user.UserName);
            add.Parameters.AddWithValue("@hash", user.PasswordHash);
            add.Parameters.AddWithValue("@salt", user.SaltHash);

            // Execture store procedure
            add.ExecuteNonQuery();

            // Close the connection (VERY important!)
            myConnection.Close();
            return (int)SuccessEnum.Success;
        }

        public static int UpdatePassword(LoginUser user)
        {
            // Open the SQL connection
            myConnection.Open();

            // Create a new SQL command using the stored procedure
            SqlCommand add = new SqlCommand("dbo.sp_Passwords_UpdatePassword", myConnection);
            // Set commandtype to store procedure
            add.CommandType = CommandType.StoredProcedure;
            // Add parameters to command
            add.Parameters.AddWithValue("@userName", user.UserName);
            add.Parameters.AddWithValue("@hash", user.PasswordHash);
            add.Parameters.AddWithValue("@salt", user.Salt);

            // Execture store procedure
            add.ExecuteNonQuery();

            // Close the connection (VERY important!)
            myConnection.Close();

            return (int)SuccessEnum.Success;
        }

        public static int SearchUserName(string userName)
        {
            // Open the SQL connection
            myConnection.Open();

            // Create a new SQL command using the stored procedure
            SqlCommand search = new SqlCommand("dbo.sp_Passwords_SearchUserName", myConnection);
            // Set commandtype to store procedure
            search.CommandType = CommandType.StoredProcedure;
            // Add parameters to command
            search.Parameters.AddWithValue("@userName", userName);

            // Execture store procedure
            SqlDataReader myReader = search.ExecuteReader();

            // Check myReader has login info
            if (myReader.HasRows)
            {
                myConnection.Close();
                return (int)SuccessEnum.Fail;
            }
            else
            {
                myConnection.Close();
                return (int)SuccessEnum.Success;
            }
        }
        public static string GetSalt(string userName)
        {
            // Open the SQL connection
            myConnection.Open();

            // Create a new SQL command using the stored procedure
            SqlCommand add = new SqlCommand("dbo.sp_Passwords_GetSalt", myConnection);
            // Set commandtype to store procedure
            add.CommandType = CommandType.StoredProcedure;
            // Add parameters to command
            add.Parameters.AddWithValue("@userName", userName);

            // Execture query
            SqlDataReader myReader = add.ExecuteReader();
            string salt = "";
            // Check myReader has login info
            if (myReader.HasRows)
            {
                while (myReader.Read())
                {
                    salt = (myReader["Salt"]).ToString();
                }
                myConnection.Close();
                return salt;
            }

            // Close the connection (VERY important!)
            myConnection.Close();
            return "Didn't get salt!";
        }
        public static int Login(LoginUser user)
        {
            // Open the SQL connection
            myConnection.Open();

            // Create a new SQL command using the stored procedure
            SqlCommand search = new SqlCommand("dbo.sp_Passwords_CheckLogin", myConnection);
            // Set commandtype to store procedure
            search.CommandType = CommandType.StoredProcedure;
            // Add parameters to command
            search.Parameters.AddWithValue("@userName", user.UserName);
            search.Parameters.AddWithValue("@hash", user.PasswordHash);

            // Execture store procedure
            SqlDataReader myReader = search.ExecuteReader();

            // Check myReader has login info
            if (myReader.HasRows)
            {
                myConnection.Close();
                return (int)SuccessEnum.Success;
            }
            else
            {
                myConnection.Close();
                return (int)SuccessEnum.Fail;
            }
        }
    }
}
