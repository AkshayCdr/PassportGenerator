using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PassportGenerator.Models
{
    public class Passport
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Please select a photo.")]

        public string PassportOfficeNAme { get; set; }
        public HttpPostedFileBase Photo { get; set; }
        public byte[] PhotoBytes { get; set; }

        [Required(ErrorMessage = "Please select an identity proof.")]
        public HttpPostedFileBase IdentityProof { get; set; }
        public byte[] IdentityProofBytes { get; set; }

        [Required(ErrorMessage = "Please select a proof of birth.")]
        public HttpPostedFileBase BirthProof { get; set; }
        public byte[] BirthProofBytes { get; set; }

        [Required(ErrorMessage = "Please select a proof of nationality.")]
        public HttpPostedFileBase NationalityProof { get; set; }
        public byte[] NationalityProofBytes { get; set; }
        public HttpPostedFileBase Signature { get; set; }
        public byte[] SignatureBytes { get; set; }
        public int RegistrationId { get; set; }
        public string Email { get; set; }
    }
}