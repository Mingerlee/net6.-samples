using Infrastructure.Config;
using Infrastructure.Models;
using Infrastructure.Utilities;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.JSInterop;
using System.Net;

namespace Focus.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadFileController : ControllerBase
    {
        /// <summary>
        /// 上传文件
        /// </summary>
        /// <returns></returns>
        [HttpPost("Upload")]
        [EnableCors("AllowSpecificOrigin")]//配置Cors,允许跨域
        public async Task<ResultUploadFile> UploadFile(IFormFile formFile)
        {
            return await FileHelper.SaveToFile(formFile,"测试文件上传");
        }

    }
}
