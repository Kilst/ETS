using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETS.logic.Domain
{
    public class Employee
    {
        // Fields
        public int EmpID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime DOB { get; set; }
        public string Phone { get; set; }
        public List<EmpHours> HoursList { get; set;}

        // Constructors
        public Employee()
        {
        }

        public Employee(int empID, string firstName, string lastName, string email, DateTime dob, string phone)
        {
            this.EmpID = empID;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Email = email;
            this.DOB = dob;
            this.Phone = phone;
        }

        // Methods
        public override string ToString()
        {
            // Split the date and the time
            string[] dob = this.DOB.ToString().Split(null);

            string empInfo = "";
            empInfo += "Name: " + this.FirstName + ", "
                    + this.LastName + "\n"
                    + "ID: " + this.EmpID + "\n"
                    + "Email: " + this.Email + "\n"
                    + "DOB: " + dob[0] + "\n"
                    + "Phone: " + this.Phone + "\n\n";
            return empInfo;
        }

        public string Report()
        {
            string output = "";
            try
            {
                // If no hours are found, HoursList[0] throws a nullpointer exception
                if (this.HoursList[0] != null)
                {
                    // Foreach through the List and add the hours to the txtBox
                    foreach (var item in this.HoursList)
                    {
                        output += item.ToString();
                    }
                }
            }
            catch (Exception)
            {
                // Report that there are no hours
                output += "No hours in the DataBase.";
            }
            return output;
        }

        public int CalcHours()
        {
            DateTime date = new DateTime(1999, 12, 05);
            return 1;
        }
    }
}
