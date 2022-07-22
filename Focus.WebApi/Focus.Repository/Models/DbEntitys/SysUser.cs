using Dapper.Contrib.Extensions;
using System.ComponentModel;

namespace Focus.Repository.Models
{
    [Table("SysUser")]
    public class SysUser:BaseEntity
    {
        [Key]
        public int UserId { get; set; }
        public string UserCode { get; set; }
        [Description("用户名")]
        public string UserName { get; set; }
        [Description("用户密码")]
        public string UserPwd { get; set; }
        public string UserPwdTimesTamp { get; set; }
        public int? UserStatus { get; set; }
        public DateTime? LastLoginTime { get; set; }
        public int? JobId { get; set; }
        public DateTime? AddTime { get; set; }
        [Description("手机号码")]
        public string PhoneNumber { get; set; }
        public string CompanyName { get; set; }
        [Write(false)]
        public int? Valid { get; set; }
    }
}
