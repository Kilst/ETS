using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//..
using System.Text.RegularExpressions;
using ETS.logic.Domain;

namespace ETS.view
{
    public class RegexCheck
    {
        public static string Checker(Employee emp)
        {
            string[] dob = emp.DOB.ToString().Split(null);
            string output = "";
            // Method to pass validation inputs to correct methods
            if (NumbersOnly(emp.EmpID.ToString()) == true &&
                LettersOnly(emp.FirstName) == true &&
                LettersOnly(emp.LastName) == true &&
                EmailFormat(emp.Email) == true &&
                DateFormat(dob[0]) == true &&
                PhoneFormat(emp.Phone) == true)
            {
                return "Pass";
            }
            else if (NumbersOnly(emp.EmpID.ToString()) == false)
            {
                //return "Invalid Employee ID!";
                output += "Invalid Employee ID!\n";
            }
            if (LettersOnly(emp.FirstName) == false)
            {
                //return "Invalid First Name!";
                output += "Invalid First Name!\n";
            }
            if (LettersOnly(emp.LastName) == false)
            {
                //return "Invalid Last Name!";
                output += "Invalid Last Name!\n";
            }
            if (EmailFormat(emp.Email) == false)
            {
                //return "Invalid Email!";
                output += "Invalid Email!\n";
            }
            if (DateFormat(dob[0]) == false)
            {
                //return "Invalid Date Format!";
                output += "Invalid Date Format!\n";
            }
            if (PhoneFormat(emp.Phone) == false)
            {
                //return "Invalid Phone Number!";
                output += "Invalid Phone Number!\n";
            }

            return output;
        }
        public static string Checker(EmpHours empHours)
        {
            string output = "";
            // Method to pass validation inputs to correct methods
            if (DateFormat(empHours.Date.ToString()) == true && (NumbersOnly(empHours.Hours) == true))
            {
                // Check that hours is actually a number
                try
                {
                    double doubleCheck = double.Parse(empHours.Hours.ToString()) * 2;
                }
                catch (Exception)
                {
                    return "Invalid Hours (nonRegexCheck)!";
                }
                return "Pass";
            }
            else if (DateFormat(empHours.Date.ToString()) == false)
            {
                //return "Invalid Date Format!\nDD/MM/YYYY or D/MM/YYYY only";
                output += "Invalid Date Format!\nDD/MM/YYYY or D/MM/YYYY only!\n";
            }
            if (NumbersOnly(empHours.Hours.ToString()) == false)
            {
                //return "Invalid Hours!";
                output += "Invalid Hours!\n";
            }
            else
            {
                return "unknown failure";
            }
            return output;
        }
        
        private static bool NoSQL(string input)
        {
            return true;
        }
        private static bool LettersOnly(string input)
        {
            // Letters only Validation
            string pattern = @"^[a-zA-Z]{1,30}$";
            return RegCheck(input, pattern);
        }
        private static bool NumbersOnly(string input)
        {

            // Numbers only Validation (includes decimals)
            if (input.Contains(".") && input[0] != '.')
            {
                // Regex idea from:
                // http://stackoverflow.com/questions/8809354/replace-first-occurrence-of-pattern-in-a-string
                Regex reg = new Regex(Regex.Escape("."));
                input = reg.Replace(input, "", 1);
            }
            string pattern = @"^[0-9]{1,30}$";
            return RegCheck(input, pattern);
        }

        //private static bool LettersAndNumbers(string input)
        //{
        //    // Letters only Validation
        //    string pattern = @"^[a-zA-Z,0-9]+$";
        //    return RegCheck(input, pattern);
        //}
        public static bool Password(string input)
        {
            // Letters only Validation
            string pattern = @"^[a-zA-Z,0-9]{4,12}$";
            return RegCheck(input, pattern);
        }
        private static bool PhoneFormat(string input)
        {
            // Info on phone number min and max::
            // http://stackoverflow.com/questions/3350500/international-phone-number-max-and-min
            // Phone number format Validation
            // I set Phone to varchar(10) in the DB, so I can't go up to 16 anyway
            string pattern = @"^[0-9]{7,10}$";
            return RegCheck(input, pattern);
        }

        private static bool DateFormat(string input)
        {
            // Date Validation
            // Convert date format
            //input = MDYToDMY(input);
            string pattern = @"^[0-9]{1,2}[/,-]{1}[0-9]{2}[/,-]{1}[1-9]{1}[0-9]{3}$";
            return RegCheck(input, pattern);
        }

        //public static string MDYToDMY(string input)
        //{
        //    // Convert from MM/DD/YYYY to DD/MM/YYYY
        //    // Directly copied from
        //    // https://msdn.microsoft.com/en-us/library/c8e9427h%28v=vs.110%29.aspx
        //    try
        //    {
        //        return Regex.Replace(input,
        //              "\\b(?<month>\\d{1,2})/(?<day>\\d{1,2})/(?<year>\\d{2,4})\\b",
        //              "${day}/${month}/${year}", RegexOptions.None,
        //              TimeSpan.FromMilliseconds(150));
        //    }
        //    catch (RegexMatchTimeoutException)
        //    {
        //        return input;
        //    }
        //}

        private static bool EmailFormat(string input)
        {
            // Regex pattern for Email validation taken from:
            // http://stackoverflow.com/questions/5342375/c-sharp-regex-email-validation
            string pattern = @"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*"
                            + "@"
                            + @"((([\-\w]+\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\.){3}[0-9]{1,3}))$";
            return RegCheck(input, pattern);
        }

        private static bool RegCheck(string input, string pattern)
        {
            // Compare input with pattern
            Match match = Regex.Match(input, pattern);
            if (match.Success)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
