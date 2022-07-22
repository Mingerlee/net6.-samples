using Focus.Repository.DBContext;
using Focus.Repository.Models;
using Infrastructure.Models;
using Infrastructure.UserManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Focus.Repository.DBManager
{
    public class SysLogSQLManager
    {
        public static void WriteSqlLog(string content,decimal exectime)
        {
            using (DbHelper db=DbHelperFactory.Create()) 
            {
                var user = UserContext.Current.UserInfo;
                SysLogSQL log = new SysLogSQL {
                    LogCode = Guid.NewGuid().GetNextGuid(),
                    UserCode = user.UserCode,
                    LogContent = content,
                    LogIpAddress = user.IP,
                    ExecTime = exectime
                };
                db.Insert(log);
            }
        }

    }
}
