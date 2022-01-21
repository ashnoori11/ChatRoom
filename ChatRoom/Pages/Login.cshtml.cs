using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ChatRoom.Core.InterfaceServices;
using ChatRoom.Core.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ChatRoom.Pages
{
    public class LoginModel : PageModel
    {

        #region injection
        private readonly IUserservices _userservices;
        public LoginModel(IUserservices userservices)
        {
            _userservices = userservices;
        }
        #endregion


        #region UserModel

        [BindProperty]
        public UserAdminViewModel ViewModel { get; set; }

        public string ReturnUrl { get; set; }

        public class UserAdminViewModel
        {
            [Required]
            public string UserName { get; set; }

            [Required]
            [DataType(DataType.Password)]
            [MinLength(8, ErrorMessage = "cant be less than 8 characters !")]
            public string Passwrod { get; set; }
        }
        #endregion

        public void OnGet(string returnUrl = "")
        {
            if (!string.IsNullOrWhiteSpace(returnUrl)) ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPost([FromQuery] string returnUrl = null)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            //string username = Request.Form["UserName"];
            //string password = Request.Form["Password"];

            //if (password != "ashkanChat") return Page();

            UserViewModel lastViewModel = new UserViewModel
            {
                UserName = ViewModel.UserName,
                Password = ViewModel.Passwrod
            };

            string userType = await _userservices.LoginAsync(lastViewModel);
            if (!string.IsNullOrEmpty(userType))
            {
                var claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.Name,ViewModel.UserName),
                    new Claim(ClaimTypes.Role,userType)
                };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var properties = new AuthenticationProperties
                {
                    RedirectUri = "https://localhost:44316/SupportAgent"
                };

                return SignIn(new ClaimsPrincipal(identity), properties, CookieAuthenticationDefaults.AuthenticationScheme);
            }

            return Page();

        }
    }
}
