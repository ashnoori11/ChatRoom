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

        public async Task<Guid> SaveMessageAsync(ChatMessageViewModels viewModel)
        {
            try
            {
                ChatMessage chatMessage = new ChatMessage
                {
                    MessageId=Guid.NewGuid(),
                    SendAt=viewModel.SendAt,
                    MessageBody=viewModel.MessageBody,
                    RoomId=viewModel.RoomId,
                    SenderName=viewModel.SenderName,
                    UserId=viewModel.UserId,
                };

                await _baseRepository.InsertAsync(chatMessage);
                await _baseRepository.SaveAsync();
                return (Guid)chatMessage.MessageId;
            }
            catch
            {
                return Guid.Empty;
            }
        }

        public async Task InsertChatGroupInfoAsync(string connectionId,Guid roomId)
        {
            Room model = new Room
            {
                RoomId=roomId,
                OwnerConnectionId=connectionId,
                GroupId=roomId,
            };

            await _roomRepository.InsertAsync(model);
            await _roomRepository.SaveAsync();
        }

        public async Task<IEnumerable<ChatMessageViewModels>> GetAllChatHistories()
        {
            List<ChatMessageViewModels> messages = new List<ChatMessageViewModels>();
            var chats = await _baseRepository.GetAllAsListAsync();
            foreach (var item in chats)
            {
                ChatMessageViewModels Vm = new ChatMessageViewModels
                {
                    SendAt=item.SendAt,
                    MessageBody=item.MessageBody,
                    MessageId=(Guid)item.MessageId,
                    SenderName=item.SenderName,
                    RoomId=(Guid)item.RoomId,
                    UserId=(int)item.UserId
                };

                messages.Add(Vm);
            }

            var sortedMessages = messages.OrderBy(a => a.SendAt).AsEnumerable();

            return sortedMessages;
        }

        public async Task<IEnumerable<ChatMessageViewModels>> GetAllChatHistoryByRoomId(Guid roomid)
        {
            var chats = await _baseRepository.GetAllAsListAsync();
            var chathistory = chats.Where(a => a.RoomId == roomid).Select(item => new ChatMessageViewModels
            {
                SendAt = item.SendAt,
                MessageBody = item.MessageBody,
                MessageId = (Guid)item.MessageId,
                SenderName = item.SenderName,
                RoomId=(Guid)item.RoomId,
                UserId=(int)item.UserId

            }).ToList();

            var sortedMessages = chathistory.OrderBy(a => a.SendAt).AsEnumerable();
            return sortedMessages;
        }
    }
}
