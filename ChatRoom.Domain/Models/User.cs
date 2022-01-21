using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatRoom.Domain.Models
{
   public class User
    {
        [Key]
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string UserType { get; set; }

        #region relation
        public virtual List<ChatMessage> ChatMessages { get; set; }
        public virtual List<User_Room> User_Rooms { get; set; }
        #endregion
    }
}
