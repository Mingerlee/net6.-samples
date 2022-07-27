using Dapper.Contrib.Extensions;

namespace Focus.Repository.Models
{
    [Table("SysLogin")]
    public class SysLogin:BaseEntity
    {
        ///<summary>
        ///登录Id主键
        ///<summary>
        [Key]
        public int LoginId {get;set;}
        ///<summary>
        ///用户Id主键
        ///<summary>
        public int UserId {get;set;}
        ///<summary>
        ///登录账号
        ///<summary>
        public string LoginName {get;set;}
        ///<summary>
        ///登录密码
        ///<summary>
        public string LoginPwd {get;set;}
        ///<summary>
        ///密码密钥
        ///<summary>
        public string EncryptSecret {get;set;}
        ///<summary>
        ///密码过期时间
        ///<summary>
        public DateTime PasswordExpire {get;set;}
        ///<summary>
        ///重试次数
        ///<summary>
        public int RetryNo {get;set;}
        ///<summary>
        ///每天最大重试次数
        ///<summary>
        public int MaxRetryNo {get;set;}
        ///<summary>
        ///重试日期
        ///<summary>
        public DateTime? RetryDate {get;set;}
        ///<summary>
        ///账户是否锁定
        ///<summary>
        public bool IsLocked {get;set;}
        ///<summary>
        ///账户是否激活
        ///<summary>
        public bool IsActive {get;set;}

    }
}
