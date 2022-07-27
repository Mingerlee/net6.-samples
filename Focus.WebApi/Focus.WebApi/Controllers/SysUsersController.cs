using Focus.IService;
using Focus.Repository.Models;
using Focus.Service.Validations;
using Focus.WebApi.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Focus.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SysUsersController : ControllerBase
    {
        private readonly ISysUserService _userService;
        private readonly ILogger<SysUsersController> _logger;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userService"></param>
        /// <param name="logger"></param>
        public SysUsersController(ILogger<SysUsersController> logger, ISysUserService userService)
        {
            _userService = userService;
            _logger = logger;
        }
        [HttpPost("Register01"), AllowAnonymous, ParamValidate(typeof(RegisterSysUserValidation))]
        public async Task<IActionResult> RegisterSysUsers(RegisterSysUser registerSysUser)
        {
            return Ok(await _userService.RegisterSysUsers(registerSysUser));
        }
    }
}
