using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messaging.External.Proxies
{
    public static class HttpClientTokenExtension
    {
        public static void AddBearerToken(this HttpClient client, IHttpContextAccessor contextAccessor)
        {
            var context = contextAccessor.HttpContext;
            if (context.User.Identity.IsAuthenticated && context.Request.Cookies.ContainsKey("accessToken"))
            {
                var token = context.Request.Cookies["accessToken"];
                if (!string.IsNullOrEmpty(token))
                {
                    client.DefaultRequestHeaders.Add("Cookie", $"accessToken={token}");
                }
            }
        }
    }
}
