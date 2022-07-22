using Dapper.Contrib.Extensions;

namespace Focus.Repository.Models
{
    /// <summary>
    /// 流程主表
    /// </summary>
    [Table("SysProcess")]
    public class SysProcess : BaseEntity
    {
        ///<summary>
        ///流程Id 主键
        ///<summary>
        [Key]
        public int ProcessId { get; set; }
        ///<summary>
        ///流程名称
        ///<summary>
        public string ProcessName { get; set; }
        ///<summary>
        ///流程类型
        ///<summary>
        public SysProcessType ProcessType { get; set; }
        ///<summary>
        ///流程描述
        ///<summary>
        public string? ProcessDescription { get; set; }
        ///<summary>
        ///创建时间
        ///<summary>
        public DateTime CreateDate { get; set; }
        ///<summary>
        ///创建人
        ///<summary>
        public string Creator { get; set; }
        ///<summary>
        ///是否启用，默认为启用
        ///<summary>
        public bool IsEnable { get; set; }
        /// <summary>
        /// 相邻两个审核人为同一个人时，是否自动审核
        /// </summary>
        public bool IsAutoAudit { get; set; }
        
        /// <summary>
        /// 流程配置表列表
        /// </summary>
        [Write(false)]
        public List<SysProcessConfig>? sysProcessConfigs { get; set; }
    }
}
