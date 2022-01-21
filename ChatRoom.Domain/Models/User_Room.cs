using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatRoom.Domain.Models
{
   public class User_Room
    {
        public int UserId { get; set; }
        public Guid? RoomId { get; set; }

        #region relations
        public virtual User User { get; set; }
        public virtual Room Room { get; set; }
        #endregion
    }
}
