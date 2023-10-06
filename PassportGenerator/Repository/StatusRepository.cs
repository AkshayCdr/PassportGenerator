using PassportGenerator.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Security.Permissions;
using Antlr.Runtime.Tree;

namespace PassportGenerator.Repository
{

    public class StatusRepository
    {

        SqlConnection connectionLink = new SqlConnection(ConfigurationManager.ConnectionStrings["cs"].ConnectionString);
        SqlCommand command;
        SqlDataAdapter adapter;
        DataTable table;



        /// <summary>
        /// To get status of the 
        /// users who are applied to passport 
        /// get data from status table
        /// </summary>
        /// <returns></returns>
        public List<Status> GetDetils()
        {
            command = new SqlCommand("SPS_StatusRegistration", connectionLink);
            command.CommandType = CommandType.Text;
            List<Status> statusList = new List<Status>();      
            connectionLink.Open();
            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    Status status = new Status
                    {
                        //Id = reader.GetInt32(reader.GetOrdinal("Id")),
                        //Email = reader.GetString(reader.GetOrdinal("Email")),
                        StatusName = reader.GetString(reader.GetOrdinal("Status")),
                        RegistrationId = reader.GetInt32(reader.GetOrdinal("RegistrationId"))
                    };

                    Registration registration = new Registration
                    {
                        //Id = status.RegistrationId, // Use the RegistrationId from the Status table
                        //Id = reader.GetInt32(reader.GetOrdinal("Id")),
                        FirstName = reader.GetString(reader.GetOrdinal("FirstName")), // Assuming FirstName is in the result
                                                                                      //LastName = reader.GetString(reader.GetOrdinal("LastName")), // Assuming LastName is in the result
                        Email = reader.GetString(reader.GetOrdinal("Email")),                                                         // Add the remaining properties here
                    };

                    status.Registration = registration; // Assign the Registration object to the Status
                    statusList.Add(status);
                }
            }
            return statusList;
        }


       


        /// <summary>
        /// Getting Passport application details
        /// using email retreive registraion id 
        /// and getting name and status of user
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>

        public List<Status> GetDetails(string email)
        {
            List<Status> statusList = new List<Status>();

            // Find the RegistrationID associated with the provided email
            using (connectionLink)
            {
                connectionLink.Open();

                using (SqlCommand command = new SqlCommand("SPS_RegistrationId", connectionLink))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Email", email);

                    int registrationId = (int?)command.ExecuteScalar() ?? 0; 

                    
                    using (SqlCommand statusCommand = new SqlCommand("SPS_StatusRegistrationUsingRegistrationId", connectionLink))
                    {
                        statusCommand.CommandType = CommandType.StoredProcedure;
                        statusCommand.Parameters.AddWithValue("@RegistrationId", registrationId);

                        using (SqlDataReader reader = statusCommand.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Status status = new Status
                                {
                                    //Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                    //Email = reader.GetString(reader.GetOrdinal("Email")),
                                    StatusName = reader.GetString(reader.GetOrdinal("Status")),
                                    RegistrationId = reader.GetInt32(reader.GetOrdinal("RegistrationId")),
                                    PoliceApproval = reader.GetString(reader.GetOrdinal("PoliceApproval"))
                                    //RegistrationId = registrationId
                                };

                                Registration registration = new Registration
                                {
                                    //Id = status.RegistrationId, // Use the RegistrationId from the Status table
                                    FirstName = reader.GetString(reader.GetOrdinal("FirstName")), // Assuming FirstName is in the result
                                    //LastName = reader.GetString(reader.GetOrdinal("LastName")), // Assuming LastName is in the result
                                     Email = reader.GetString(reader.GetOrdinal("Email")),                                                         // Add the remaining properties here
                                };

                                status.Registration = registration; // Assign the Registration object to the Status

                               

                                statusList.Add(status);
                            }
                        }
                    }
                }
            }

            return statusList;
        }

        /// <summary>
        /// Update the status of the 
        /// passport application of user to 
        /// approved
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool changeStatusToApproved(int id)
        {
            try
            {
                connectionLink.Open();
                command = new SqlCommand("SPU_Status", connectionLink);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@status", "Approved");
                command.Parameters.AddWithValue("@registrationid", id);

                int result = command.ExecuteNonQuery();

                if (result > 0)
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
        /// Change the status of the user to
        /// Pending 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool removeApproval(int id)
        {
            try
            {
                connectionLink.Open();
                command = new SqlCommand("SPU_Status", connectionLink);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@status", "Pending");
                command.Parameters.AddWithValue("@registrationid", id);
                int result = command.ExecuteNonQuery();

                if (result > 0)
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

    }
}