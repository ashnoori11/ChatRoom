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
    public class Userservices : IUserservices
    {
        #region injection
        private readonly IBaseRepository<User> _baseRepository;
        public Userservices(IBaseRepository<User> baseRepository)
        {
            _baseRepository = baseRepository;
        }
        #endregion

        public async Task<bool> AddUserAsync(UserViewModel viewModel)
        {
            try
            {
                User user = new User
                {
                    UserName = viewModel.UserName,
                    UserType = viewModel.UserType,
                    Password = viewModel.Password
                };

               await _baseRepository.InsertAsync(user);
                await _baseRepository.SaveAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public async Task<string> LoginAsync(UserViewModel viewmodel)
        {
            try
            {
                var allUsers = await _baseRepository.GetAllAsListAsync();
                var user = allUsers.Where(a => a.UserName == viewmodel.UserName && a.Password == viewmodel.Password).FirstOrDefault();
                if (user == null)
                {
                    return string.Empty;
                }
                    return user.UserType;
            }
            catch
            {
                return string.Empty;
            }
        }
    }
}
