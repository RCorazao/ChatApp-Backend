using Auth.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Application.Identity
{
    public interface IAuthService
    {
        Task<IdentityAccess> SignInAsync(SignInRequest signInRequest);
        Task SignOutAsync();
        Task<AuthenticationResponse> SignUpAsync(SignUpRequest signUpRequest);
    }
}
