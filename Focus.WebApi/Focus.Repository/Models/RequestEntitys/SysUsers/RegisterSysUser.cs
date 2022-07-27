using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Focus.Repository.Models
{
    public class RegisterSysUser
    {
        [Description("登录名")]
        public string LoginName { get; set; }
        [Description("登录密码")]
        public string LoginPwd { get; set; }
        [Description("确认登录密码")]
        public string ConfirmLoginPwd { get; set; }
        [Description("用户名称")]
        public string UserName { get; set; }
        [Description("手机号码")]
        public string? PhoneNubmer { get; set; }
        [Description("用户住址")]
        public string? Address { get; set; }
        [Description("用户邮箱")]
        public string? Email { get; set; }
    }
}
