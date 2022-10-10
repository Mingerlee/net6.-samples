using Infrastructure.Config;
using Infrastructure.Models;
using Infrastructure.UserManager;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Focus.IService;
using Focus.Repository.Models;
using Focus.Repository.DBContext;
using Focus.WebApi.Attributes;
using Infrastructure.CacheManager;
using Focus.Service.Validations;
using FluentValidation.Results;
using Infrastructure.Enums;
using Infrastructure.Utilities;
using Newtonsoft.Json;
using Focus.WebApi.Filters;
using AutoMapper;
using Focus.WebApi.Extensions;

namespace Focus.WebApi.Controllers
{
    [Route("api/[controller]"), Authorize]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly TokenManagement _tokenManagement;
        private readonly ISysUserService _userService;
        private readonly ILogger<TestController> _logger;
        private readonly IMapper _mapper;
        [Autowired]
        private ITest _test { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tokenManagement"></param>
        /// <param name="userService"></param>
        /// <param name="logger"></param>
        /// <param name="mapper"></param>
        public TestController(AutowiredService autowiredService,ILogger<TestController> logger, IOptions<TokenManagement> tokenManagement, ISysUserService userService, IMapper mapper)
        {
            autowiredService.Autowired(this);//注入服务
            _tokenManagement = tokenManagement.Value;
            _userService = userService;
            _logger = logger;
            _mapper = mapper;
        }
        /// <summary>
        /// 登录授权(默认角色为：Administrator)
        /// </summary>
        /// <returns></returns>
        [HttpPost, Route("W102"), AllowAnonymous]
        public IActionResult Login()
        {
            AddAuthorization();
            return Ok(new ResultModel<string>("Success"));
        }
        ///// <summary>
        ///// 测试Async 获取数据库数据
        ///// </summary>
        ///// <param name="id"></param>
        ///// <returns></returns>
        //[HttpGet, Route("W103"), AllowAnonymous]
        //public async Task<IActionResult> Get(int id)
        //{
        //    return Ok(new ResultModel<SysUser>(await _userService.GetSysUser(id)));
        //}
        ///// <summary>
        ///// FluentValidation 示例
        ///// </summary>
        ///// <param name="registerSysUser"></param>
        ///// <returns></returns>
        //[HttpPost("W104"), AllowAnonymous, ParamValidate(typeof(RegisterSysUserValidation))]
        //public async Task<IActionResult> GetSysUser(RegisterSysUser registerSysUser)
        //{
        //    //SysUserValidation validationRules = new SysUserValidation();
        //    //ValidationResult vResult = validationRules.Validate(sysUser);
        //    //if (!vResult.IsValid)
        //    //{
        //    //    //Tips:验证不通过，输出验证信息
        //    //    ResultModel<List<ValidationFailureResult>> mResult = new ResultModel<List<ValidationFailureResult>> {
        //    //        Status = 0,
        //    //        Data= vResult.Errors.ToValidationFailureResultList(typeof(SysUser)),
        //    //        ErrorCode = ResponseCode.sys_verify_failed
        //    //    };
        //    //    return Ok(mResult);
        //    //}
        //    //验证通过执行 业务逻辑
        //    return Ok(await _userService.GetSysUser(registerSysUser));
        //}
        ///// <summary>
        ///// CustomerExceptionFilter 示例 （需登录）
        ///// </summary>
        ///// <param name="id"></param>
        ///// <returns></returns>
        //[HttpGet, Route("W105")]
        //public async Task<IActionResult> GetSysUserById(int id)
        //{
        //    int i = 0, j = 10;
        //    int x = j / i;
        //    return Ok(new ResultModel<SysUser>(await _userService.GetSysUsers(id)));
        //}
        /// <summary>
        /// 角色权限示例 Administrator
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("W106"), AuthorizeRoles("Administrator")]
        public IActionResult TestAuthorizeRoles()
        {
            //获取随机数
            string guid = Guid.NewGuid().GetNextGuid().ToString();
            return Ok("Success");
        }
        /// <summary>
        /// 角色权限示例 Public
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("W107"), AuthorizeRoles("Public")]
        public IActionResult TestAuthorizeRoles1()
        {
            //获取当前登录用户信息
            var UserToken = UserContext.Current.UserInfo;
            return Ok("Success");
        }
        /// <summary>
        /// FluentValidation 示例（没有特性）
        /// </summary>
        /// <param name="sysUser"></param>
        /// <returns></returns>
        [HttpPost("W108"), AllowAnonymous]
        public IActionResult ValidateUser(SysUser sysUser)
        {
            if (!sysUser.IsValid())
            {
                return Ok("该手机号码已注册！");
            }
            return Ok("没有FluentValidation验证");
        }

        /// <summary>
        /// 设置Session
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("SetSession"), AllowAnonymous]
        public IActionResult SetSession()
        {
            HttpContextHelper.Current.Session.SetString("MyTset", (DateTime.Now.DiffDays(new DateTime(2022, 7, 17))).ToString());
            return Ok("Success");
        }
        /// <summary>
        /// 获取Session
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("GetSession"), AllowAnonymous]
        public IActionResult GetSession()
        {
            string result = HttpContextHelper.Current.Session.GetString("MyTset") ?? "该Session不存在或者已被移除！";
            HttpContextHelper.Current.Session.Remove("MyTset");
            return Ok(result);
        }
        /// <summary>
        /// 获取验证码
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("GetVerifyCode"), AllowAnonymous]
        public IActionResult GetVerifyCode()
        {
            string vCode = "";
            byte[] result = VerifyCodeHelper.VerifyCodeBytes(ref vCode);
            HttpContextHelper.Current.Session.SetString("VerifyCode", vCode);
            return Ok(new ResultModel<byte[]>(result));
        }
        /// <summary>
        /// 对比实体数据是否修改
        /// </summary>
        /// <returns></returns>
        [HttpGet("W109"), AllowAnonymous]
        public IActionResult CompareModel()
        {
            //CompareHelper.CompareToString<SysUser>();
            SysUser user1 = new SysUser
            {
                UserId = 1,
                UserName = "张三",
                IsInternal = true,
                RegDate = DateTime.Now
            };
            SysUser user2 = new SysUser
            {
                UserId = 1,
                UserName = "张三",
                IsInternal = false,
                RegDate = DateTime.Now
            };
            bool s = user1.IsModify(user2);
            var sf = user2.CompareModel(user1);
            return Ok();
        }

        [HttpGet("W110"), AllowAnonymous]
        public IActionResult TestHttpContext()
        {
            Console.WriteLine(HttpContext.TraceIdentifier);
            return Ok();
        }
        /// <summary>
        /// AutoMapper 示例 DB Entity To ViewModel
        /// </summary>
        /// <returns></returns>
        [HttpGet("W111"), AllowAnonymous]
        public IActionResult TestAutoMapper()
        {
            SysUser sysUser = new SysUser
            {
                UserName = "张三",
                PhoneNumber = "15200000000",
                Email = "1234@qq.com"
            };
            RegisterSysUser RegUser = _mapper.Map<RegisterSysUser>(sysUser);
            return Ok(RegUser);
        }

        /// <summary>
        /// AutoMapper 示例 ViewModel To DB Entity
        /// </summary>
        /// <returns></returns>
        [HttpGet("W112"), AllowAnonymous]
        public IActionResult TestAutoMapper2()
        {
            RegisterSysUser regUser = new RegisterSysUser
            {
                UserName = "李四",
                PhoneNumber = "15200000001",
                Email = "123456@qq.com"
            };
            SysUser sysUser = _mapper.Map<SysUser>(regUser);
            return Ok(sysUser);
        }
        /// <summary>
        /// 属性注入示例
        /// </summary>
        /// <returns></returns>
        [HttpGet("W113"), AllowAnonymous]
        public IActionResult TestGet()
        {
            _test.Get();
            return Ok();
        }

        private void AddAuthorization()
        {
            var userToken = new UserToken
            {
                IP = this.GetClientIP(),
                IMEI = this.GetIMEI(),
                Channel = "",
                Version = "v1",
                UID = "1",
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
