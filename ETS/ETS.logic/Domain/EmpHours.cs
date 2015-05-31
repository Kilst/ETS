using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETS.logic.Domain
{
    public class EmpHours
    {
        // Properties
        public int EmpHoursID { get; set; }
        public Employee Emp { get; set; }
        public DateTime Date { get; set; }
        public string Hours { get; set; }

        // Constructors

        public EmpHours(Employee emp, int empHoursID, DateTime date, string hours)
        {
            this.Emp = emp;
            this.EmpHoursID = empHoursID;
            this.Date = date;
            this.Hours = hours;
        }

        public EmpHours(Employee emp)
        {
            this.Emp = emp;
        }

        public override string ToString()
        {
            string[] date = this.Date.ToString().Split(null);
            string empInfo = "";
            empInfo += "EmpHoursID: " + this.EmpHoursID + "\n"
                    + "Employee ID: " + this.Emp.EmpID + "\n"
                    + "Date: " + date[0] + "\n"
                    + "Hours: " + this.Hours + "\n\n";
            return empInfo;
        }
    }
}
