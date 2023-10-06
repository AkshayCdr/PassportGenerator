using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PassportGenerator.Models
{
    public class Police
    {
        public int Id { get; set; }
        public int RegistrationId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string PassportOfficeName { get; set; }
        public HttpPostedFileBase Photo { get; set; }
        public byte[] PhotoBytes { get; set; }    
        public HttpPostedFileBase IdentityProof { get; set; }
        public byte[] IdentityProofBytes { get; set; }
        public HttpPostedFileBase BirthProof { get; set; }
        public byte[] BirthProofBytes { get; set; }
        public HttpPostedFileBase NationalityProof { get; set; }
        public byte[] NationalityProofBytes { get; set; }
        public HttpPostedFileBase Signature { get; set; }
        public byte[] SignatureBytes { get; set; }
        public string Generated { get;set; }
        public string AdminApproveStatus { get; set; }
        public string PoliceApproveStatus { get; set; }
        public DateTime? PoliceApproveDate { get; set; }
        //public String PoliceApproveDate { get; set; }
    }
}