using Focus.IService.IServices;
using Focus.Repository.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Focus.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "v1")]
    public class SysProcessController : ControllerBase
    {
        private readonly ISysProcessService _sysProcessService;
        public SysProcessController(ISysProcessService sysProcessService)
        {
            _sysProcessService=sysProcessService;
        }
        /// <summary>
        /// 获取所有流程列表
        /// </summary>
        /// <param name="sc"></param>
        /// <returns></returns>
        [HttpPost("GetSysProcessList")]
        public async Task<IActionResult> GetSysProcessList([FromBody] SysProcessSc sc) => Ok(await _sysProcessService.GetSysProcessList(sc)) ;
        /// <summary>
        /// 添加流程
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost("AddSysProcess")]
        public async Task<IActionResult> AddSysProcess([FromBody] ReqSysProcess req) => Ok(await _sysProcessService.AddSysProcess(req));
    }
}
