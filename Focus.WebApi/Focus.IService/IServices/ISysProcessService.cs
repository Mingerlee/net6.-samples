using Focus.Repository.Models;
using Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Focus.IService.IServices
{
    public interface ISysProcessService
    {
        Task<ResultModel<ListPage<SysProcess>>> GetSysProcessList(SysProcessSc sc);
        Task<ResultModel<string>> AddSysProcess(ReqSysProcess req);
    }
}
