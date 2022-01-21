using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatRoom.Core.ViewModels
{
   public class ChatMessageViewModels
    {
        public Guid MessageId { get; set; }

        [Required]
        public string SenderName { get; set; }

        [Required]
        public string MessageBody { get; set; }

        [Required]
        public DateTimeOffset SendAt { get; set; }

        public Guid RoomId { get; set; }
        public int UserId { get; set; }
    }
}
