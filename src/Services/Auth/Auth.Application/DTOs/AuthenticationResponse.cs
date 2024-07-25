using System;


namespace Auth.Application.DTOs
{
    public class AuthenticationResponse
    {
        public bool Succeeded { get; set; }
        public dynamic? Data { get; set; }
        public Dictionary<string, string>? Errors { get; set; }
    }
}
