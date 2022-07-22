using Dapper.Contrib.Extensions;

namespace Focus.Repository.Models
{
    /// <summary>
    /// ����������ñ�
    /// </summary>
    [Table("SysProcessAudit")]
    public class SysProcessAudit : BaseEntity
    {
        ///<summary>
        ///Id,����
        ///<summary>
        [Key]
        public int SysProcessAuditId {get;set;}
        ///<summary>
        ///�������ñ�Id
        ///<summary>
        public int ProcessConfigId {get;set;}
        ///<summary>
        ///�����
        ///<summary>
        public string? AuditUser {get;set;}
        ///<summary>
        ///��˸�λ��Code����Id��
        ///<summary>
        public string? AuditPosition {get;set;}
        ///<summary>
        ///��˽�ɫ��Code����Id��
        ///<summary>
        public string? AuditRole {get;set;}
        ///<summary>
        ///����ʱ��
        ///<summary>
        public DateTime CreateDate {get;set;}

    }
}
