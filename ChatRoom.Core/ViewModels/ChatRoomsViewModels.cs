using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatRoom.Core.ViewModels
{
    public class ChatRoomsViewModels
    {
        public int RoomId { get; set; }

        public string OwnerConnectionId { get; set; }
        public string GroupId { get; set; }
    }
}
