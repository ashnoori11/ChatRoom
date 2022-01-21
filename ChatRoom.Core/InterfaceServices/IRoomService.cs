using ChatRoom.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatRoom.Core.InterfaceServices
{
    public interface IRoomService
    {
        Task<bool> InsertChatRoomAsync(ChatRoomsViewModels viewModel);
    }
}
