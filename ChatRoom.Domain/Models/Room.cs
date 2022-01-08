using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatRoom.Domain.Models
{
    public class Room
    {
        [Key]
        public int RoomId { get; set; }

        public string OwnerConnectionId { get; set; }
        public string GroupId { get; set; }
    }
}
