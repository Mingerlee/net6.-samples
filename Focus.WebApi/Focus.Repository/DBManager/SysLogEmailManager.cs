using Focus.Repository.DBContext;
using Focus.Repository.Models.DbEntitys;
using Infrastructure.Models;
using Infrastructure.UserManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Focus.Repository.DBManager
{
    public class SysLogEmailManager
    {
        /// <summary>
        /// 记录邮件发送日志
        /// </summary>
        /// <param name="logEmail"></param>
        public static void WriteSysLogEmail(SysLogEmail logEmail)
        {
            using (DbHelper db = DbHelperFactory.Create())
            {
                var user = UserContext.Current.UserInfo;              
                db.Insert(logEmail);
            }
        }
    }
}
