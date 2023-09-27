using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PassportGenerator.Models
{
    public class GeneratedPassport
    {
        public int Id { get; set; }
        public string Status { get; set; }
        public string PassportNumber { get;set; }
        public int RegistrationId { get; set; }

    }
}