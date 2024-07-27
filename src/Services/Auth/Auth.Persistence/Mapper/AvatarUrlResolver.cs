using Auth.Application.DTOs;
using Auth.Persistence.Identity;
using AutoMapper;
using Microsoft.AspNetCore.Http;


namespace Auth.Persistence.Mapper
{
    public class AvatarUrlResolver : IValueResolver<ApplicationUser, ApplicationUserDto, string>
    {
        public HttpContext _httpContext => new HttpContextAccessor().HttpContext;

        public AvatarUrlResolver()
        {
        }

        public string Resolve(ApplicationUser source, ApplicationUserDto destination, string destMember, ResolutionContext context)
        {
            if (source.Avatar is null) return null;

            var baseUrl = $"{_httpContext.Request.Scheme}://{_httpContext.Request.Host}{_httpContext.Request.PathBase}";
            return $"{baseUrl}{source.Avatar}";
        }
    }
}
