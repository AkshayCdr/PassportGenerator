using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PassportGenerator.Models
{
    public class PassportGeneratedList
    {
        public HttpPostedFileBase Photo { get; set; }
        public byte[] PhotoBytes { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PassportNumber { get; set; }
        public string PassportOfficeName { get; set; }
    }
}