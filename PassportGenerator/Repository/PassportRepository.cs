using PassportGenerator.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.IO;
using System.CodeDom;
using System.Web.Helpers;

namespace PassportGenerator.Repository
{
    public class PassportRepository
    {
        SqlConnection connectionLink = new SqlConnection(ConfigurationManager.ConnectionStrings["cs"].ConnectionString);
        SqlCommand command;
        SqlDataAdapter adapter;
        DataTable table;

        /// <summary>
        /// To get the data from passport data
        /// </summary>
        /// <returns></returns>
        public List<Passport> GetUsers()
        {
            try
            {
                command = new SqlCommand("SPS_Passport", connectionLink);
                command.CommandType = CommandType.StoredProcedure;
                adapter = new SqlDataAdapter(command);
                table = new DataTable();
                adapter.Fill(table);

                List<Passport> list = new List<Passport>();
                foreach (DataRow table in table.Rows)
                {
                    list.Add(
                        new Passport()
                        {
                            Id = (int)table["id"],
                            PassportOfficeNAme = table["PassportOfficeName"].ToString(),
                            PhotoBytes = (byte[])table["Photo"],
                            IdentityProofBytes = (byte[])table["IdentityProof"],
                            BirthProofBytes = (byte[])table["BirthProof"],
                            NationalityProofBytes = (byte[])table["NationalityProof"]
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
        /// To get data from passport data
        /// of individual user 
        /// using the email of the user
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public List<Passport> GetUsers(string email)
        {
            int registrationId = getRegistrationId(email);
            try
            {
                connectionLink.Open();
                command = new SqlCommand("SPS_PassportUsingId", connectionLink);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@RegistrationId", registrationId);
                adapter = new SqlDataAdapter(command);
                table = new DataTable();
                adapter.Fill(table);

                List<Passport> list = new List<Passport>();
                foreach (DataRow table in table.Rows)
                {
                    list.Add(
                        new Passport()
                        {
                            Id = (int)table["id"],
                            PassportOfficeNAme = table["PassportOfficeName"].ToString(),
                            PhotoBytes = (byte[])table["Photo"],
                            IdentityProofBytes = (byte[])table["IdentityProof"],
                            BirthProofBytes = (byte[])table["BirthProof"],
                            NationalityProofBytes = (byte[])table["NationalityProof"]
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
        /// To get Registration id using email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public int getRegistrationId(string email)
        {
            try
            {
                connectionLink.Open();
                command = new SqlCommand("SPS_RegistrationId", connectionLink);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Email", email);

                int registrationId = (int?)command.ExecuteScalar() ?? 0;
           
                return registrationId;
            }
            catch (Exception)
            {
                throw;
            }
            finally { connectionLink.Close(); }
        }

        /// <summary>
        /// To check wheather registration id is present in
        /// the passport data to prevent submission of multiple 
        /// applications
        /// </summary>
        /// <returns></returns>
        public bool checkRegistrationId(int id)
        {
            try
            {
                connectionLink.Open();
                command = new SqlCommand("SPS_PassportRegistrationId", connectionLink);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Id", id);

                //var result = (int)command.ExecuteScalar();
                object result = command.ExecuteScalar();
                if(result == null)
                {
                    return true;
                }
                else { return  false; }       
            }
            catch (Exception)
            {
                throw;
            }
            finally { connectionLink.Close(); }
        }

        /// <summary>
        /// To inser data into passport application
        /// first using email the id is retrieved 
        /// and after that data is inserted
        /// </summary>
        /// <param name="passport"></param>
        /// <returns></returns>
        public bool InsertUsers(Passport passport)
        {
            try
            {
                passport.RegistrationId = getRegistrationId(passport.Email);

                passport.PhotoBytes = ConvertToByteArray(passport.Photo);
                passport.BirthProofBytes = ConvertToByteArray(passport.BirthProof);
                passport.IdentityProofBytes = ConvertToByteArray(passport.IdentityProof);     
                passport.NationalityProofBytes = ConvertToByteArray(passport.NationalityProof);

                //checked wheather status already exist using registration id
                
                command = new SqlCommand("SPI_Status", connectionLink);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@status", "pending");
                command.Parameters.AddWithValue("@registrationId", passport.RegistrationId);

                connectionLink.Open();
                command.ExecuteNonQuery();

                connectionLink.Close();
               
                command = new SqlCommand("SPI_Passport", connectionLink);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@PassportOfficeName", passport.PassportOfficeNAme);
                command.Parameters.AddWithValue("@Photo", passport.PhotoBytes);
                command.Parameters.AddWithValue("@IdentityProof", passport.BirthProofBytes);
                command.Parameters.AddWithValue("@BirthProof", passport.BirthProofBytes);
                command.Parameters.AddWithValue("@NationalityProof", passport.NationalityProofBytes);
                command.Parameters.AddWithValue("@RegistrationID", passport.RegistrationId);

                connectionLink.Open();

                int r = command.ExecuteNonQuery();
                if (r > 0)
                {
                    return true;
                }
                else
                    return false;
            }
            catch(Exception ex)
            {
                //Console.WriteLine(ex.Message);
                throw;
            }
            finally { connectionLink.Close(); }
        }

        /// <summary>
        /// To update the passport data of user
        /// using id
        /// </summary>
        /// <param name="passport"></param>
        /// <returns></returns>
        public bool UpdateUser(Passport passport)
        {
            try
            {
                passport.PhotoBytes = ConvertToByteArray(passport.Photo);
                passport.BirthProofBytes = ConvertToByteArray(passport.BirthProof);
                passport.IdentityProofBytes = ConvertToByteArray(passport.IdentityProof);
                passport.NationalityProofBytes = ConvertToByteArray(passport.NationalityProof);

                command = new SqlCommand("SPU_Passport", connectionLink);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@PassportOfficeName", passport.PassportOfficeNAme);
                command.Parameters.AddWithValue("@Photo", passport.PhotoBytes);
                command.Parameters.AddWithValue("@IdentityProof", passport.BirthProofBytes);
                command.Parameters.AddWithValue("@BirthProof", passport.BirthProofBytes);
                command.Parameters.AddWithValue("@NationalityProof", passport.NationalityProofBytes);
                command.Parameters.AddWithValue("@id", passport.Id);

                connectionLink.Open();

                int r = command.ExecuteNonQuery();
                if (r > 0)
                {
                    return true;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {

                throw;
            }
            finally { connectionLink.Close(); }

        }

        /// <summary>
        /// Delete application from passport data 
        /// and also delete the status from 
        /// status table 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool DeleteUser(int id)
        {
            try
            {
                command = new SqlCommand("SPD_Status", connectionLink);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@id", id);
                connectionLink.Open();
                command.ExecuteNonQuery();
                connectionLink.Close();

                command = new SqlCommand("SPD_Passport", connectionLink);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@id", id);

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
        /// To convert the file to binary data
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        private byte[] ConvertToByteArray(HttpPostedFileBase file)
        {
            if (file != null && file.ContentLength > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    file.InputStream.CopyTo(memoryStream);
                    return memoryStream.ToArray();
                }
            }
            return null;
        }

        /// <summary>
        /// To check wheather registration id 
        /// is already present or not in 
        /// passport data to prevent duplicate
        /// insertion of the passport application
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool checkIdPassportData(int id)
        {
            try
            {
                connectionLink.Open();
                command = new SqlCommand("SPS_PassportRegistrationId", connectionLink);
                
                    //SELECT id from PassportData where RegistrationID = @id
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Id", id);
                object result = command.ExecuteScalar();

                if (result != null)
                {
                    return false;
                }
                else
                {
                    return true;
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