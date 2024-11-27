using Infrastructure.Models;
using Infrastructure.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Focus.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "v1")]
    public class EMailController : ControllerBase
    {
        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="mail"></param>
        /// <returns></returns>
        [HttpPost("S101")]
        public IActionResult SendEmail(MailContent mail)
        {
            MailHelper helper=  new MailHelper();
            return Ok(helper.SendMail(mail));
        }
    }
}
