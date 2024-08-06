
using Messaging.Application.DTOs;
using Messaging.Application.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace Messaging.External.SignalR
{
    public class NotificationService : INotificationService
    {
        private readonly IHubContext<NotificationHub> _hubContext;

        public NotificationService(IHubContext<NotificationHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task SendMessageAsync(List<int> userIds, NotificationDto notification)
        {
            foreach (int userId in userIds)
            {
                notification.IsCurrentUser = userId == notification.Message.UserId;
                await _hubContext.Clients.User(userId.ToString()).SendAsync("NewMessage", notification);
            }
        }
    }
}
