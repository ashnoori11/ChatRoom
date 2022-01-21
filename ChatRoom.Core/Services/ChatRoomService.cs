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
    public class ChatRoomService : IChatRoomService
    {
        // keep room or group id in memory
        private readonly Dictionary<Guid, ChatRoomsViewModels> _roomInfo = new Dictionary<Guid, ChatRoomsViewModels>();
        private readonly Dictionary<Guid, List<ChatMessageViewModels>> _messageHistory = new Dictionary<Guid, List<ChatMessageViewModels>>();

        public Task<Guid> CreateRoom(string connectionId)
        {
            var id = Guid.NewGuid();
            _roomInfo[id] = new ChatRoomsViewModels()
            {
                OwnerConnectionId = connectionId
            };

            return Task.FromResult(id);
        }

        public async Task<Guid> GetRoomForConnectionId(string connectionId)
        {
            // find all groups that user is into - find all users groups
            var foundRoom = _roomInfo.FirstOrDefault(r => r.Value.OwnerConnectionId == connectionId);

            // if user dosent have any group throw a exception
            if (foundRoom.Key == Guid.Empty)
            {
                throw new ArgumentException("invalid connection id");
            }

            // key is group id
            return await Task.FromResult(foundRoom.Key);
        }

        public async Task SetRoomName(Guid roomId,string name)
        {
            if (!_roomInfo.ContainsKey(roomId))
                throw new ArgumentException("There is no such group with these specifications");

            _roomInfo[roomId].RoomName = name;
            await Task.CompletedTask;
        }

        public Task<IReadOnlyDictionary<Guid, ChatRoomsViewModels>> GetAllRooms()
        {
            return Task.FromResult(_roomInfo as IReadOnlyDictionary<Guid, ChatRoomsViewModels>);
        }

        public Task AddMessage(Guid roomid, ChatMessageViewModels message)
        {
            if (!_messageHistory.ContainsKey(roomid))
            {
                _messageHistory[roomid] = new List<ChatMessageViewModels>();
            }

            _messageHistory[roomid].Add(message);
            return Task.CompletedTask;
        }

        public Task<IEnumerable<ChatMessageViewModels>> GetMessageHistory(Guid roomid)
        {
            _messageHistory.TryGetValue(roomid, out var messages);

            messages = messages ?? new List<ChatMessageViewModels>();
            var sortMessages = messages.OrderBy(a => a.SendAt).AsEnumerable();

            return Task.FromResult(sortMessages);
        }
    }
}
