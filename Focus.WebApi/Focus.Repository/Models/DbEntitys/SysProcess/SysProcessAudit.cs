using Dapper.Contrib.Extensions;

namespace Focus.Repository.Models
{
    /// <summary>
    /// 流程审核配置表
    /// </summary>
    [Table("SysProcessAudit")]
    public class SysProcessAudit : BaseEntity
    {
        ///<summary>
        ///Id,主键
        ///<summary>
        [Key]
        public int SysProcessAuditId {get;set;}
        ///<summary>
        ///流程配置表Id
        ///<summary>
        public int ProcessConfigId {get;set;}
        ///<summary>
        ///审核人
        ///<summary>
        public string? AuditUser {get;set;}
        ///<summary>
        ///审核岗位（Code或者Id）
        ///<summary>
        public string? AuditPosition {get;set;}
        ///<summary>
        ///审核角色（Code或者Id）
        ///<summary>
        public string? AuditRole {get;set;}
        ///<summary>
        ///创建时间
        ///<summary>
        public DateTime CreateDate {get;set;}

    }
}
