using Infrastructure.Config;
using Infrastructure.Models;
using Infrastructure.UserManager;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Focus.Service.IServices;
using Focus.Repository.Models.DbEntitys;
using Focus.Repository.DBContext;
using Focus.WebApi.Attributes;
using Infrastructure.CacheManager;

namespace Focus.WebApi.Controllers
{
    [Route("api/[controller]"), Authorize]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly TokenManagement _tokenManagement;
        private readonly ISysUserService _userService;
        private readonly ILogger<TestController> _logger;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tokenManagement"></param>
        /// <param name="userService"></param>
        /// <param name="logger"></param>
        public TestController(ILogger<TestController> logger,IOptions<TokenManagement> tokenManagement, ISysUserService userService)
        {
            _tokenManagement = tokenManagement.Value;
            _userService = userService;
            _logger = logger;
        }

        [HttpGet("Test")]
        public IActionResult Test() => new JsonResult("Testting......");

        [HttpPost, Route("W102"), AllowAnonymous]
        public IActionResult Login(SysUser user)
        {
            AddAuthorization();
            var s = new ResultModel<string>("Success");
            return Ok(s);
        }
        [HttpGet, Route("W103"), AllowAnonymous]
        public async Task<IActionResult> Get(int id)
        {
            //var uid = UserContext.Current.UserInfo.UID;
            //var s = Guid.NewGuid().GetNextGuid();
            return Ok(new ResultModel<SysUser>(await _userService.GetSysUser(id)));
        }
        [HttpPost("W104"), AllowAnonymous]
        public async Task<IActionResult> GetSysUser(SysUser sysUser)
        {
            int i = 0, j = 10;
            int x = j / i;
            //CacheContext.Cache.AddObject(DateTime.Now.ToString(), sysUser);//存入缓存
            //var uid = UserContext.Current.UserInfo.UID;
            //var s = Guid.NewGuid().GetNextGuid();
            return Ok(await _userService.GetSysUser(sysUser));
        }
        [HttpGet, Route("W105"), AllowAnonymous]
        public async Task<IActionResult> GetSysUserById(int id)
        {
            int i = 0, j = 10;
            int x = j / i;
            //var uid = UserContext.Current.UserInfo.UID;
            //var s = Guid.NewGuid().GetNextGuid();
            return Ok(new ResultModel<SysUser>(await _userService.GetSysUsers(id)));
        }
        private void AddAuthorization()
        {
            var userToken = new UserToken
            {
                IP = Infrastructure.Utilities.HttpContext.GetIp,
                IMEI = "",
                Channel = "",
                Version = "v1",
                UID ="1",
                UserCode = "U000001",
                Name = "张三",
                Email = "zhangsan@qq.com",
                Mobile = "15289890000",
                MobileArea = "86",
                Account = 1,
            };

            string token = userToken.Serialization(_tokenManagement);
            HttpContext.Response.Headers.Add("Authorization", new StringValues(token));
        }
    }
}
