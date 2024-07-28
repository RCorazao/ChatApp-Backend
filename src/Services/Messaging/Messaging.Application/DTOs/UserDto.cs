﻿
using Messaging.Domain.Enums;

namespace Messaging.Application.DTOs
{
    public class UserDto
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Avatar { get; set; }
        public UserRol Rol { get; set; }
    }
}
