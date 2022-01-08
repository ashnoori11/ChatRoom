using ChatRoom.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatRoom.Data.Context
{
   public class ChatRoomContext : DbContext
    {
        public ChatRoomContext(DbContextOptions<ChatRoomContext> options)
            :base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {

        }

        public DbSet<ChatMessage> ChatMessages { get; set; }
        public DbSet<Room> Rooms { get; set; }
    }
}
