using Infrastructure.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Focus.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "Focus")]
    public class LoggingTestController : ControllerBase
    {
        private readonly ILogger<LoggingTestController> _logger;
        public LoggingTestController(ILogger<LoggingTestController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// 登录授权(默认角色为：Administrator)
        /// </summary>
        /// <returns></returns>
        [HttpPost, Route("W1020"), AllowAnonymous]
        public bool Login()
        {
            
            _logger.LogError("测试错误日志！");
            return true;
            //return Ok(new ResultModel<string>("Success"));
        }
    }
}
