using Dapper.Contrib.Extensions;

namespace Focus.Repository.Models
{
    /// <summary>
    /// �������ñ�
    /// </summary>
    [Table("SysProcessConfig")]
    public class SysProcessConfig : BaseEntity
    {
        ///<summary>
        ///-�������ñ�Id������
        ///<summary>
        [Key]
        public int ProcessConfigId {get;set;}
        ///<summary>
        ///����Id
        ///<summary>
        public int ProcessId {get;set;}
        ///<summary>
        ///���̲���
        ///<summary>
        public int ProcessStep {get;set;}
        ///<summary>
        ///��˷�ʽ����ָ����Ա��ˣ���ָ����λ����ָ����ɫ
        ///<summary>
        public SysAuditMethod AuditMethod {get;set;}
        ///<summary>
        ///�����𣺻�ǩ��һ��������ͬ����߾ܾ����ɣ�����ǩ���������������ͨ����
        ///<summary>
        public SysAuditType AuditType {get;set;}
        ///<summary>
        ///����ʱ��
        ///<summary>
        public DateTime CreateDate {get;set;}
        ///<summary>
        ///������
        ///<summary>
        public string Creator {get;set;}
        /// <summary>
        /// ����������� �б�
        /// </summary>
        [Write(false)]
        public List<SysProcessAudit> sysProcessAudits {get;set;}
    }
}
