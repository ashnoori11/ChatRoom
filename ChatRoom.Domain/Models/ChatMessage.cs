using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatRoom.Domain.Models
{
    public class ChatMessage
    {
        [Key]
        public int MessageId { get; set; }
        public string SenderName { get; set; }
        public string MessageBody { get; set; }
        public DateTimeOffset SendAt { get; set; }
    }
}
