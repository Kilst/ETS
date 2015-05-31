using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETS.logic.Domain
{
    public class EmployeeList
    {
        public List<Employee> EmpList { get; set; }

        public EmployeeList()
        {
            //this.EmpList = DataHandler.GetEmpList();
        }
    }
}
