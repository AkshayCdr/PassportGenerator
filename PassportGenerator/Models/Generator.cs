using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PassportGenerator.Models
{
    public class Generator
    {
        public int Id { get; set; }
        public HttpPostedFileBase Photo { get; set; }
        public byte[] PhotoBytes { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Dob { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; }
        public string State { get; set; }
        public string StatusName { get; set; }
        public int RegistrationId { get; set; }
       


    }
}