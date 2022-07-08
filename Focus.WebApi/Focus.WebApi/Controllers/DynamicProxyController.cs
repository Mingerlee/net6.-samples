using Focus.IService.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Focus.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DynamicProxyController : ControllerBase
    {
        private readonly IDynamicProxyService _proxyService;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="proxyService"></param>
        public DynamicProxyController(IDynamicProxyService proxyService)
        {
            _proxyService = proxyService;
        }
        [HttpGet, Route("SayHello")]
        public IActionResult SayHello()
        {
            _proxyService.SayHello();
            return Ok();
        }
    }
}
