
using Auth.Application.DTOs;
using Auth.Application.Identity;
using Auth.Persistence.Helpers;
using Auth.Persistence.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;

namespace Auth.Persistence.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _configuration;

        public AuthService(
            UserManager<ApplicationUser> userManager, 
            SignInManager<ApplicationUser> signInManager,
            IConfiguration configuration
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }

        public async Task<AuthenticationResponse> SignUpAsync(SignUpRequest request)
        {
            ApplicationUser user = new ApplicationUser
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                UserName = request.Email
            };

            IdentityResult result = await _userManager.CreateAsync(user, request.Password);

            return result.ToAuthenticationResult();
        }

        public async Task<IdentityAccess> SignInAsync(SignInRequest request)
        {
            var result = new IdentityAccess();

            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user == null)
            {
                result.Succeeded = false;
                return result;
            }

            var response = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);

            if (response.Succeeded)
            {
                result.Succeeded = true;
                await GenerateToken(user, result);
            }

            return result;
        }

        public async Task SignOutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        private async Task GenerateToken(ApplicationUser user, IdentityAccess data)
        {
            var secretKey = _configuration.GetSection("SecretKey").Value;
            var key = Encoding.ASCII.GetBytes(secretKey!);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.FirstName),
                new Claim(ClaimTypes.Surname, user.LastName)
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature
                )
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var createdToken = tokenHandler.CreateToken(tokenDescriptor);

            data.AccessToken = tokenHandler.WriteToken(createdToken);
            data.ExpiresAt = tokenDescriptor.Expires;
        }
    }
}
