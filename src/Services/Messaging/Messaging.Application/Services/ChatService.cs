
using AutoMapper;
using Messaging.Application.DTOs;
using Messaging.Application.Interfaces;
using Messaging.Domain.Entities;
using Messaging.Domain.Repositories;
using System.Linq.Expressions;

namespace Messaging.Application.Services
{
    public class ChatService : IChatService
    {
        private readonly IChatRepository _chatRepository;
        private readonly IMapper _mapper;

        public ChatService(
            IChatRepository chatRepository,
            IMapper mapper
            )
        {
            _chatRepository = chatRepository;
            _mapper = mapper;
        }

        public async Task<ChatDto> CreatePrivate(UserDto user, int contactId)
        {
            var chat = await _chatRepository.GetContactChatAsync(user.Id, contactId);

            if (chat is null)
            {
                var users = new List<ChatUser>
                {
                    new ChatUser(user.Id),
                    new ChatUser(contactId)
                };

                chat = new Chat { Users = users };
                
                await _chatRepository.CreateAsync(chat);
            }

            return _mapper.Map<ChatDto>(chat);
        }

        public async Task<List<ChatDto>> GetChats(UserDto user)
        {
            var chats = await _chatRepository.GetUserChatsAsync(user.Id);

            return _mapper.Map<List<ChatDto>>(chats);
        }

        public async Task<ChatDto> GetPaginated(UserDto user, string chatId, int pageNumber, int pageSize)
        {
            var chat = await _chatRepository.GetContactChatPaginatedAsync(user.Id, chatId, pageNumber, pageSize);

            return _mapper.Map<ChatDto>(chat);
        }
    }
}
