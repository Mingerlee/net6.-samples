using Dapper.Contrib.Extensions;

namespace Focus.Repository.Models
{
    /// <summary>
    /// ҵ�����̹�����
    /// </summary>
    [Table("SysWorkProcess")]
    public class SysWorkProcess : BaseEntity
    {
        ///<summary>
        ///����Id
        ///<summary>
        public int ProcessId {get;set;}
        ///<summary>
        ///ϵͳҵ�����Id��Tips��ע������--1
        ///<summary>
        public int WorkType {get;set;}
        ///<summary>
        ///�Ƿ����ã�Ĭ��Ϊ����
        ///<summary>
        public bool IsEnable {get;set;}
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
