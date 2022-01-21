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
    public class ChatHub : Hub
    {
        #region dependencies
        private readonly IChatMessageService _chatMessageService;
        private readonly IChatRoomService _chatRoomService;
        private readonly IRoomService _roomServiceDb;
        private readonly IHubContext<AgentHub> _agentHub;

        public ChatHub(IChatMessageService chatMessageService, IChatRoomService chatRoomService,
            IRoomService roomServiceDb, IHubContext<AgentHub> agentHub)
        {
            _chatMessageService = chatMessageService;
            _chatRoomService = chatRoomService;
            _roomServiceDb = roomServiceDb;
            _agentHub = agentHub;
        }
        #endregion

        public override async Task OnConnectedAsync()
        {
            //agents login
            if (Context.User.Identity.IsAuthenticated)
            {
                await base.OnConnectedAsync();
                return;
            }

            // build a chat room and take chat room id
            var roomId = await _chatRoomService.CreateRoom(Context.ConnectionId);

            //add user Client to chat room
            await Groups.AddToGroupAsync(Context.ConnectionId, roomId.ToString());

            // add created room to db
            Guid groupId = await _chatRoomService.GetRoomForConnectionId(Context.ConnectionId);
            await _roomServiceDb.InsertChatRoomAsync(new ChatRoomsViewModels
            {
                OwnerConnectionId = Context.ConnectionId,
                RoomId = roomId,
                GroupId = groupId
            });

            // default message show to all clients at the begining
            await Clients.Caller.SendAsync("ReciveMessage", "Ashkan Noori", DateTimeOffset.UtcNow, "Welcom to My ChatRoom Application");

            await base.OnConnectedAsync();
        }

        public async Task SendMessage(string name, string message)
        {
            // find user group id by connection id
            var roomId = await _chatRoomService.GetRoomForConnectionId(Context.ConnectionId);

            // save chat message to database
            ChatMessageViewModels viewModel = new ChatMessageViewModels()
            {
                SendAt=DateTimeOffset.UtcNow,
                MessageBody=message,
                RoomId=roomId,
                SenderName=name
            };

            //save to db
            await _chatMessageService.SaveMessageAsync(viewModel);

            // save to dictionary
            await _chatRoomService.AddMessage(roomId, viewModel);

            await Clients.Group(roomId.ToString()).SendAsync("ReciveMessage", viewModel.SenderName, viewModel.SendAt, viewModel.MessageBody);
            // await Clients.All.SendAsync("ReciveMessage",viewModel.SenderName,viewModel.SendAt,viewModel.MessageBody);
        }

        public async Task SetName(string visitorName) // visitorname = user name
        {
            //chose room name
            var roomName = $"Chat With {visitorName} In Ashkan Noori Chat App ";

            // find room by uniqe connection id
            var roomId = await _chatRoomService.GetRoomForConnectionId(Context.ConnectionId); // context Belongs to signalR hub

            // set chat room name
            await _chatRoomService.SetRoomName(roomId, roomName);
            await _agentHub.Clients.All.SendAsync("ActiveRooms",await _chatRoomService.GetAllRooms());
        }

        // join and exit of agents with authorization

        [Authorize]
        public async Task AgentJoinRoom(Guid roomid)
        {
            if (roomid == Guid.Empty) throw new ArgumentException("invalid room id");
            await Groups.AddToGroupAsync(Context.ConnectionId, roomid.ToString());
        }

        [Authorize]
        public async Task agentLeaveRoom(Guid roomId)
        {
            if (roomId == Guid.Empty) throw new ArgumentException("invalid room id");
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomId.ToString());
        }
    }
}
