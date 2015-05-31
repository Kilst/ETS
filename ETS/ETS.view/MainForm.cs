using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
//..
using ETS.logic;
using ETS.logic.Domain;
using ETS.service;
using System.Threading;

namespace ETS.view
{
    public partial class MainForm : Form
    {
        // Create a global ListService so we don't have to keep calling the DB 
        // for each employee change in list and cmblist
        // Updated when FillListEmp() is called (called on button clicks like btnUpdate, btnAdd etc etc)
        EmployeeList listServ = new EmployeeList();
        // Create new Employee to read to
        Employee emp = new Employee();
        // Current logged in user
        string currentUser = "";
        
        public MainForm()
        {
            InitializeComponent();

            // Initial FillList to fill the listbox
            FillListEmp();
            // About tab message
            lblAboutInfo.Text = "Features::\n\nAdd employee\nUpdate employee information\nAdd hours to employee timesheet\nAdd new users\nUpdate password";
        }

        // Testing Threading
        public void ThreadTest()
        {
            int x = 0;
            while (x < 2000000000)
            {
                x++;
            }
            for (int i = 0; i < 2000000000; i++)
            {
                
            }
            for (int s = 0; s < 2000000000; s++)
            {

            }
            MessageBox.Show("Finally hit the end of the thread!");
        }

        public void FillListEmp()
        {
            try
            {
                // Set Connection String
                DataHandler.ConnectionString();
                // On create it connects to the Database and retreives employee info
                //DataSetService dsServ = new DataSetService();
                listServ.EmpList = DataHandler.GetEmpList();
                // Update listbox with 
                listboxUpdate.DataSource = listServ.EmpList;
                listboxUpdate.DisplayMember = "FirstName";
                // Update combobox with 
                cmbEmployee.DataSource = listServ.EmpList;
                cmbEmployee.DisplayMember = "FirstName";
            }
            catch (Exception)
            {

                MessageBox.Show("DataBase is unreachable!\nPlease contact your network administrator.", "ERROR!!");
            }
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(new ThreadStart(ThreadTest));
            thread.Start();
            //string info = DataHandler.GetEmps();
            //string info = DataHandler.SearchById(10);
            //MessageBox.Show(info, "Employee Info");
            //FillList();
            ////MessageBox.Show(sp.InsertEmployee("Greg", "Atom", "atom@atom.com", "12-10-2001", "33222222"));
        }

        private void btnAddEmployee_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Do you wish to add a new employee?", "Add new employee?", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                string output = "";
                // Read inputs
                emp.FirstName = txtAddFName.Text;
                emp.LastName = txtAddLName.Text;
                emp.Email = txtAddEmail.Text;
                emp.DOB = Convert.ToDateTime(txtAddDOB.Text);
                emp.Phone = txtAddPhone.Text;

                // Call to DataHandler, then off to RegexCheck and finally StoredProcedure
                if (RegexCheck.Checker(emp) == "Pass")
                {
                    // Add employee to DB and store the result
                    int success = DataHandler.InsertEmployee(emp);

                    // Display the result
                    if (success == (int)SuccessEnum.Success)
                    {
                        output += "Added Employee to the DataBase!\n";
                        // Reset Fields
                        txtAddFName.Text = "";
                        txtAddLName.Text = "";
                        txtAddEmail.Text = "";
                        txtAddDOB.Text = "";
                        txtAddPhone.Text = "";
                    }
                    else
                    {
                        output += "Failed to add Employee to the DataBase!\n";
                    }
                }
                // Display Regex fail message
                else
                {
                    output += RegexCheck.Checker(emp) + "\n";
                }

                MessageBox.Show(output, "Result");

                // Update List with new Employee
                FillListEmp();
            }
        }

        private void listboxUpdate_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Split date and time so we can set just date
            string[] date = listServ.EmpList[listboxUpdate.SelectedIndex].DOB.ToString().Split(null);
            // Set details from DataSet
            lblUpdateEmpIDText.Text = listServ.EmpList[listboxUpdate.SelectedIndex].EmpID.ToString();
            txtUpdateFName.Text = listServ.EmpList[listboxUpdate.SelectedIndex].FirstName;
            txtUpdateLName.Text = listServ.EmpList[listboxUpdate.SelectedIndex].LastName;
            txtUpdateEmail.Text = listServ.EmpList[listboxUpdate.SelectedIndex].Email;
            // Reverse day and month format since it's reversed in the DB
            // date[0] = date; date[1] = time;
            //txtUpdateDOB.Text = RegexCheck.MDYToDMY(date[0].ToString());
            txtUpdateDOB.Text = date[0].ToString();
            //txtUpdateDOB.Text = listServ.EmpList[listboxUpdate.SelectedIndex].DOB.ToString();
            txtUpdatePhone.Text = listServ.EmpList[listboxUpdate.SelectedIndex].Phone;
            txtUpdateWorkDate.Text = monthCalendar.TodayDate.ToShortDateString();
        }

        private void cmbEmployee_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Split date and time so we can set just the date
            //string[] date = listServ.EmpList[cmbEmployee.SelectedIndex].DOB.Split(null);
            // Set details from DataSet
            emp.EmpID = listServ.EmpList[cmbEmployee.SelectedIndex].EmpID;
            emp.FirstName = listServ.EmpList[cmbEmployee.SelectedIndex].FirstName;
            emp.LastName = listServ.EmpList[cmbEmployee.SelectedIndex].LastName;
            emp.Email = listServ.EmpList[cmbEmployee.SelectedIndex].Email;
            // Reverse day and month format since it's reversed in the DB
            // date[0] = date; date[1] = time;
            //emp.DOB = RegexCheck.MDYToDMY(date[0].ToString());
            //emp.DOB = date[0].ToString();
            emp.DOB = Convert.ToDateTime(listServ.EmpList[cmbEmployee.SelectedIndex].DOB);
            emp.Phone = listServ.EmpList[cmbEmployee.SelectedIndex].Phone;
            //List<EmpHours> empHours = new List<EmpHours>();
            // Get employees HoursList
            emp.HoursList = DataHandler.GetEmpHours(emp);
            // Display Employee and hours info
            rtxtReport.Text = emp.ToString() + emp.Report();
        }

        private void btnUpdateEmployee_Click(object sender, EventArgs e)
        {
            string output = "";
            DialogResult result = MessageBox.Show("Do you want to update employee information?", "Update employee?", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                // Remember which employee was selected
                int selected = listboxUpdate.SelectedIndex;

                // REALLY horrible solution..
                //-----------------------------------------
                IFormatProvider culture = new System.Globalization.CultureInfo("fr-FR", true);
                DateTime dt2 = new DateTime();
                try
                {
                    dt2 = DateTime.Parse(txtUpdateDOB.Text, culture, System.Globalization.DateTimeStyles.AssumeLocal);
                }
                catch (Exception)
                {
                    dt2 = new DateTime(0001, 01, 01);
                    // Had to change regex dateformat to not accept a year below 1000
                }
                //-----------------------------------------

                // Read inputs
                emp.EmpID = int.Parse(lblUpdateEmpIDText.Text);
                emp.FirstName = txtUpdateFName.Text;
                emp.LastName = txtUpdateLName.Text;
                emp.Email = txtUpdateEmail.Text;
                //emp.DOB = Convert.ToDateTime(txtUpdateDOB.Text);
                emp.DOB = dt2;
                emp.Phone = txtUpdatePhone.Text;

                // Call to DataHandler, then off to RegexCheck and finally StoredProcedure
                if (RegexCheck.Checker(emp) == "Pass")
                {
                    // Add employee to DB and store the result
                    int success = DataHandler.UpdateEmployee(emp);

                    // Display the result
                    if (success == (int)SuccessEnum.Success)
                    {
                        output += "Updated Employee Information!\n";

                    }
                    else
                    {
                        output += "Failed to update Employee Information!\n";
                    }
                }
                // Display Regex fail message
                else
                {
                    output += RegexCheck.Checker(emp);
                }

                MessageBox.Show(output, "Result");

                // Update List to include new Employee
                FillListEmp();
                // FillList resets the selected Employee
                // So we set it back using the index
                listboxUpdate.SelectedIndex = selected;
            }
        }

        private void btnUpdateHours_Click(object sender, EventArgs e)
        {
            string output = "";
            DialogResult result = MessageBox.Show("Do you want update employee hours?", "Update hours?", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                // Read input
                emp.EmpID = int.Parse(lblUpdateEmpIDText.Text);
                // Create new EmpHours object
                EmpHours empHours = new EmpHours(emp);
                empHours.Date = Convert.ToDateTime(txtUpdateWorkDate.Text);
                empHours.Hours = txtUpdateHours.Text;

                // Call to DataHandler, then off to RegexCheck and finally StoredProcedure
                if (RegexCheck.Checker(empHours) == "Pass")
                {
                    // Add employee to DB and store the result
                    int success = DataHandler.InsertHours(empHours);

                    // Display the result
                    if (success == (int)SuccessEnum.Success)
                    {
                        output += "Added Employee Hours to the DataBase!\n";
                    }
                    else
                    {
                        output += "Failed to add Employee Hours to the DataBase!\n";
                    }
                }
                // Regex fail message
                else
                {
                    output += RegexCheck.Checker(empHours) + "\n";
                }

                MessageBox.Show(output, "Result");

                txtUpdateHours.Text = "";
                txtUpdateWorkDate.Text = monthCalendar.TodayDate.ToShortDateString();
            }
        }

        private void monthCalendar_DateChanged(object sender, DateRangeEventArgs e)
        {
            // Select day by calendar
            string date = monthCalendar.SelectionRange.Start.ToShortDateString();
            // Select a single day
            monthCalendar.MaxSelectionCount = 1;
            txtUpdateWorkDate.Text = date;
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string output = "Incorrect Password!";
            // Create new Login object
            LoginUser user = new LoginUser();
            // Read input
            user.UserName = txtUserName.Text;
            user.Password = txtPassword.Text;

            // Attempt to Log in
            //string result = DataHandler.SysLogin(userName, password);
            int success = DataHandler.HashLogin(user);

            // RegexCheck
            if (RegexCheck.Password(user.Password) == true)
            {
                if (success == (int)SuccessEnum.Success)
                {
                    currentUser = user.UserName;
                    // Call to method to turn on or off login buttons
                    LoginItems("off");
                    lblWelcome.Text = "Welcome, " + user.UserName + "!";
                    output = "Welcome, " + user.UserName + "!";
                    tabControl1.Show();
                    // Clear UserName and Password
                    txtUserName.Text = "";
                    txtPassword.Text = "";
                }
            }
            // Display Result
            MessageBox.Show(output, "Login");
            // Reset Password txtBox
            txtPassword.Text = "";
        }

        private void btnNewLogin_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Do you want to create a new login?", "Create new login?", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                string output = "";
                // Check passwords are matching
                if (txtNewUserConfirmPassword.Text == txtNewUserPassword.Text)
                {
                    // Create new LoginUser object
                    LoginUser user = new LoginUser();
                    user.UserName = txtNewUser.Text;
                    user.Password = txtNewUserConfirmPassword.Text;
                    // RegexCheck
                    if (RegexCheck.Password(user.Password) == true)
                    {
                        int success = DataHandler.NewLogin(user);

                        if (success == (int)SuccessEnum.Success)
                        {
                            output += "Added new Login User!";
                        }
                        else
                        {
                            output += "Failed to add a new user!";
                        }
                    }
                    else
                    {
                        output += "Incorrect Password Format!\n4-12 characters.\nLetters and numbers only.";
                    }

                    // Reset textboxes
                    txtNewUser.Text = "";
                    txtNewUserConfirmPassword.Text = "";
                    txtNewUserPassword.Text = "";
                }
                else
                {
                    output += "Passwords don't match!";
                }
                MessageBox.Show(output, "Result");
            }
        }

        private void btnUpdatePassword_Click(object sender, EventArgs e)
        {
            string output = "";
            // Create new Login object
            LoginUser user = new LoginUser();

            DialogResult result = MessageBox.Show("Do you want to update your password?", "Update password?", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                // Check passwords are matching

                if (txtNewPassword.Text == txtConfirmNewPassword.Text)
                {
                    // Update password
                    user.UserName = currentUser;
                    user.Password = txtOldPassword.Text;
                    user.NewPassword = txtNewPassword.Text;
                    if (RegexCheck.Password(user.NewPassword) == true)
                    {
                        int success = DataHandler.UpdatePassword(user);
                        if (success == (int)SuccessEnum.Success)
                        {
                            output = "Updated Password!";
                        }
                        else
                        {
                            output = "Failed to update Password!";
                        }
                    }
                    else
                    {
                        output += "Incorrect Password Format!"; 
                    }

                    // Display result
                    MessageBox.Show(output, "Result");
                    // Reset textboxes
                    txtOldPassword.Text = "";
                    txtNewPassword.Text = "";
                    txtConfirmNewPassword.Text = "";
                }
                else
                {
                    MessageBox.Show("New passwords don't match!","Error");
                }
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Do you wish to exit?", "Exit?", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                Close();
            }
        }

        private void btnLogOut_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Do you want to log out?", "Log out?", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                tabControl1.Hide();
                LoginItems("on");
                txtUserName.Text = currentUser;
                txtPassword.Text = "";
                MessageBox.Show("You've sucessfully logged out.", "LogOut");
            }
        }
        public void LoginItems(string choice)
        {
            if (choice == "on")
            {
                lblETS.Show();
                lblUserName.Show();
                lblPassword.Show();
                txtUserName.Show();
                txtPassword.Show();
                btnLogin.Show();
                btnExit.Show();
                pnlLogin.Show();
            }
            else
            {
                lblETS.Hide();
                lblUserName.Hide();
                lblPassword.Hide();
                txtUserName.Hide();
                txtPassword.Hide();
                btnLogin.Hide();
                btnExit.Hide();
                pnlLogin.Hide();
            }
        }

        private void txtPassword_KeyDown(object sender, KeyEventArgs e)
        {
            // Login in by pressing enter while in password textbox
            if (e.KeyValue == (char)Keys.Enter)
            {
                //MessageBox.Show("Enter Pressed!");
                btnLogin_Click(sender, e);
            }

        } 
    }
}
