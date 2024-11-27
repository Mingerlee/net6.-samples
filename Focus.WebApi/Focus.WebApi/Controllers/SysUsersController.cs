using Focus.IService;
using Focus.Repository.Models;
using Focus.Service.Validations;
using Focus.WebApi.Filters;
using Infrastructure.Config;
using Infrastructure.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;

namespace Focus.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "v1")]
    public class SysUsersController : ControllerBase
    {
        private readonly ISysUserService _userService;
        private readonly TokenManagement _tokenManagement;
        private readonly ILogger<SysUsersController> _logger;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userService"></param>
        /// <param name="logger"></param>
        /// <param name="tokenManagement"></param>
        public SysUsersController(ILogger<SysUsersController> logger, ISysUserService userService, TokenManagement tokenManagement)
        {
            _userService = userService;
            _logger = logger;
            _tokenManagement = tokenManagement;
        }
        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="registerSysUser"></param>
        /// <returns></returns>
        [HttpPost("Register01"), AllowAnonymous, ParamValidate(typeof(RegisterSysUserValidation))]
        public async Task<IActionResult> RegisterSysUsers(RegisterSysUser registerSysUser)
        {
            return Ok(await _userService.RegisterSysUsers(registerSysUser));
        }
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        [HttpPost("Login"), AllowAnonymous, ParamValidate(typeof(LoginValidation))]
        public async Task<IActionResult> Login(ReqSysLogin login)
        {
            SysUser user = await _userService.Login(login);
            if (user!=null) AddAuthorization(user);
            return Ok();
        }


        private void AddAuthorization(SysUser user)
        {
            var userToken = new UserToken
            {
                IP = this.GetClientIP(),
                IMEI = this.GetIMEI(),
                Channel = "",
                Version = "v1",
                UID = "1",
                UserCode = user.UserCode,
                Name = user.UserName,
                Email = user.Email,
                Mobile = user.PhoneNumber,
                MobileArea = "86",
                Account = 1,
                Role="Administrator"
            };

            string token = userToken.Serialization(_tokenManagement);
            HttpContext.Response.Headers.Add("Authorization", new StringValues(token));
        }
    }
}
