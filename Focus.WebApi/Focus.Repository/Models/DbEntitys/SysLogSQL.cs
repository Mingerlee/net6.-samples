using Dapper.Contrib.Extensions;

namespace Focus.Repository.Models
{
    /// <summary>
    /// 系统SQL日志表
    /// </summary>
    [Table("SysSqlLog")]
    public class SysLogSQL: BaseEntity
    {
        /// <summary>
        /// 日志编号
        /// </summary>
        public Guid LogCode { get; set; }
        /// <summary>
        /// 用户编号
        /// </summary>
        public string? UserCode { get; set; }
        /// <summary>
        /// 日志记录时间（默认为添加时间）
        /// </summary>
        [Write(false)]
        public DateTime? LogTime { get; set; }
        /// <summary>
        /// SQL语句内容
        /// </summary>
        public string? LogContent { get; set; }
        /// <summary>
        /// IP地址
        /// </summary>
        public string? LogIpAddress { get; set; }
        /// <summary>
        /// 执行时间（记录执行效率）(单位：毫秒)
        /// </summary>
        public decimal? ExecTime { get; set; }

    }
}
