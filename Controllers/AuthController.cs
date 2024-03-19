 using be_artwork_sharing_platform.Core.Dtos.Auth;
using be_artwork_sharing_platform.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using System.Net;

namespace be_artwork_sharing_platform.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost]
        [Route("seed-roles")]
        public async Task<IActionResult> SeedRoles()
        {
            var seedRoles = await _authService.SeedRoleAsync();
            return StatusCode(seedRoles.StatusCode, seedRoles.Message);
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            var registerResult = await _authService.RegisterAsync(registerDto);
            return StatusCode(registerResult.StatusCode, registerResult.Message);
        }

        //Route -> Login
        [HttpPost]
        [Route("login")]
        public async Task<ActionResult<LoginServiceResponceDto>> Login([FromBody] LoginDto loginDto)
        {
            try
            {
                var login = await _authService.LoginAsync(loginDto);
                if (login is null)
                {
                    return NotFound("Username or Password incorrect");
                }
                else
                {
                    var checkIsActive = _authService.GetStatusUser(loginDto.UserName);
                    if (checkIsActive is true)
                    {
                        return Ok(login);
                    }
                    return BadRequest("Your account have been lock");
                }
            }
                
            catch
            {
                return BadRequest("Login Failed");
            }
        }

        [HttpPost]
        [Route("me")]
        public async Task<ActionResult<LoginServiceResponceDto>> Me([FromBody] MeDto meDto)
        {
            try
            {
                var me = await _authService.MeAsync(meDto);
                if (me is not null)
                {
                    return Ok(me);
                }
                else
                {
                    return Unauthorized("InvalidToken");
                }
            }
            catch (Exception)
            {
                return Unauthorized("InvalidToken");
            }
        }
    }
}
