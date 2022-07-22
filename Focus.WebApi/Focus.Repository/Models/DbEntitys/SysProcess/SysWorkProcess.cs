using Dapper.Contrib.Extensions;

namespace Focus.Repository.Models
{
    /// <summary>
    /// 业务流程关联表
    /// </summary>
    [Table("SysWorkProcess")]
    public class SysWorkProcess : BaseEntity
    {
        ///<summary>
        ///流程Id
        ///<summary>
        public int ProcessId {get;set;}
        ///<summary>
        ///系统业务类别Id，Tips：注册主表--1
        ///<summary>
        public int WorkType {get;set;}
        ///<summary>
        ///是否启用，默认为启用
        ///<summary>
        public bool IsEnable {get;set;}
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
