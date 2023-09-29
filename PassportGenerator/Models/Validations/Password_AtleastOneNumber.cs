using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace PassportGenerator.Models.Validations
{
    public class Password_AtleastOneNumber:ValidationAttribute 
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var Registration = validationContext.ObjectInstance as Registration;
            if (Registration.Password == null) 
            {
                return new ValidationResult("Password is null");
            }
            else if(Registration.Password.Length <8)
            {
                return new ValidationResult("Password length must be more than 8 ");
            }
            else if(!Regex.IsMatch(Registration.Password, @"\d"))
            {
                return new ValidationResult("Password Should contains atleast one digit");
            }
            else
            {
                return ValidationResult.Success;
            }
            //return base.IsValid(value, validationContext);
        }
    }
}