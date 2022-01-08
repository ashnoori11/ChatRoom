using ChatRoom.Core.InterfaceServices;
using ChatRoom.Core.Services;
using ChatRoom.Data.Repository;
using ChatRoom.Domain.InterfaceRepositories;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatRoom.IoC.Dependencies
{
   public static class ChatRoomDependencyContainer
    {
        public static void ChatRoomServiceRegistery(this IServiceCollection services)
        {
            services.AddTransient(typeof(IBaseRepository<>), typeof(BaseRepository<>));
            services.AddTransient<IChatMessageService, ChatMessageService>();

            // signal R service 

            services.AddSingleton<IChatRoomService, ChatRoomService>();
        }
    }
}
