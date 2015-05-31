using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//..
using ETS.data;
using ETS.logic;
using ETS.logic.Domain;

namespace ETS.service
{
    public class LoginService
    {
        public static void ConnectionString()
        {
            // Encoded connection string
            string conString = "Data Source=.;Initial Catalog=EmployeeInformation;User ID=sa;Password=********";
            // Decode
            conString = Hasher.Decode(conString);
            // Set it in both DB classes
            LoginDao.SetCon(conString);
            EmployeeDao.SetCon(conString);
        }
        public static int NewLogin(LoginUser user)
        {
            try
            {
                // Get salt
                user.Salt = Hasher.SetSalt();
                // Encode
                user.PasswordHash = Hasher.EncodeFull(user.Salt, user.Password);

                // Check that there is no duplicate username in the DB
                if (LoginDao.SearchUserName(user.UserName) == (int)SuccessEnum.Success)
                {
                    // Add new Login to the DB
                    return LoginDao.NewLogin(user);
                }
                else
                {
                    // Return Search fail message
                    return LoginDao.SearchUserName(user.UserName);
                }
            }
            catch (Exception)
            {

                return (int)SuccessEnum.Fail;
            }

        }

        public static int HashLogin(LoginUser user)
        {
            try
            {
                // Get salt
                user.Salt = GetSalt(user.UserName);

                // Hash
                user.PasswordHash = Hasher.EncodeFull(user.Salt, user.Password);

                // Log in
                if (LoginDao.Login(user) == (int)SuccessEnum.Success)
                {
                    return (int)SuccessEnum.Success;
                }
                else
                {
                    return (int)SuccessEnum.Fail;
                }
            }
            catch (Exception)
            {

                return (int)SuccessEnum.Fail;
            }

        }

        public static int UpdatePassword(LoginUser user)
        {
            string result = "";
            // Check log in with old password
            if (HashLogin(user) == (int)SuccessEnum.Success)
            {
                result = "Pass";
            }

            //Send PasswordHash && SaltHash to DataBase for saving
            if (result == "Pass")
            {
                try
                {
                        // Get salt hash
                        user.Salt = Hasher.SetSalt();
                        // Encode password + salt
                        user.PasswordHash = Hasher.EncodeFull(user.Salt, user.NewPassword);
                        return LoginDao.UpdatePassword(user);
                }
                catch (Exception)
                {

                    return (int)SuccessEnum.Fail;
                }
            }
            else
            {
                return (int)SuccessEnum.Fail;
            }
        }
        private static string GetSalt(string userName)
        {
            // Retrieve salt hash from database
            string salt = LoginDao.GetSalt(userName);
            return salt;
        }
    }
}
