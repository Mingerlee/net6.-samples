using Dapper.Contrib.Extensions;

namespace Focus.Repository.Models
{
    /// <summary>
    /// ҵ��������˱�
    /// </summary>
    [Table("SysWorkAudit")]
    public class SysWorkAudit : BaseEntity
    {
        ///<summary>
        ///����������ñ�Id
        ///<summary>
        public int SysProcessAuditId {get;set;}
        ///<summary>
        ///ϵͳҵ��Id��Tips��ע�������Id
        ///<summary>
        public int WorkId {get;set;}
        ///<summary>
        ///ϵͳҵ�����Id��Tips��ע������--1
        ///<summary>
        public SysProcessWorkType WorkType {get;set;}
        ///<summary>
        ///���ʱ�䣬Ĭ��Ϊ��ǰϵͳʱ��
        ///<summary>
        public DateTime? AuditTime {get;set;}
        ///<summary>
        ///���״̬���ύ����ˣ����ͨ��������
        ///<summary>
        public SysProcessAuditStatus? AuditStatus {get;set;}
        ///<summary>
        ///����ԭ��
        ///<summary>
        public string? RejectionReason {get;set;}
        ///<summary>
        ///������
        ///<summary>
        public string Creator {get;set;}
        ///<summary>
        ///����ʱ��
        ///<summary>
        public DateTime CreateDate {get;set;}

    }
}
