
using Messaging.Application.DTOs;

namespace Messaging.Application.Interfaces
{
    public interface INotificationService
    {
        Task SendMessageAsync(List<int> userIds, NotificationDto notification);
    }
}
