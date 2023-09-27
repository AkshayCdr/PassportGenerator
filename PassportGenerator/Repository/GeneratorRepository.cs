using iTextSharp.text.pdf;
using iTextSharp.text;
using PassportGenerator.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using Image = iTextSharp.text.Image;
using iTextSharp.text.pdf.qrcode;
using System.Drawing.Imaging;
using System.Net.NetworkInformation;
using System.Security.Cryptography;

namespace PassportGenerator.Repository
{
    public class GeneratorRepository
    {

        //SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["cs"].ConnectionString);
        SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["cs"].ConnectionString);
        SqlCommand command;
        SqlDataAdapter adapter;
        DataTable table;

        /// <summary>
        /// Get Data 
        /// </summary>
        /// <returns></returns>
        public List<Generator> GetPassportDetails() 
        {
            List<Generator> list = new List<Generator>();
            using (connection)
            {
                command = new SqlCommand("SPS_Generate", connection);
                command.CommandType = CommandType.StoredProcedure;
                adapter = new SqlDataAdapter(command);
                table = new DataTable();
                adapter.Fill(table);
               
                foreach(DataRow table in table.Rows)
                {
                    DateTime dobWithTime = DateTime.Parse(table["Dob"].ToString());
                    string dobWithoutTime = dobWithTime.Date.ToString("yyyy-MM-dd");
                    list.Add(new Generator
                    {
                        PhotoBytes = (byte[])table["photo"],
                        FirstName = table["FirstName"].ToString(),
                        LastName = table["LastName"].ToString(),
                        //Dob = (DateTime.Parse(table["Dob"].ToString())).Date,
                        //Dob = DateTime.Parse(table["Dob"].ToString()).Date,
                        Dob = DateTime.Parse(dobWithoutTime),
                        Age = Convert.ToInt32(table["Age"]),
                        Gender = table["Gender"].ToString(),
                        State = table["State"].ToString(),
                        StatusName = table["status"].ToString(),
                        RegistrationId = (int)table["id"]
                    });
                }

            }
            list.ForEach(generator => { generator.Dob = generator.Dob.Date ; });
            return list;
        }





        /// <summary>
        /// To generate passport in pdf file 
        /// It will generate passport in a pdf file 
        /// in passport folder 
        /// </summary>
        /// <param name="generator"></param>
        /// <returns></returns>
        public bool GeneratePassport(Generator generator)
        {

            string randomString = RandomStringGenerator.GenerateRandomString();
            //if already generated return false
            if (checkPassportGenerated(generator.RegistrationId))
            {
                InsertIntoGenerator(generator, randomString);
            }
            else
            {
                return false;
            }


            var location = "C:\\Users\\Akshay\\Desktop\\Claysys\\ProjectNoTempDay10\\PassportGenerator\\PassportGenerator\\Passport\\";
            var fileName = $"{generator.FirstName}_{generator.LastName}_Passport.pdf"; // PDF file format
            var fileNameImg = $"{generator.FirstName}_{generator.LastName}_Passport.jpg"; // PDF file format
            var filePath = Path.Combine(location, fileName);
            var filePathImg = Path.Combine(location, fileNameImg);
            createImage(generator, filePathImg);

            Document doc = null;
            PdfWriter writer = null;

            try
            {
                doc = new Document(iTextSharp.text.PageSize.LETTER, 10, 10, 42, 35);
                writer = PdfWriter.GetInstance(doc, new FileStream(filePath, FileMode.Create));
                doc.Open();

                //iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance("C:\\Users\\Akshay\\Desktop\\Claysys\\ProjectNoTempDay5.1\\PassportGenerator\\PassportGenerator\\Imgs\\1.jpg");
                iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(filePathImg);

                img.ScaleToFit(50f, 50f);
               
                doc.Add(img);

                //var passportContent = $"Passport Information:\n" +
                //                                      $"ID: {generator.Id}\n" +
                //                                      $"First Name: {generator.FirstName}\n" +
                //                                      $"Last Name: {generator.LastName}\n" +
                //                                      $"Date of Birth: {generator.Dob:yyyy-MM-dd}\n" +
                //                                      $"Age: {generator.Age}\n" +
                //                                      $"Gender: {generator.Gender}\n" +
                //                                      $"State: {generator.State}\n" +
                //                                      $"Status Name: {generator.StatusName}\n" +
                //                                      $"Registration ID: {generator.RegistrationId}";

                //var paragraph = new Paragraph(passportContent);
                //doc.Add(paragraph);

                

                List list = new List(List.UNORDERED);

                list.IndentationLeft = 35f;

                list.Add(randomString);
                list.Add(generator.FirstName);
                list.Add(generator.LastName);
                list.Add(generator.Dob.Date.ToString());
                list.Add(generator.Gender);
                list.Add(generator.State);
                doc.Add(list);
                doc.Close();

            }
            catch (Exception ex)
            {

            }
            finally
            {
                doc.Close();
            }
            return true;
        }

        /// <summary>
        /// Function to create image in 
        /// the passport folder
        /// </summary>
        /// <param name="generator"></param>
        /// <param name="filePathImg"></param>
        public void createImage(Generator generator, string filePathImg)
        {
            try
            {
                File.WriteAllBytes(filePathImg, generator.PhotoBytes);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// To insert passport details into 
        /// Passport table which is the 
        /// details of users which passport generated
        /// </summary>
        /// <param name="generator"></param>
        /// <param name="randomString"></param>

        public void InsertIntoGenerator(Generator generator,string randomString)
        {
            try
            {
                SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["cs"].ConnectionString);
      
                command = new SqlCommand("INSERT INTO Passport (Status,PassportNumber,RegistrationId) VALUES (@status,@passportnumber,@registrationid)",connection);
                command.CommandType = CommandType.Text;
                command.Parameters.AddWithValue("@status", "Generated");
                command.Parameters.AddWithValue("@passportnumber", randomString);
                command.Parameters.AddWithValue("@registrationid", generator.RegistrationId);
                connection.Open();
                int r = command.ExecuteNonQuery();
            }
            catch (Exception)
            {

                throw;
            }
            finally { connection.Close(); }



        }


        /// <summary>
        /// To check wheather User passport
        /// is already generated or not
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool checkPassportGenerated(int id)
        {
            try
            {
                SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["cs"].ConnectionString);
                command = new SqlCommand("SELECT PassportNumber FROM Passport WHERE RegistrationId = @id",connection);
                command.CommandType = CommandType.Text;
                command.Parameters.AddWithValue("@id", id);
                connection.Open();
                object passportNumber = command.ExecuteScalar();
                if (passportNumber != null)
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
            finally { connection.Close(); }
          
        }
    }


    /// <summary>
    /// To generatre random string 
    /// for passport numbr 
    /// </summary>
    public class RandomStringGenerator
    {
        private static readonly Random random = new Random();
        private const string Chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

        public static string GenerateRandomString()
        {
            int length = 10;
            char[] buffer = new char[length];

            for (int i = 0; i < length; i++)
            {
                buffer[i] = Chars[random.Next(Chars.Length)];
            }
            return new string(buffer);
        }


    }
}


