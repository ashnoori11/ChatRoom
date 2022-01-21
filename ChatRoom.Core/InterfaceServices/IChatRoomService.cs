using ChatRoom.Core.ViewModels;
using ChatRoom.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatRoom.Core.InterfaceServices
{
    public interface IChatRoomService
    {
        Task<Guid> CreateRoom(string connectionId);
        Task<Guid> GetRoomForConnectionId(string connectionId);
        Task SetRoomName(Guid roomId, string name);
        // Task<IReadOnlyDictionary<Guid, Room>> GetAllRooms();
        Task<IReadOnlyDictionary<Guid, ChatRoomsViewModels>> GetAllRooms();
        Task AddMessage(Guid roomid, ChatMessageViewModels message);
        Task<IEnumerable<ChatMessageViewModels>> GetMessageHistory(Guid roomid);
    }
}
