using Infrastructure.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Focus.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "ApiFox")]
    public class ApifixController : ControllerBase
    {
        public ApifixController()
        {

        }
        [HttpPost, Route("Login"), AllowAnonymous]
        public IActionResult Login()
        {
            return Ok(new ResultModel("Success"));
        }
    }
}
