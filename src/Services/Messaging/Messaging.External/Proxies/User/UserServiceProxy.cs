
using Messaging.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System.Text.Json;
using System.Text;
using Messaging.Domain.Models;
using Messaging.Application.DTOs;

namespace Messaging.External.Proxies.User
{
    public class UserServiceProxy : IUserServiceProxy
    {
        private readonly ApiUrls _apiUrls;
        private readonly HttpClient _httpClient;

        public UserServiceProxy(
            HttpClient httpClient,
            IOptions<ApiUrls> apiUrls,
            IHttpContextAccessor httpContextAccessor)
        {
            httpClient.AddBearerToken(httpContextAccessor);
            _httpClient = httpClient;
            _apiUrls = apiUrls.Value;
        }

        public async Task<UserDto?> GetUserByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync($"{_apiUrls.UserUrl}api/users/{id}");

            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();

                var baseResponse = JsonSerializer.Deserialize<BaseResponseModel>(
                    jsonResponse,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    }
                );

                if (baseResponse != null && baseResponse.Data.ValueKind != JsonValueKind.Null)
                {
                    var userData = JsonSerializer.Deserialize<UserDto>(
                        baseResponse.Data.ToString(),
                        new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        }
                    );

                    return userData;
                }
            }

            return null;
        }

        public async Task<List<UserDto>?> GetUsersAsync(GetChatsProxyRequestDto request)
        {
            var content = new StringContent(
                JsonSerializer.Serialize(request),
                Encoding.UTF8,
                "application/json"
            );

            var response = await _httpClient.PostAsync($"{_apiUrls.UserUrl}api/users", content);

            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();

                var baseResponse = JsonSerializer.Deserialize<BaseResponseModel>(
                    jsonResponse,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    }
                );

                if (baseResponse != null && baseResponse.Data.ValueKind != JsonValueKind.Null)
                {
                    var userData = JsonSerializer.Deserialize<List<UserDto>>(
                        baseResponse.Data.ToString(),
                        new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        }
                    );

                    return userData;
                }
            }

            return null;
        }
    }
}
