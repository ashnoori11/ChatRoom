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
            builder.Entity<User_Room>().HasKey(a => new { a.RoomId, a.UserId });

            builder.Entity<User_Room>().HasOne(a => a.Room)
                .WithMany(a => a.User_Rooms)
                .HasForeignKey(a => a.RoomId);

            builder.Entity<User_Room>().HasOne(a => a.User)
                .WithMany(a => a.User_Rooms)
                .HasForeignKey(a => a.UserId);
        }

        public DbSet<ChatMessage> ChatMessages { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<User_Room> User_Rooms { get; set; }
    }
}
