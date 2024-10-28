using Taks.Core.Interfaces.IRepositories;
using Tasks.Core.Entities.General;
using Tasks.Core.Entities.ViewModels;

namespace Tasks.Core.Interfaces.IRepositories
{
    public interface IUserRepository : IBaseRepository<User>
    {
        Task<bool> UserExistByEmail(string email);
        Task<bool> UserExistById(int id);
        Task<bool> UserUsernameExists(string username);

        Task<ICollection<UserViewModel>> GetAllUsers(CancellationToken cancellationToken);
    }
}
