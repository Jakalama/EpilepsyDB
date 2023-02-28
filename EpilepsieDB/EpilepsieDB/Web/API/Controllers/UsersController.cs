using EpilepsieDB.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace EpilepsieDB.Web.API.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUsersService _userService;
        private readonly ITokenService _tokenService;

        public UsersController(
            IUsersService userService,
            ITokenService tokenService)
        {
            _userService = userService;
            _tokenService = tokenService;
        }

        // POST: api/users/login
        [HttpPost("login")]
        public async Task<ActionResult<AuthenticationResponse>> CreateBearerToken(AuthenticationRequest request)
        {
            var user = await _userService.FindByName(request.Username);
            
            var isPasswordValid = await _userService.CheckPassword(user, request.Password);

            if (!isPasswordValid)
                return BadRequest("Bad credentials");


            List<string> userRoles = await _userService.GetRoles(user.Id);

            var token = _tokenService.CreateToken(user, userRoles);

            return token;
        }

        public class AuthenticationRequest
        {
            [Required]
            public string Username { get; set; }
            [Required]
            public string Password { get; set; }
        }

    }
}
