using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//..
using System.Data;
using ETS.logic;
using ETS.logic.Domain;
using ETS.data;

namespace ETS.service
{
    // All data being sent goes through here first

    public static class DataHandler
    {

        public static List<Employee> GetEmpList()
        {
            return EmployeeService.GetEmpList();
        }
        public static void ConnectionString()
        {
            // Set Connection string
            LoginService.ConnectionString();
        }

        public static int InsertEmployee(Employee emp)
        {
            // Add a new Employee to the DataBase
            // Call StoredProcedure
            return EmployeeService.InsertEmployee(emp);
        }

        public static int UpdateEmployee(Employee emp)
        {
            // Update an Employee in the DataBase
            // Call StoredProcedure
            return EmployeeService.UpdateEmployee(emp);
        }

        public static int InsertHours(EmpHours emp)
        {
            // Add a new Employee Working Hours and Date to the DataBase
            // Call StoredProcedure
            return EmployeeService.InsertHours(emp);
        }

        public static List<EmpHours> GetEmpHours(Employee emp)
        {
            // Call StoredProcedure
            return EmployeeService.GetEmpHours(emp);
        }

        public static int NewLogin(LoginUser user)
        {
            return LoginService.NewLogin(user);
        }

        public static int HashLogin(LoginUser user)
        {
            return LoginService.HashLogin(user);
        }

        public static int UpdatePassword(LoginUser user)
        {
            return LoginService.UpdatePassword(user);
        }

        //public static string SysLogin(string userName, string password)
        //{
        //    // Login
        //    return LoginDao.SysLoginSQLInjection(userName, password);
        //}

        //public static string SearchById(int empId)
        //{
        //    if (RegexCheck.NumbersOnly(empId.ToString()) == true)
        //    {
        //        return EmployeeService.SearchById(empId);
        //    }
        //    else
        //    {
        //        return "Invalid entry. Numbers only.";
        //    }
        //}
    }
}
