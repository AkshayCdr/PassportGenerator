using PassportGenerator.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Security.Cryptography;
using System.Text;

namespace PassportGenerator.Repository
{
    public class LoginRepository
    {
        SqlConnection connectionLink = new SqlConnection(ConfigurationManager.ConnectionStrings["cs"].ConnectionString);

        SqlCommand command;

        SqlDataAdapter adapter;

        DataTable table;

        /// <summary>
        /// Get details from the Registration table
        /// to check wheather user is registered or 
        /// not 
        /// </summary>
        /// <returns></returns>
        public List<Registration> GetUsers()
        {
            try
            {
                command = new SqlCommand("SPS_Registrations", connectionLink);
                command.CommandType = CommandType.StoredProcedure;
                adapter = new SqlDataAdapter(command);
                table = new DataTable();
                adapter.Fill(table);
                List<Registration> list = new List<Registration>();
                foreach (DataRow table in table.Rows)
                {
                    list.Add(new Registration
                    {
                        Id = Convert.ToInt32(table["Id"]),
                        Email = table["Email"].ToString(),
                        Password = table["Password"].ToString(),
                    });
                }
                return list;
            }
            catch (Exception)
            {

                throw;
            }
            finally { connectionLink.Close(); }
        }

        /// <summary>
        /// Change password of the user
        /// in case of forget password 
        /// </summary>
        /// <param name="registration"></param>
        /// <returns></returns>
        public bool ChangePassword(Registration registration)
        {
            try
            {
                command = new SqlCommand(" update Registrations set Password = @password where Email = @email ;", connectionLink);
                command.CommandType = CommandType.Text;
                command.Parameters.AddWithValue("@email", registration.Email);
                //command.Parameters.AddWithValue("@password", registration.Password);

                //To hash password 
                string password = registration.Password;
                //string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);
                string hashedPassword = HashString(password);

                command.Parameters.AddWithValue("@password", hashedPassword);
                connectionLink.Open();
                int r = command.ExecuteNonQuery();
                if (r > 0)
                {
                    return true;
                }
                else
                    return false;
            }
            catch (Exception)
            {

                throw;
            }
            finally { connectionLink.Close(); }
        }


        /// <summary>
        /// To hash Password into SHA-256
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        static string HashString(string input)
        {
            // Convert the input string to bytes
            byte[] inputBytes = Encoding.UTF8.GetBytes(input);

            using (SHA256 sha256 = SHA256.Create())
            {
                // Compute the hash value from the input bytes
                byte[] hashBytes = sha256.ComputeHash(inputBytes);

                // Convert the byte array to a hexadecimal string
                string hashString = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();

                return hashString;
            }
        }
    }
}