using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        //public DateTime Dob { get; set; }
        public string Dob { get; set; }
        public string Gender { get; set; }
        public string Email { get; set; }
        public string PassportNumber { get; set; }
        public string State { get; set; }
        public string PassportOfficeName { get; set; }

        //public DateTime DateOfIssue { get; set; }

        //public DateTime DateOfExpiry { get; set; }

        public string DateOfIssue { get; set; }

        public string DateOfExpiry { get; set; }
        public byte[] SignatureBytes { get; set; }

    }
}