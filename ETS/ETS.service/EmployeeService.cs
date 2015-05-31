using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//..
using System.Data;
using ETS.data;
using ETS.logic.Domain;
using ETS.logic;

namespace ETS.service
{
    public class EmployeeService
    {
        public static List<Employee> GetEmpList()
        {
            try
            {
                return EmployeeDao.GetEmpList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        //Add customer
        public static int InsertEmployee(Employee emp)
        {
            try
            {
                return EmployeeDao.InsertEmployee(emp);
            }
            catch (Exception)
            {
                return (int)SuccessEnum.Fail;  
            }
        }
        // Update Employee Info
        public static int UpdateEmployee(Employee emp)
        {
            try
            {
                return EmployeeDao.UpdateEmployee(emp);
            }
            catch (Exception)
            {
                return (int)SuccessEnum.Fail;               
            }
        }
        // Add Date and Hours to Employee timesheet
        public static int InsertHours(EmpHours emp)
        {
            try
            {
                return EmployeeDao.InsertHours(emp);
            }
            catch (Exception)
            {
                return (int)SuccessEnum.Fail;             
            }
        }

        public static List<EmpHours> GetEmpHours(Employee emp)
        {
            try
            {
                return EmployeeDao.GetEmpHours(emp);
            }
            catch (Exception)
            {
                return EmployeeDao.GetEmpHours(emp);             
            }
        }
    }
}
