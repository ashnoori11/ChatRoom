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
        private readonly Dictionary<Guid, ChatRoomsViewModels> _roomInfo = new Dictionary<Guid, ChatRoomsViewModels>();

        public async Task<Guid> CreateRoom(string connectionId)
        {
            var id = Guid.NewGuid();
            _roomInfo[id] = new ChatRoomsViewModels()
            {
                OwnerConnectionId = connectionId
            };

            return await Task.FromResult(id);
        }

        public async Task<Guid> GetRoomForConnectionId(string connectionId)
        {
            var foundRoom = _roomInfo.FirstOrDefault(r => r.Value.OwnerConnectionId == connectionId);
            if (foundRoom.Key == Guid.Empty)
            {
                throw new ArgumentException("invalid connection id");
            }

            return await Task.FromResult(foundRoom.Key);
        }
    }
}
