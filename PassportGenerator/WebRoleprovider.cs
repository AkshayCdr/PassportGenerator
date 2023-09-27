using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Security;

namespace PassportGenerator
{
    public class WebRoleprovider : RoleProvider
    {
        public override string ApplicationName { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            throw new NotImplementedException();
        }

        //public override string[] GetRolesForUser(string username)
        //{
        //    throw new NotImplementedException();
        //}

        public override string[] GetRolesForUser(string email)
        {
            List<string> userRoles = new List<string>();

            // Your connection string
            string connectionString = "Data Source= DESKTOP-10MH51F\\SQLEXPRESS; Integrated Security=true;Initial Catalog= Passport; ";


            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Define your SQL query
                //    string sqlQuery = @"
                //SELECT ur.Role
                //FROM Registration r
                //INNER JOIN UserRole ur ON r.id = ur.UserId
                //WHERE r.Email = @email";


//                string sqlQuery = @"SELECT Roles.Role
//FROM Registrations
//INNER JOIN Roles ON Registrations.id = Roles.RegistrationId
//WHERE Registrations.Email = @email";
                using (SqlCommand command = new SqlCommand("SPS_RolesEmail", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    // Add a parameter for the username
                    command.Parameters.AddWithValue("@Email", email);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // Retrieve the Role from the result set
                            string roleName = reader.GetString(0);
                            userRoles.Add(roleName);
                        }
                    }
                }
            }

            return userRoles.ToArray();
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            throw new NotImplementedException();
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }
    }
}