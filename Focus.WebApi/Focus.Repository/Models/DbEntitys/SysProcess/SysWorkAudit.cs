using Dapper.Contrib.Extensions;

namespace Focus.Repository.Models
{
    /// <summary>
    /// 业务流程审核表
    /// </summary>
    [Table("SysWorkAudit")]
    public class SysWorkAudit : BaseEntity
    {
        ///<summary>
        ///流程审核配置表Id
        ///<summary>
        public int SysProcessAuditId {get;set;}
        ///<summary>
        ///系统业务Id，Tips：注册主表的Id
        ///<summary>
        public int WorkId {get;set;}
        ///<summary>
        ///系统业务类别Id，Tips：注册主表--1
        ///<summary>
        public SysProcessWorkType WorkType {get;set;}
        ///<summary>
        ///审核时间，默认为当前系统时间
        ///<summary>
        public DateTime? AuditTime {get;set;}
        ///<summary>
        ///审核状态：提交待审核，审核通过，驳回
        ///<summary>
        public SysProcessAuditStatus? AuditStatus {get;set;}
        ///<summary>
        ///驳回原因
        ///<summary>
        public string? RejectionReason {get;set;}
        ///<summary>
        ///创建人
        ///<summary>
        public string Creator {get;set;}
        ///<summary>
        ///创建时间
        ///<summary>
        public DateTime CreateDate {get;set;}

    }
}
