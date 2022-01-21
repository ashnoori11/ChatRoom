using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatRoom.Domain.Models
{
    public class ChatMessage
    {
        [Key]
        public Guid? MessageId { get; set; }
        public string SenderName { get; set; }
        public string MessageBody { get; set; }
        public DateTimeOffset SendAt { get; set; }

        #region foreignKey
        public Guid? RoomId { get; set; }
        public int? UserId { get; set; }
        #endregion

        #region relations

        [ForeignKey(nameof(RoomId))]
        public virtual Room Room { get; set; }

        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; }
        #endregion
    }
}
