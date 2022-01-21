using ChatRoom.Core.InterfaceServices;
using ChatRoom.Core.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatRoom.SignalRHubs
{
    [Authorize]
    public class AgentHub : Hub
    {
        #region dependencies
        private readonly IChatMessageService _chatMessageService;
        private readonly IChatRoomService _chatRoomService;
        private readonly IRoomService _roomServiceDb;

        //inject chat hub into agent hub by IHubContext<T>
        private readonly IHubContext<ChatHub> _chatHub;

        public AgentHub(IChatMessageService chatMessageService, IChatRoomService chatRoomService,
            IHubContext<ChatHub> chatHub, IRoomService roomServiceDb)
        {
            _chatMessageService = chatMessageService;
            _chatRoomService = chatRoomService;
            _chatHub = chatHub;
            _roomServiceDb = roomServiceDb;
        }
        #endregion

        public override async Task OnConnectedAsync()
        {
            var fortest = _chatRoomService.GetAllRooms().Result.Values;
            await Clients.Caller.SendAsync("ActiveRooms", await _chatRoomService.GetAllRooms());
            await base.OnConnectedAsync();
        }

        public async Task AgentSendMessageToUser(Guid roomid,string text)
        {
            ChatMessageViewModels viewModel = new ChatMessageViewModels
            {
                SendAt=DateTime.Now,
                MessageBody=text,
                RoomId=roomid,
                SenderName=Context.User.Identity.Name,
            };

            await _chatRoomService.AddMessage(roomid, viewModel);
            await _chatMessageService.SaveMessageAsync(viewModel);

            await _chatHub.Clients.Group(roomid.ToString())
                .SendAsync("ReciveMessage", viewModel.SenderName, viewModel.SendAt, viewModel.MessageBody);
        }

        public async Task LoadHistory(Guid roomid)
        {
            // var history = await _chatMessageService.GetAllChatHistoryByRoomId(roomid);
            var history = await _chatRoomService.GetMessageHistory(roomid);
            await Clients.Caller.SendAsync("ReciveMessages", history);
        }
    }
}
