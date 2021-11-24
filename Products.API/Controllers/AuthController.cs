using System;
using System.Security.Claims;
using Products.API.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Products.Services.Interfaces;
using Products.API.Dto.Infrastructure;

namespace Products.API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;
        private readonly IUserService _userService;
        private readonly IJwtAuthManager _jwtAuthManager;

        public AuthController(ILogger<AuthController> logger, IUserService userService, IJwtAuthManager jwtAuthManager)
        {
            _logger = logger;
            _userService = userService;
            _jwtAuthManager = jwtAuthManager;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public ActionResult Login([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            if (!_userService.IsValidUserCredentials(request.Username, request.Password))
            {
                return Unauthorized();
            }

            var role = _userService.GetUserRole(request.Username);
            var claims = new[]
            {
                new Claim(ClaimTypes.Name,request.Username),
                new Claim(ClaimTypes.Role, role)
            };

            var jwtResult = _jwtAuthManager.GenerateJwtToken(request.Username, claims, DateTime.Now);

            _logger.LogInformation($"User [{request.Username}] logged in the system.");
            
            return Ok(new LoginResult
            {
                Username = request.Username,
                Role = role,
                AccessToken = jwtResult.AccessToken
            });
        }
    }
}
