
using Microsoft.AspNetCore.SignalR;
using System.IdentityModel.Tokens.Jwt;

namespace Messaging.External.SignalR
{
    public class CustomUserIdProvider : IUserIdProvider
    {
        public string? GetUserId(HubConnectionContext connection)
        {
            var jwtToken = connection.GetHttpContext()?.Request.Cookies["accessToken"];

            if (jwtToken != null)
            {
                var handler = new JwtSecurityTokenHandler();
                var token = handler.ReadToken(jwtToken) as JwtSecurityToken;

                if (token != null)
                {
                    var userId = token.Claims.FirstOrDefault(claim => claim.Type == "nameid")?.Value;
                    return userId;
                }
            }

            return null;
        }
    }
}
