using ChatRoom.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatRoom.Core.InterfaceServices
{
    public interface IUserservices
    {
        Task<bool> AddUserAsync(UserViewModel viewModel);
        Task<string> LoginAsync(UserViewModel viewmodel);
    }
}
