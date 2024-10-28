using Microsoft.AspNetCore.Identity;
using Taks.Core.Entities.ViewModels;
using Taks.Core.Interfaces.IRepositories;
using Tasks.Core.Entities.General;
using Tasks.Core.Entities.ViewModels;
using Task = System.Threading.Tasks.Task;

namespace Tasks.Infrastructure.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AuthRepository(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<ResponseViewModel<UserViewModel>> Login(string userName, string password)
        {
            User? user = await _userManager.FindByNameAsync(userName);
            if (user == null || !user.IsActive)
            {
                return new ResponseViewModel<UserViewModel>
                {
                    Success = false,
                };
            }

            SignInResult result = await _signInManager.PasswordSignInAsync(user, password, isPersistent: false, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                // Register the last login
                User? currentUser = await _userManager.FindByNameAsync(userName);
                currentUser.LastLogin = DateTime.Now;
                _ = await _userManager.UpdateAsync(currentUser);
            }
            return result.Succeeded
                ? new ResponseViewModel<UserViewModel>
                {
                    Success = true,
                    Data = new UserViewModel { Id = user.Id, UserName = user.UserName, Email = user.Email },
                }
                : new ResponseViewModel<UserViewModel>
                {
                    Success = false
                };
        }

        public async Task Logout()
        {
            await _signInManager.SignOutAsync();

        }
    }
}
