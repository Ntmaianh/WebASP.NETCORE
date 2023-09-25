using Application.System;
using eShopSolution.ViewModel.System.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
namespace eShopSolution.BackEndAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUser _user;

        public UserController(IUser user)
        {
            _user = user;
        }
        [HttpPost("authenticate")]
        [AllowAnonymous] // cho phép chưa đăng nhập cũng có thể gọi 
        public async Task<IActionResult> Authencate([FromForm] LoginRequest request)
        {
            if (!ModelState.IsValid) // check xem dữ liệu đầu vào nó có đúng như mình đã quy định cho nó ở Fulent chưa 
            {
                return BadRequest();
            }
            var result = await _user.Login(request);
            if (string.IsNullOrEmpty(result))
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromForm] RegisterRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _user.Register(request);
            if (!result)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

    }
}
