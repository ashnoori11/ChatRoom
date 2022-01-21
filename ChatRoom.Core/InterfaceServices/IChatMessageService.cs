using ChatRoom.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatRoom.Core.InterfaceServices
{
   public interface IChatMessageService
    {
        Task<Guid> SaveMessageAsync(ChatMessageViewModels viewModel);
        Task InsertChatGroupInfoAsync(string connectionId, Guid roomId);
        Task<IEnumerable<ChatMessageViewModels>> GetAllChatHistories();
        Task<IEnumerable<ChatMessageViewModels>> GetAllChatHistoryByRoomId(Guid roomid);
    }
}
