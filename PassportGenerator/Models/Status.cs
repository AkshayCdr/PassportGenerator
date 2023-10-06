using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PassportGenerator.Models
{
    public class Status
    {
        [Key]
        public int Id { get; set; }

        
        public string StatusName { get; set; }

        [ForeignKey("Registration")]
        public int RegistrationId { get; set; }
        public string Email { get; set; }
        public string PoliceApproval { get;set; }

        public Registration Registration { get; set; }
    }
}