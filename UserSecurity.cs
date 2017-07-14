using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApiDEMO.Repository;

namespace WebApiDEMO
{
    public static class UserSecurity
    {
        public static bool Login(string userName, string password)
        {
            return UserDBContext.ValidateLogin(userName, password);
        }
    }
}