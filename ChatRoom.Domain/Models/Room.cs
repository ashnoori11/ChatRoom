﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatRoom.Domain.Models
{
    public class Room
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid? RoomId { get; set; }
        public string OwnerConnectionId { get; set; }
        public Guid GroupId { get; set; }
        public string RoomName { get; set; }

        #region Relations
        public virtual List<ChatMessage> ChatMessages { get; set; }
        public virtual List<User_Room> User_Rooms { get; set; }
        #endregion
    }
}
