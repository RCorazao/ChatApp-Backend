
using Microsoft.AspNetCore.Http;

namespace Auth.Application.Storage
{
    public interface IFileStorageService
    {
        Task<string> SaveFileAsync(IFormFile file);
    }
}
