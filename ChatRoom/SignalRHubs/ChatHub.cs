using ChatRoom.Core.InterfaceServices;
using ChatRoom.Core.ViewModels;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatRoom.SignalRHubs
{
    public class ChatHub : Hub
    {
        #region dependencies
        private readonly IChatMessageService _chatMessageService;
        private readonly IChatRoomService _chatRoomService;

        public ChatHub(IChatMessageService chatMessageService, IChatRoomService chatRoomService)
        {
            _chatMessageService = chatMessageService;
            _chatRoomService = chatRoomService;
        }
        #endregion

        public override async Task OnConnectedAsync()
        {
            var roomId = await _chatRoomService.CreateRoom(Context.ConnectionId);
            await Groups.AddToGroupAsync(Context.ConnectionId,roomId.ToString());

            await Clients.Caller.SendAsync("ReciveMessage","Ashkan Noori",DateTimeOffset.UtcNow,"Welcom to My ChatRoom Application");

            await base.OnConnectedAsync();
        }

        public async Task SendMessage(string name,string message)
        {
            var roomId = await _chatRoomService.GetRoomForConnectionId(Context.ConnectionId);

            await _chatMessageService.InsertChatGroupInfoAsync(Context.ConnectionId, roomId);

            var viewModel = new ChatMessageViewModels
            {
                SenderName=name,
                MessageBody=message,
                SendAt=DateTimeOffset.Now
            };

            await _chatMessageService.SaveMessageAsync(viewModel);

            await Clients.Group(roomId.ToString()).SendAsync("ReciveMessage", viewModel.SenderName, viewModel.SendAt, viewModel.MessageBody);
         // await Clients.All.SendAsync("ReciveMessage",viewModel.SenderName,viewModel.SendAt,viewModel.MessageBody);
        }
    }
}
