using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETS.logic.Domain
{
    public class LoginUser
    {
        public string UserName { get; set; }
        public string Salt { get; set; }
        public string SaltHash { get; set; }
        public string Password { get; set; }
        public string PasswordHash { get; set; }
        public string NewPassword { get; set; }
    }
}
