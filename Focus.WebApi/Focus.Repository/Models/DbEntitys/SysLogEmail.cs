using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Focus.Repository.Models
{
    [Table("SysLogEmails")]
    public class SysLogEmail: BaseEntity
    {
        [Key]
        public int LogEmailId { get; set; }
        public int TemplateEmailId { get; set; }
        public int? Key { get; set; }
        public int? KeyType { get; set; }
        public int TriggerByUserId { get; set; }
        public string AccessIP { get; set; }
        public string FromAddress { get; set; }
        public string ToAddresses { get; set; }
        public string CcAddresses { get; set; }
        public string BccAddresses { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public bool IsHTML { get; set; }
        public int AttachmentsCount { get; set; }
        public DateTime TimeStamp { get; set; }
        public bool IsSuccess { get; set; }
        public string ErrorMessage { get; set; }
        public string Body_Text { get; set; }  
    }
}
