using Auth.Application.DTOs;
using Auth.Application.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Auth.Api.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;
        private readonly IAuthService _authService;

        public AuthController(
            ILogger<AuthController> logger,
            IAuthService authService
            )
        {
            _logger = logger;
            _authService = authService;
        }

        [HttpPost("signin")]
        public async Task<ActionResult<AuthenticationResponse>> SignInAsync(SignInRequest request)
        {
            try
            {
                var result = await _authService.SignInAsync(request);

                if (!result.Succeeded)
                {
                    return BadRequest("Access denied");
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("signup")]
        public async Task<ActionResult<AuthenticationResponse>> SignUpAsync(SignUpRequest request)
        {
            try
            {
                AuthenticationResponse response = await _authService.SignUpAsync(request);

                if (response == null || !response.Succeeded)
                {
                    return BadRequest(response?.Errors);
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("signout")]
        public async Task<ActionResult> SignOutAsync()
        {
            await _authService.SignOutAsync();

            return NoContent();
        }
    }
}
