using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//..
using ETS.logic.Domain;
using ETS.logic;
using System.Data;
using System.Data.SqlClient;

namespace ETS.data
{
    public class EmployeeDao
    {
        public static List<Employee> GetEmpList()
        {
            List<Employee> empList = new List<Employee>();
            myConnection.Open();
            SqlCommand get = new SqlCommand("dbo.sp_Employee_SelectAll", myConnection);
            get.CommandType = CommandType.StoredProcedure;
            SqlDataReader myReader = get.ExecuteReader();

            // Check myReader has hours info
            if (myReader.HasRows)
            {
                while (myReader.Read())
                {
                    // Create new Employee object
                    Employee emp = new Employee();
                    // Add properties
                    emp.EmpID = (int)(myReader["EmpID"]);
                    emp.FirstName = (myReader["FirstName"]).ToString();
                    emp.LastName = (myReader["LastName"]).ToString();
                    emp.Email = (myReader["Email"]).ToString();
                    emp.DOB = (DateTime)(myReader["DOB"]);
                    emp.Phone = (myReader["Phone"]).ToString();
                    // Add to empList
                    empList.Add(emp);
                }
            }
            myConnection.Close();
            return empList;
        }
        // Set connection string
        private static SqlConnection myConnection = new SqlConnection();
        public static void SetCon(string conn)
        {
            myConnection = new SqlConnection(conn);
        }
        public static DataSet GetDBInfo()
        {
            // Method to open database, retrieve all Employees, store it in a DataSet,
            // and the pass it back to MainForm
            myConnection.Open();
            DataSet ds = new DataSet();
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = new SqlCommand("dbo.sp_Employee_SelectAll", myConnection);
            adapter.Fill(ds);
            myConnection.Close();
            return ds;
        }

        // Get EmpHours by EmpID
        public static List<EmpHours> GetEmpHours(Employee emp)
        {
            List<EmpHours> empHoursList = new List<EmpHours>();
            
            // Open the SQL connection
            myConnection.Open();

            // Create a new SQL command using the stored procedure
            SqlCommand get = new SqlCommand("dbo.sp_EmpHours_GetEmpHours", myConnection);
            // Set commandtype to store procedure
            get.CommandType = CommandType.StoredProcedure;
            // Add parameters to command
            get.Parameters.AddWithValue("@empID", emp.EmpID);

            // Execture store procedure
            SqlDataReader myReader = get.ExecuteReader();

            // Check myReader has hours info
            if (myReader.HasRows)
            {
                while (myReader.Read())
                {
                    // Create new EmpHours
                    EmpHours empHours = new EmpHours(emp);
                    empHours.EmpHoursID = (int)(myReader["EmpHoursID"]);
                    empHours.Emp.EmpID = (int)(myReader["EmpID"]);
                    empHours.Date = (DateTime)(myReader["WorkDate"]);
                    empHours.Hours = (myReader["Hours"]).ToString();
                    // Add to List
                    empHoursList.Add(empHours);
                }
            }

            // Close the connection (VERY important!)
            myConnection.Close();
            return empHoursList;
        }

        public static int InsertEmployee(Employee emp)
        {
            // Open the SQL connection
            myConnection.Open();

            // Create a new SQL command using the stored procedure
            SqlCommand add = new SqlCommand("dbo.sp_Employee_InsertEmployee", myConnection);
            // Set commandtype to store procedure
            add.CommandType = CommandType.StoredProcedure;
            // Add parameters to command
            add.Parameters.AddWithValue("@firstName", emp.FirstName);
            add.Parameters.AddWithValue("@lastName", emp.LastName);
            add.Parameters.AddWithValue("@email", emp.Email);
            add.Parameters.AddWithValue("@dob", emp.DOB);
            add.Parameters.AddWithValue("@phone", emp.Phone);

            // Execture store procedure
            add.ExecuteNonQuery();

            // Close the connection (VERY important!)
            myConnection.Close();
            return (int)SuccessEnum.Success;
        }

        public static int UpdateEmployee(Employee emp)
        {
            // Open the SQL connection
            myConnection.Open();

            // Create a new SQL command using the stored procedure
            SqlCommand update = new SqlCommand("dbo.sp_Employee_UpdateEmployee", myConnection);
            // Set commandtype to store procedure
            update.CommandType = CommandType.StoredProcedure;
            // Add parameters to command
            update.Parameters.AddWithValue("@empID", emp.EmpID);
            update.Parameters.AddWithValue("@firstName", emp.FirstName);
            update.Parameters.AddWithValue("@lastName", emp.LastName);
            update.Parameters.AddWithValue("@email", emp.Email);
            update.Parameters.AddWithValue("@dob", emp.DOB);
            update.Parameters.AddWithValue("@phone", emp.Phone);

            // Execture store procedure
            update.ExecuteNonQuery();

            // Close the connection (VERY important!)
            myConnection.Close();
            return (int)SuccessEnum.Success;
        }

        private static Employee SearchById(int empId)
        {
            Employee emp = new Employee();
            // Open the SQL connection
            myConnection.Open();

            // Create a new SQL command using the stored procedure
            SqlCommand search = new SqlCommand("dbo.sp_Employee_SearchByID", myConnection);
            // Set commandtype to store procedure
            search.CommandType = CommandType.StoredProcedure;
            // Add parameters to command
            search.Parameters.AddWithValue("@empID", empId);

            // Execture store procedure
            SqlDataReader myReader = search.ExecuteReader();

            // Check myReader has employee info
            if (myReader.HasRows)
            {
                while (myReader.Read())
                {
                    emp.EmpID = (int)(myReader["EmpID"]);
                    emp.FirstName = (myReader["FirstName"]).ToString();
                    emp.LastName = (myReader["LastName"]).ToString();
                    emp.Email = (myReader["Email"]).ToString();
                    emp.DOB = (DateTime)(myReader["DOB"]);
                    emp.Phone = (myReader["Phone"]).ToString();
                }
            }

            // Close the connection (VERY important!)
            myConnection.Close();
            return emp;
        }

        public static int InsertHours(EmpHours empHours)
        {
            // Format date so we can split it using (null) [whitespaces]
            //string date = emp.Date.Replace("/", " ");
            //date = date.Replace("-", " ");
            //string[] dateFormat = date.Split(null);
            //date = dateFormat[2] + dateFormat[1] + dateFormat[0];
            // Open the SQL connection
            myConnection.Open();

            // Create a new SQL command using the stored procedure
            SqlCommand add = new SqlCommand("dbo.sp_EmpHours_InsertEmployeeHours", myConnection);
            // Set commandtype to store procedure
            add.CommandType = CommandType.StoredProcedure;
            // Add parameters to command
            add.Parameters.AddWithValue("@empID", empHours.Emp.EmpID);
            add.Parameters.AddWithValue("@workDate", empHours.Date);
            add.Parameters.AddWithValue("@hours", empHours.Hours);

            // Execture store procedure
            add.ExecuteNonQuery();

            // Close the connection (VERY important!)
            myConnection.Close();
            return (int)SuccessEnum.Success;
        }

        //public static int SelectHoursById(Employee emp)
        //{
        //    string empInfo = "";
        //    // Open the SQL connection
        //    myConnection.Open();

        //    // Create a new SQL command using the stored procedure
        //    SqlCommand search = new SqlCommand("dbo.sp_EmpHours_SelectHours", myConnection);
        //    // Set commandtype to store procedure
        //    search.CommandType = CommandType.StoredProcedure;
        //    // Add parameters to command
        //    search.Parameters.AddWithValue("@empID", emp.EmpID);

        //    // Execture store procedure
        //    SqlDataReader myReader = search.ExecuteReader();

        //    // Check myReader has employee info
        //    if (myReader.HasRows)
        //    {
        //        while (myReader.Read())
        //        {
        //            string date = (myReader["WorkDate"]).ToString();
        //            string[] dateArr = date.Split(null);
        //            empInfo += "Hours ID: " + (myReader["EmpHoursID"]).ToString() + "\n"
        //                    + "Work Date: " + dateArr[0] + "\n"
        //                    + "Hours: " + (myReader["Hours"]).ToString() + "\n\n";
        //        }
        //    }
        //        // Doesn't send a string now, so I have to work out antoher way
        //        // to return the info.
        //    else
        //    {
        //        myConnection.Close();
        //        return (int)SuccessEnum.Fail;
        //    }

        //    // Close the connection (VERY important!)
        //    myConnection.Close();
        //    return (int)SuccessEnum.Success;
        //}

    }
}
