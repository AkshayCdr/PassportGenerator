using PassportGenerator.Models.Validations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PassportGenerator.Models
{
    public class Registration
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "First name is required")]
        [StringLength(100, ErrorMessage = "Name cannot be longer than 100 characters")]
        [RegularExpression(@"^[a-zA-Z\s]*$", ErrorMessage = "Name can only contain alphabets")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "First name is required")]
        [StringLength(100, ErrorMessage = "Name cannot be longer than 100 characters")]
        [RegularExpression(@"^[a-zA-Z\s]*$", ErrorMessage = "Name can only contain alphabets")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Date of Birth is required")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Dob { get; set; }

        [Required(ErrorMessage = "Age is required")]
        [Range(1, 150, ErrorMessage = "Age must be between 1 and 150")]
        public int Age { get; set; }

        [Required(ErrorMessage = "Gender is required")]
        public string Gender { get; set; }

        [Required(ErrorMessage = "Phone number is required")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Invalid phone number format. Please enter a 10-digit phone number.")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "State is required")]
        public string State { get; set; }

        [Required(ErrorMessage = "City is required")]
        public string City { get; set; }

        [Required(ErrorMessage = "Username is required")]
        [StringLength(50, ErrorMessage = "Username cannot be longer than 50 characters")]
        public string Username { get; set; }

        //[Required(ErrorMessage = "Password is required")]
        //[StringLength(100, MinimumLength = 8, ErrorMessage = "Password must be at least 8 characters long")]
        [Password_AtleastOneNumber]
        public string Password { get; set; }
    }

}