using Auth.Application.DTOs;
using Auth.Application.Features;
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

                if (result is null)
                {
                    return StatusCode(StatusCodes.Status400BadRequest,
                        ResponseApiService.Response(StatusCodes.Status400BadRequest, message: "Incorrect credentials"));
                }

                return StatusCode(StatusCodes.Status200OK,
                    ResponseApiService.Response(StatusCodes.Status200OK, result));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                        ResponseApiService.Response(StatusCodes.Status500InternalServerError, message: ex.Message));
            }
        }

        [HttpPost("signup")]
        public async Task<ActionResult<AuthenticationResponse>> SignUpAsync([FromForm] SignUpRequest request)
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
    }
}
