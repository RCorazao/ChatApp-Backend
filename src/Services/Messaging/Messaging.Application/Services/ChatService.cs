﻿
using AutoMapper;
using Messaging.Application.DTOs;
using Messaging.Application.Interfaces;
using Messaging.Domain.Entities;
using Messaging.Domain.Repositories;

namespace Messaging.Application.Services
{
    public class ChatService : IChatService
    {
        private readonly IChatRepository _chatRepository;
        private readonly IUserServiceProxy _userServiceProxy;
        private readonly IMapper _mapper;

        public ChatService(
            IChatRepository chatRepository,
            IUserServiceProxy userServiceProxy,
            IMapper mapper
            )
        {
            _chatRepository = chatRepository;
            _userServiceProxy = userServiceProxy;
            _mapper = mapper;
        }

        public async Task<ChatDto?> CreatePrivate(UserDto user, int contactId)
        {
            var chat = await _chatRepository.GetContactChatAsync(user.Id, contactId);

            if (chat is null)
            {
                var contact = await _userServiceProxy.GetUserByIdAsync(contactId);

                if (contact is null) return null;

                var users = new List<ChatUser>
                {
                    new ChatUser(user.Id),
                    new ChatUser(contactId)
                };

                chat = new Chat { Users = users };
                
                await _chatRepository.CreateAsync(chat);
            }

            var chatDto = _mapper.Map<ChatDto>(chat);

            await UserMapping([chatDto], user);

            return chatDto;
        }

        public async Task<List<ChatDto>> GetChats(UserDto user)
        {
            var chats = await _chatRepository.GetUserChatsAsync(user.Id);

            var chatDtos = _mapper.Map<List<ChatDto>>(chats);

            await UserMapping(chatDtos, user);

            return chatDtos
                    .OrderByDescending(p => p.Messages.Any() ? p.Messages[0].CreatedAt : DateTime.MinValue)
                    .ToList();
        }

        public async Task<ChatDto> GetPaginated(UserDto user, string chatId, int pageNumber, int pageSize)
        {
            var chat = await _chatRepository.GetContactChatPaginatedAsync(user.Id, chatId, pageNumber, pageSize);

            return _mapper.Map<ChatDto>(chat);
        }

        public async Task<ChatDto> GetWithSkip(UserDto user, string chatId, int skip, int pageSize)
        {
            var chat = await _chatRepository.GetMessagesChatAsync(user.Id, chatId, skip, pageSize);

            return _mapper.Map<ChatDto>(chat);
        }

        public async Task<List<SearchChatsResponseDto>> SearchChats(UserDto user, string filter, int pageNumber, int pageSize)
        {
            GetChatsProxyRequestDto request = new GetChatsProxyRequestDto
            {
                Filter = filter,
                PageNumber = pageNumber,
                PageSize = pageSize
            };

            var usersProxy = await _userServiceProxy.GetUsersAsync(request);

            var chats = await _chatRepository.GetUserChatsAsync(user.Id);
            var contactIds = chats.SelectMany(p => p.Users)
                                .Where(p => p.Id != user.Id)
                                .Select(p => p.Id)
                                .Distinct()
                                .ToList();

            var result = new List<SearchChatsResponseDto>();

            if (usersProxy is not null)
            {
                foreach (var userProxy in usersProxy)
                {
                    if (userProxy.Id == user.Id) continue;
                    result.Add(new SearchChatsResponseDto
                    {
                        Id = userProxy.Id,
                        FullName = userProxy.FullName,
                        Email = userProxy.Email,
                        Avatar = userProxy.Avatar,
                        IsContact = contactIds.Contains(userProxy.Id)
                    });
                }
            }

            return result;
        }


        // ===================================================================================

        private async Task UserMapping(List<ChatDto> chatDtos, UserDto currentUser)
        {
            var userIds = chatDtos.SelectMany(p => p.Users)
                               .Where(p => p.Id != currentUser.Id)
                               .Select(p => p.Id)
                               .Distinct()
                               .ToList();

            GetChatsProxyRequestDto request = new GetChatsProxyRequestDto
            {
                UserIds = userIds,
                PageNumber = 1,
                PageSize = userIds.Count()
            };

            var userDtos = await _userServiceProxy.GetUsersAsync(request);

            foreach (var chatDto in chatDtos)
            {
                var currentUserDto = chatDto.Users.FirstOrDefault(p => p.Id == currentUser.Id);
                AssignData(currentUserDto, currentUser);

                var userContact = chatDto.Users.FirstOrDefault(p => p.Id != currentUser.Id);
                if (userContact != null)
                {
                    var userDto = userDtos.FirstOrDefault(p => p.Id == userContact.Id);
                    if (userDto != null)
                    {
                        AssignData(userContact, userDto);
                    }
                }
            }
        }

        private void AssignData(UserDto output, UserDto input)
        {
            output.FullName = input.FullName;
            output.Email = input.Email;
            output.Avatar = input.Avatar;
        }
    }
}
