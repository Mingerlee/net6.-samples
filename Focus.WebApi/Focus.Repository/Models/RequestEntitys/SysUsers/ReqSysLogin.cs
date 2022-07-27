using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Focus.Repository.Models
{
    public class ReqSysLogin
    {
        public string LoginName { get; set; }
        public string LoginPwd { get; set; }
        public string VerifyCode { get; set; }
    }
}
