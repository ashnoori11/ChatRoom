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
        Task<bool> SaveMessageAsync(ChatMessageViewModels viewModel);
        Task InsertChatGroupInfoAsync(string connectionId, Guid roomId);
    }
}
