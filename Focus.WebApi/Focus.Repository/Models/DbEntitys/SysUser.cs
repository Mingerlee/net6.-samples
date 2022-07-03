using Dapper.Contrib.Extensions;

namespace Focus.Repository.Models.DbEntitys
{
    [Table("SysUser")]
    public class SysUser:BaseEntity
    {
        [Key]
        public int UserId { get; set; }
        public string UserCode { get; set; }
        public string UserName { get; set; }
        public string UserPwd { get; set; }
        public string UserPwdTimesTamp { get; set; }
        public int? UserStatus { get; set; }
        public DateTime? LastLoginTime { get; set; }
        public int? JobId { get; set; }
        public DateTime? AddTime { get; set; }
        public string PhoneNumber { get; set; }
        public string CompanyName { get; set; }
        [Write(false)]
        public int? Valid { get; set; }
    }
}
