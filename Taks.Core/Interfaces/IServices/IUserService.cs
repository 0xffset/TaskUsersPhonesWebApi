using Taks.Core.Interfaces.IServices;
using Tasks.Core.Entities.ViewModels;

namespace Tasks.Core.Interfaces.IServices
{
    public interface IUserService : IBaseService<UserViewModel>
    {
        Task<UserViewModel> Create(UserCreateViewModel model, CancellationToken cancellationToken);

        Task<UserViewModel> Update(UserUpdateViewModel model, CancellationToken cancellationToken);
        Task Delete(int id, CancellationToken cancellationToken);

        Task<ICollection<UserViewModel>> GetAllUsers(CancellationToken cancellationToken);

        Task<bool> UserExistByEmail(string email);

        Task<bool> UserExistsById(int id);

        Task<bool> UserUsernameExists(string username);

    }
}
