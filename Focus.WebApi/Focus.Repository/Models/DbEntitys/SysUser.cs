using Dapper.Contrib.Extensions;
using Infrastructure.Config;
using Infrastructure.Models;

namespace Focus.Repository.Models
{
    //[Table("SysUser")]
    //public class SysUser:BaseEntity
    //{
    //    [Key]
    //    public int UserId { get; set; }
    //    public string UserCode { get; set; }
    //    [Description("用户名")]
    //    public string UserName { get; set; }
    //    [Description("用户密码")]
    //    public string UserPwd { get; set; }
    //    public string UserPwdTimesTamp { get; set; }
    //    public int? UserStatus { get; set; }
    //    public DateTime? LastLoginTime { get; set; }
    //    public int? JobId { get; set; }
    //    public DateTime? AddTime { get; set; }
    //    [Description("手机号码")]
    //    public string PhoneNumber { get; set; }
    //    public string CompanyName { get; set; }
    //    [Write(false)]
    //    public int? Valid { get; set; }
    //}
    [Table("SysUser")]
    public class SysUser : BaseEntity
    {
        ///<summary>
        ///用户Id主键
        ///<summary>
        [Key]
        public int UserId { get; set; }
        ///<summary>
        ///用户编号
        ///<summary>
        public string? UserCode { get; set; }
        ///<summary>
        ///用户名称
        ///<summary>
        public string UserName { get; set; }
        ///<summary>
        ///手机号码
        ///<summary>
        [DBEntityVerification]
        public string? PhoneNumber { get; set; }
        ///<summary>
        ///用户住址
        ///<summary>
        public string? UserAddress { get; set; }
        ///<summary>
        ///邮件地址
        ///<summary>
        public string? Email { get; set; }
        ///<summary>
        ///是否内部用户
        ///<summary>
        public bool IsInternal { get; set; }
        ///<summary>
        ///注册日期
        ///<summary>
        [CompareValue]
        public DateTime RegDate { get; set; }

    }
}
