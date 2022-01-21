using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatRoom.Core.ViewModels
{
    public class ChatRoomsViewModels
    {
        public Guid RoomId { get; set; }

        public string OwnerConnectionId { get; set; }
        public Guid GroupId { get; set; }
        public string RoomName { get; set; }
        public string UserType { get; set; }
        public string UserName { get; set; }
    }
}
