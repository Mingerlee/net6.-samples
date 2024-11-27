using Focus.IService.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Focus.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "v1")]
    public class DynamicProxyController : ControllerBase
    {
        private readonly IDynamicProxyService _proxyService;
        private readonly IDynamicProxyService2 _proxyService2;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="proxyService"></param>
        /// <param name="proxyService2"></param>
        public DynamicProxyController(IDynamicProxyService proxyService, IDynamicProxyService2 proxyService2)
        {
            _proxyService = proxyService;
            _proxyService2 = proxyService2;
        }
        [HttpGet, Route("SayHello")]
        public IActionResult SayHello()
        {
            _proxyService.SayHello();
            return Ok();
        }
        [HttpGet, Route("SayHello2")]
        public IActionResult SayHello2()
        {
            _proxyService2.SayHello("I'm is OuyangMing!");
            return Ok();
        }
    }
}
