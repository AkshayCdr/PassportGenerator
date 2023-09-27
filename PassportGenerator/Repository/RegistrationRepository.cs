using PassportGenerator.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;

namespace PassportGenerator.Repository
{
    public class RegistrationRepository
    {
        SqlConnection connectionLink = new SqlConnection(ConfigurationManager.ConnectionStrings["cs"].ConnectionString);
        SqlCommand command;
        SqlDataAdapter adapter;
        DataTable table;

        /// <summary>
        /// To get data of registered users
        /// Registration table
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
                        FirstName = table["FirstName"].ToString(),
                        LastName = table["LastName"].ToString(),
                        Dob = DateTime.Parse(table["Dob"].ToString()),
                        Age = Convert.ToInt32(table["Age"]),
                        Gender = table["Gender"].ToString(),
                        Phone = table["Phone"].ToString(),
                        Email = table["Email"].ToString(),
                        State = table["State"].ToString(),
                        City = table["City"].ToString(),
                        Username = table["Username"].ToString(),
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
        /// To return registration id 
        /// using email 
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public int findRegistrationId(string email)
        {
            try
            {
                connectionLink.Open();
                SqlCommand command = new SqlCommand("SPS_RegistrationId", connectionLink);      
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Email", email);
                int id = (int)command.ExecuteScalar();
                return id;
            }
            catch (Exception)
            {

                throw;
            }
            finally { connectionLink.Close(); }

        }

        /// <summary>
        /// To inset user details in
        /// Registraion table
        /// </summary>
        /// <param name="registration"></param>
        /// <returns></returns>
        public bool InsertUser(Registration registration)
        {
            try
            {
                command = new SqlCommand("SPI_Registrations", connectionLink);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@firstname", registration.FirstName);
                command.Parameters.AddWithValue("@Lastname", registration.LastName);
                command.Parameters.AddWithValue("@dob", registration.Dob);
                command.Parameters.AddWithValue("@age", registration.Age);
                command.Parameters.AddWithValue("@gender", registration.Gender);
                command.Parameters.AddWithValue("@phone", registration.Phone);
                command.Parameters.AddWithValue("@email", registration.Email);
                command.Parameters.AddWithValue("@state", registration.State);
                command.Parameters.AddWithValue("@city", registration.City);
                command.Parameters.AddWithValue("@username", registration.Username);
                command.Parameters.AddWithValue("@password", registration.Password);

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
        /// To update user details in registration
        /// table
        /// </summary>
        /// <param name="registration"></param>
        /// <returns></returns>
        public bool UpdateUser(Registration registration)
        {
            try
            {
                command = new SqlCommand("SPU_Registrations", connectionLink);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@firstname", registration.FirstName);
                command.Parameters.AddWithValue("@lastname", registration.LastName);
                command.Parameters.AddWithValue("@dob", registration.Dob);
                command.Parameters.AddWithValue("@age", registration.Age);
                command.Parameters.AddWithValue("@gender", registration.Gender);
                command.Parameters.AddWithValue("@phone", registration.Phone);
                command.Parameters.AddWithValue("@email", registration.Email);
                command.Parameters.AddWithValue("@state", registration.State);
                command.Parameters.AddWithValue("@city", registration.City);
                command.Parameters.AddWithValue("@username", registration.Username);
                command.Parameters.AddWithValue("@password", registration.Password);
                command.Parameters.AddWithValue("@id", registration.Id);

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
        /// To delete the user data from 
        /// the table using id 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int DeleteUser(int id)
        {
            try
            {
                command = new SqlCommand("SPD_Registrations", connectionLink);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@id", id);

                connectionLink.Open();
                return command.ExecuteNonQuery();
            }
            catch (Exception)
            {

                throw;
            }
            finally { connectionLink.Close(); }
        }

        /// <summary>
        /// To insert data into User role table
        /// User role talble contains user roles
        /// </summary>
        /// <param name="userRole"></param>
        /// <returns></returns>
        public bool InsertIntoRole(UserRole userRole)
        {
            try
            {
                command = new SqlCommand("SPI_Roles", connectionLink);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@UserId", userRole.UserId);
                command.Parameters.AddWithValue("@UserRole", userRole.Role);
                connectionLink.Open();
                int r = command.ExecuteNonQuery();
                if (r > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {

                throw;
            }
            finally { connectionLink.Close(); }
        }

        /// <summary>
        /// To apply admin role
        /// insert admin role to the user by adding
        /// user role admin to database
        /// </summary>
        /// <param name="userRole"></param>
        /// <returns></returns>
        public bool InsertIntoRoleAdmin(UserRole userRole)
        {
            try
            {
                command = new SqlCommand("SPI_Roles", connectionLink);
                command.CommandType = CommandType.Text;
                command.Parameters.AddWithValue("@UserId", userRole.UserId);
                command.Parameters.AddWithValue("@UserRole", userRole.Role);
                connectionLink.Open();
                int r = command.ExecuteNonQuery();
                if (r > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {

                throw;
            }
            finally { connectionLink.Close(); }
        }

        /// <summary>
        /// Retrive data about user and roles
        /// from registation table and user roles
        /// </summary>
        /// <returns></returns>
        public List<RegistrationRoles> GetUsersAndRoles()
        {
            try
            {
                command = new SqlCommand("SPS_RolesRegistration", connectionLink);
                command.CommandType = CommandType.StoredProcedure;
                adapter = new SqlDataAdapter(command);
                table = new DataTable();
                adapter.Fill(table);
                List<RegistrationRoles> list = new List<RegistrationRoles>();
                foreach (DataRow table in table.Rows)
                {
                    list.Add(new RegistrationRoles
                    {
                        Id = Convert.ToInt32(table["Id"]),
                        FirstName = table["FirstName"].ToString(),
                        LastName = table["LastName"].ToString(),
                        Dob = DateTime.Parse(table["Dob"].ToString()),
                        Age = Convert.ToInt32(table["Age"]),
                        Gender = table["Gender"].ToString(),
                        Phone = table["Phone"].ToString(),
                        Email = table["Email"].ToString(),
                        State = table["State"].ToString(),
                        City = table["City"].ToString(),
                        Username = table["Username"].ToString(),
                        Password = table["Password"].ToString(),
                        Role = table["Role"].ToString()

                    }) ;
                }
                return list;
            }
            catch(Exception)
            {
                throw;
            }
            finally { connectionLink.Close(); }
        }

        /// <summary>
        /// To retrieve id from the database using
        /// the email 
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public int GetUserIdFromEmail(string email)
        {

            try
            {
                connectionLink.Open();
                command = new SqlCommand("SPS_RegistrationId", connectionLink);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Email", email);
                int result = (int)command.ExecuteScalar();
                return result;
            }
            catch (Exception)
            {

                throw;
            }
            finally { connectionLink.Close(); }
        }


        /// <summary>
        /// To check wheather user role is alerady 
        /// presented and retrieve if present 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public UserRole CheckUserIdUserRole(int id)
        {
            try
            {
                connectionLink.Open();
                command = new SqlCommand("SPS_Roles", connectionLink);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@id", id);

                UserRole userRole = null;

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        int userIdIndex = reader.GetOrdinal("RegistrationId");
                        int roleIndex = reader.GetOrdinal("Role");

                        userRole = new UserRole
                        {
                            UserId = reader.GetInt32(userIdIndex), 
                            Role = reader.GetString(roleIndex)     
                        };
                    }
                }
                return userRole;
            }
            catch (Exception)
            {
                throw;
            }
            finally { connectionLink.Close(); }
        }

        /// <summary>
        /// Remove admin previlage of the user
        /// Remove the admin role of the user
        /// delete the role data 
        /// </summary>
        /// <param name="userrole"></param>
        public void removeRole(UserRole userrole)
        {
            try
            {

                connectionLink.Open();

                command = new SqlCommand("SPD_Roles", connectionLink);

                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@userid", userrole.UserId);

                int result = command.ExecuteNonQuery();
            }
            catch (Exception)
            {

                throw;
            }
            finally { connectionLink.Close(); }
        }
    }
}