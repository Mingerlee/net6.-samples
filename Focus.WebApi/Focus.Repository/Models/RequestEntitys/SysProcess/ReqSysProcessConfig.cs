using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Focus.Repository.Models
{
    public class ReqSysProcessConfig
    {
        public int ProcessStep { get; set; }
        public SysAuditMethod AuditMethod { get; set; }
        public SysAuditType AuditType { get; set; }
        public bool IsEnable { get; set; }
        public List<ReqSysProcessAudit> reqSysProcessAudits { get; set; }
    }
}
