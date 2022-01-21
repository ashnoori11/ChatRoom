using ChatRoom.Core.InterfaceServices;
using ChatRoom.Core.ViewModels;
using ChatRoom.Domain.InterfaceRepositories;
using ChatRoom.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatRoom.Core.Services
{
    public class RoomService : IRoomService
    {
        #region injection
        private readonly IBaseRepository<Room> _baseRepository;
        private readonly IBaseRepository<User_Room> _User_Room_Repository;
        private readonly IBaseRepository<User> _userRepository;
        public RoomService(IBaseRepository<Room> baseRepository,
            IBaseRepository<User_Room> User_Room_Repository,
            IBaseRepository<User> userRepository)
        {
            _baseRepository = baseRepository;
            _User_Room_Repository = User_Room_Repository;
            _userRepository = userRepository;
        }
        #endregion

        public async Task<bool> InsertChatRoomAsync(ChatRoomsViewModels viewModel)
        {
            try
            {
                Room room = new Room
                {
                    RoomId=viewModel.RoomId,
                    GroupId=viewModel.GroupId,
                    OwnerConnectionId=viewModel.OwnerConnectionId,
                    RoomName=viewModel.RoomName??"DefaultChatRoom"
                };

                if (viewModel.UserName != null)
                {
                    int userid =await FindUserIdByUserNameAsync(viewModel.UserName);
                    User_Room user_Room = new User_Room
                    {
                        UserId=userid,
                        RoomId=viewModel.RoomId
                    };

                    await _User_Room_Repository.InsertAsync(user_Room);
                }

                await _baseRepository.InsertAsync(room);
                await _baseRepository.SaveAsync();

                return true;
            }
            catch
            {
                return false;
            }
        }

        private async Task<int> FindUserIdByUserNameAsync(string username)
        {
            var users = await _userRepository.GetAllAsListAsync();
            int userId = users.Where(a => a.UserName == username).Select(a => a.UserId).FirstOrDefault();
            return userId;
        }

    }
}
