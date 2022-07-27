using Dapper.Contrib.Extensions;

namespace Focus.Repository.Models
{
    /// <summary>
    /// 流程配置表
    /// </summary>
    [Table("SysProcessConfig")]
    public class SysProcessConfig : BaseEntity
    {
        ///<summary>
        ///-流程配置表Id，主键
        ///<summary>
        [Key]
        public int ProcessConfigId {get;set;}
        ///<summary>
        ///流程Id
        ///<summary>
        public int ProcessId {get;set;}
        ///<summary>
        ///流程步骤
        ///<summary>
        public int ProcessStep {get;set;}
        ///<summary>
        ///审核方式：按指定人员审核，按指定岗位、按指定角色
        ///<summary>
        public SysAuditMethod AuditMethod {get;set;}
        ///<summary>
        ///审核类别：或签（一名审批人同意或者拒绝即可），会签（必须所有人审核通过）
        ///<summary>
        public SysAuditType AuditType {get;set;}
        ///<summary>
        ///创建时间
        ///<summary>
        public DateTime CreateDate {get;set;}
        ///<summary>
        ///创建人
        ///<summary>
        public string Creator {get;set;}
        /// <summary>
        /// 流程审核配置 列表
        /// </summary>
        [Write(false)]
        public List<SysProcessAudit> sysProcessAudits {get;set;}
    }
}
