using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace   Exam70_483.Exercices.Validation
{
    public class Course
    {
        public static void Launch()
        {

        }

        public class Customer
        {

            public int Id { get; set; }

            [Required, MaxLength(20)]
            public string FirstName { get; set; }

            [Required,MaxLength(20)]
            public string LastName { get; set; }


        }

        public class Adress
        {

            [Range(0,100000)]
            public int Id { get; set; }

            [Required,MaxLength(20)]
            public string AdressLine1 { get; set; }

            [RegularExpression(@"^[1-9][0-9]{3}\s?[a-zA-Z]{2}$")]
            public string ZipCode { get; set; }
        }

        #region private methods

        static bool ValidateZipCode(string zipCode)
        {
            //valid zipcodes : 1234AB | 1234 AB | 1001 AB

            if (zipCode.Length > 6) return false;

            string numberPart = zipCode.Substring(0, 4);

            int number;

            if (!int.TryParse(numberPart, out number)) return false;

            string characterPart = zipCode.Substring(4);

            if (numberPart.StartsWith("0")) return false;

            if (characterPart.Trim().Length > 2) return false;

            if (characterPart.Length == 3 && characterPart.Trim().Length != 2)
                return false;

            return true;
        }

        static bool ValidateZipCodeRegEx(string zipCode)
        {
            Match match = Regex.Match(zipCode, @"^[1-9][0-9]{3}\s?[a-zA-Z]{2}$", RegexOptions.IgnoreCase);

            return match.Success;
        }

        #endregion
    }
}
