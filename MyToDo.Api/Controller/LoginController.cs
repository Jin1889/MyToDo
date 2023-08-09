using Microsoft.AspNetCore.Mvc;
using MyToDo.Api.Service;
using MyToDo.Shared.Dtos;

namespace MyToDo.Api.Controller
{
    /// <summary>
    /// 账户控制器
    /// </summary>
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class LoginController : ControllerBase
    {
        private readonly ILoginService service;

        public LoginController(ILoginService service)
        {
            this.service = service;
        }

        [HttpPost]
        public async Task<ApiResponse> Login([FromBody] UserDto user) => await service.LoginAsync(user.Account, user.PassWord);

        [HttpPost]
        public async Task<ApiResponse> Resgiter([FromBody] UserDto user) => await service.Resgiter(user);
    }
}
