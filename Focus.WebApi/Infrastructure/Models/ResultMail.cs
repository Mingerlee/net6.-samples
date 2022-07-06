using Infrastructure.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Models
{
    public class ResultMail
    {
        public ResultMail() 
        {
            status = MailStatus.Success; 
        }
        public ResultMail(string msg)
        {
            ErrorMsg = msg;
        }

        /// <summary>
        /// 邮件发送状态 
        /// </summary>
        public MailStatus status { get; set; }
        /// <summary>
        /// 错误消息
        /// </summary>
        public string ErrorMsg { get; set; }
    }
}
