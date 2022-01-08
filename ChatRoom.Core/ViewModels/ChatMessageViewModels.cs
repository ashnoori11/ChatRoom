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
        public int MessageId { get; set; }

        [Required]
        public string SenderName { get; set; }

        [Required]
        public string MessageBody { get; set; }

        [Required]
        public DateTimeOffset SendAt { get; set; }
    }
}
