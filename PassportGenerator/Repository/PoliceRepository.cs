using PassportGenerator.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace PassportGenerator.Repository
{
    public class PoliceRepository
    {
        SqlConnection connectionLink = new SqlConnection(ConfigurationManager.ConnectionStrings["cs"].ConnectionString);
        SqlCommand command;
        SqlDataAdapter adapter;
        DataTable table;

        /// <summary>
        /// To get the data of passport applications 
        /// in list 
        /// </summary>
        /// <returns></returns>
        public List<Police> GetPassportApplications()
        {
            try
            {
                command = new SqlCommand("SPS_Police", connectionLink);
                command.CommandType = CommandType.StoredProcedure;
                adapter = new SqlDataAdapter(command);
                table = new DataTable();
                adapter.Fill(table);
                List<Police> list = new List<Police>();
                foreach (DataRow table in table.Rows)
                {
                    list.Add(new Police()
                    {
                        RegistrationId = (int)table["RegistrationId"],
                        FirstName = table["FirstName"].ToString(),
                        LastName = table["LastName"].ToString(),
                        Phone = table["Phone"].ToString(),
                        State = table["State"].ToString(),
                        City = table["City"].ToString(),
                        PassportOfficeName = table["PassportOfficeName"].ToString(),
                        PhotoBytes = (byte[])table["Photo"],
                        IdentityProofBytes = (byte[])table["IdentityProof"],
                        BirthProofBytes = (byte[])table["BirthProof"],
                        NationalityProofBytes = (byte[])table["NationalityProof"],
                        //SignatureBytes = (byte[])table["Signature"]
                        SignatureBytes = table["Signature"] == DBNull.Value ? null : (byte[])table["Signature"]
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
        /// To Update the status field and
        /// Set police approval 
        /// </summary>
        /// <param name="id"></param>
        public bool PoliceStatusApprove(int id)
        {
            //Update status policeApproval where registratoin id
            try
            {
                command = new SqlCommand("SPU_StatusPolice", connectionLink);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@registrationId", id);
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
        /// To check wheather it is already approved by admin
        /// </summary>
        public bool CheckAdminApprovalStatus(int id)
        {
            try
            {
                command = new SqlCommand("SPS_StatusId", connectionLink);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@registrationId", id);
                connectionLink.Open();
                object r  = command.ExecuteScalar();
                if(r == null)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            finally
            {
                connectionLink.Close();
            }
            

        }


        /// <summary>
        /// To check wheather data is 
        /// approved by police or not
        /// </summary>
        public bool CheckPoliceApproval(int id)
        {
            try
            {
                command = new SqlCommand("SPS_StatusPoliceApp", connectionLink);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@registrationId", id);
                connectionLink.Open();
                string value = command.ExecuteScalar().ToString();
                if (value == "Approved")
                {
                    return true;
                }
                return false;
            }
            finally
            {
                connectionLink.Close();
            }

        }

        /// <summary>
        /// To get the list of applications 
        /// which are approved by the police 
        /// </summary>
        public List<Police> ListOfPoliceApprovedApp()
        {
            try
            {
                command = new SqlCommand("SPS_PoliceAdmin", connectionLink);
                command.CommandType = CommandType.StoredProcedure;
                adapter = new SqlDataAdapter(command);
                table = new DataTable();
                adapter.Fill(table);
                List<Police> list = new List<Police>();
                foreach (DataRow table in table.Rows)
                {
                    list.Add(new Police()
                    {
                        FirstName = table["FirstName"].ToString(),
                        LastName = table["LastName"].ToString(),
                        AdminApproveStatus = table["AdminApproval"].ToString(),
                        PoliceApproveStatus = table["PoliceApproval"] == DBNull.Value ? null: table["PoliceApproval"].ToString(),
                        Generated = table["Status"].ToString(),
                        //PoliceApproveDate = table["PolAppDate"] == DBNull.Value ? null : DateTime.Parse(table["PolAppDate"].ToString())
                        PoliceApproveDate = table["PolAppDate"] == DBNull.Value ? (DateTime?)null : DateTime.Parse(table["PolAppDate"].ToString()).Date,
                        //PoliceApproveDate = table["PolAppDate"] == DBNull.Value ? null : table["PolAppDate"].ToString(),
                    });
                }
                return list;

                //firtname
                //Lastname
                //admin approval
                //police approval
                //wheather passport generated or not 
                //if generated date of generation and  
            }
            finally
            {
                connectionLink.Close();
            }
        }
    }
}