using Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.UserManager
{
    public class UserContext
    {
        public static UserContext Current
        {
            get
            {
                return Context.RequestServices.GetService(typeof(UserContext)) as UserContext;
            }

        }
        private static Microsoft.AspNetCore.Http.HttpContext Context
        {
            get
            {
                return Utilities.HttpContextHelper.Current;
            }
        }
        public UserToken UserInfo
        {
            get
            {
                return Context.User.GetToken();
            }
        }
    }
}
