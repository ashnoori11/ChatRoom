using ChatRoom.Core.InterfaceServices;
using ChatRoom.Core.ViewModels;
using ChatRoom.Domain.InterfaceRepositories;
using ChatRoom.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatRoom.Core.Services
{
   public class ChatMessageService : IChatMessageService
    {
        private readonly IBaseRepository<ChatMessage> _baseRepository;
        private readonly IBaseRepository<Room> _roomRepository;

        public ChatMessageService(IBaseRepository<ChatMessage> baseRepository, IBaseRepository<Room> roomRepository)
        {
            _baseRepository = baseRepository;
            _roomRepository = roomRepository;
        }

        public async Task<bool> SaveMessageAsync(ChatMessageViewModels viewModel)
        {
            try
            {
                ChatMessage model = new ChatMessage
                {
                    SenderName = viewModel.SenderName,
                    MessageBody = viewModel.MessageBody,
                    SendAt = viewModel.SendAt
                };

                await _baseRepository.InsertAsync(model);
                await _baseRepository.SaveAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task InsertChatGroupInfoAsync(string connectionId,Guid roomId)
        {
            Room model = new Room
            {
                OwnerConnectionId=connectionId,
                GroupId=roomId.ToString()
            };

            await _roomRepository.InsertAsync(model);
            await _roomRepository.SaveAsync();
        }
    }
}
