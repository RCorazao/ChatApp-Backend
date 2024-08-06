using AutoMapper;
using Messaging.Application.DTOs;
using Messaging.Application.Interfaces;
using Messaging.Domain.Entities;
using Messaging.Domain.Repositories;

namespace Messaging.Application.Services
{
    public class MessageService : IMessageService
    {
        private readonly IRepository<Message> _messageRepository;
        private readonly IChatRepository _chatRepository;
        private readonly INotificationService _notificationService;
        private readonly IMapper _mapper;

        public MessageService(
            IRepository<Message> messageRepository,
            IChatRepository chatRepository,
            INotificationService notificationService,
            IMapper mapper
            )
        {
            _messageRepository = messageRepository;
            _chatRepository = chatRepository;
            _notificationService = notificationService;
            _mapper = mapper;
        }

        public async Task<MessageDto?> Create(UserDto sender, string chatId, string content)
        {
            var chat = await _chatRepository.GetAsync(chatId);

            if (chat is null) return null;

            var message = new Message
            {
                UserId = sender.Id,
                ChatId = chatId,
                Content = content
            };

            var record = await _messageRepository.CreateAsync(message);

            await _chatRepository.AddMessageAsync(chatId, record);

            var notification = BuildNotification(sender, chat, message);
            await _notificationService.SendMessageAsync(chat.Users.Select(p => p.Id).ToList(), notification);

            return notification.Message;
        }

        private NotificationDto BuildNotification(UserDto sender, Chat chat, Message message)
        {
            var notification = new NotificationDto
            {
                Chat = _mapper.Map<ChatDto>(chat),
                Message = _mapper.Map<MessageDto>(message)
            };

            var senderDto = notification.Chat.Users.FirstOrDefault(p => p.Id == sender.Id);
            senderDto.Email = sender.Email;
            senderDto.FullName = sender.FullName;
            senderDto.Avatar = sender.Avatar;

            return notification;
        }
    }
}
