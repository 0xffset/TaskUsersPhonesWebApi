using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Taks.Core.Interfaces.IMapper;
using Taks.Core.Interfaces.IServices;
using Taks.Core.Services;
using Tasks.Core.Entities.General;
using Tasks.Core.Entities.ViewModels;
using Tasks.Core.Interfaces.IRepositories;
using Tasks.Core.Interfaces.IServices;

namespace Tasks.Core.Services
{
    public class UserService : BaseService<User, UserViewModel>, IUserService
    {
        private readonly IBaseMapper<User, UserViewModel> _userViewModelMapper;
        private readonly IBaseMapper<UserCreateViewModel, User> _userCreateMapper;
        private readonly IBaseMapper<UserUpdateViewModel, User> _userUpdateMapper;
        private readonly IUserRepository _userRepository;
        private readonly IUserContext _userContext;
        private readonly IServiceProvider _serviceProvider;
        private readonly ILoggerFactory _loggerFactory;
        public UserService(IBaseMapper<User, UserViewModel> userViewModelMapper,
                           IBaseMapper<UserCreateViewModel, User> userCreateMapper,
                           IBaseMapper<UserUpdateViewModel, User> userUpdateMapper,
                           IUserRepository userRepository,
                           IUserContext userContext,
                           IServiceProvider serviceProvider,
                           ILoggerFactory loggerFactory) : base(userViewModelMapper, userRepository)
        {
            _userViewModelMapper = userViewModelMapper;
            _userCreateMapper = userCreateMapper;
            _userUpdateMapper = userUpdateMapper;
            _userRepository = userRepository;
            _userContext = userContext;
            _serviceProvider = serviceProvider;
            _loggerFactory = loggerFactory;
        }

        public async Task<UserViewModel> Create(UserCreateViewModel model, CancellationToken cancellationToken)
        {
            UserManager<User> UserManager = _serviceProvider.GetRequiredService<UserManager<User>>();

            // Mapping through AutoMapper
            User entity = _userCreateMapper.MapModel(model);
            entity.Created = DateTime.Now;
            entity.Updated = DateTime.Now;
            entity.EntryBy = Convert.ToInt32(_userContext.UserId);
            entity.IsActive = true;
            _ = await UserManager.CreateAsync(entity, model.Password);


            return _userViewModelMapper.MapModel(entity);

        }

        private async Task<IEnumerable<UserViewModel>> GetAllUsers(CancellationToken cancellationToken)
        {
            return await _userRepository.GetAllUsers(cancellationToken);
        }
        async Task<bool> IUserService.UserExistByEmail(string email)
        {
            return await _userRepository.UserExistByEmail(email);
        }

        async Task<bool> IUserService.UserUsernameExists(string username)
        {
            return await _userRepository.UserUsernameExists(username);
        }
        async Task<bool> IUserService.UserExistsById(int id)
        {
            return await _userRepository.UserExistById(id);
        }

        public System.Threading.Tasks.Task Delete(int id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<UserViewModel> Update(UserUpdateViewModel model, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }


        async System.Threading.Tasks.Task IUserService.Delete(int id, CancellationToken cancellationToken)
        {
            UserManager<User> UserManager = _serviceProvider.GetRequiredService<UserManager<User>>();
            User? user = await UserManager.FindByIdAsync(id.ToString());
            _ = await UserManager.DeleteAsync(user);

        }

        async Task<UserViewModel> IUserService.Update(UserUpdateViewModel model, CancellationToken cancellationToken)
        {
            UserManager<User> UserManager = _serviceProvider.GetRequiredService<UserManager<User>>();
            User? user = await UserManager.FindByIdAsync(model.Id.ToString());
            user.Updated = DateTime.Now;
            user.SecurityStamp = Guid.NewGuid().ToString();
            user.Email = model.Email;
            user.FullName = model.FullName;
            user.UserName = model.UserName;
            _ = await UserManager.UpdateAsync(user);


            return _userViewModelMapper.MapModel(user);
        }

        async Task<ICollection<UserViewModel>> IUserService.GetAllUsers(CancellationToken cancellationToken)
        {
            return await _userRepository.GetAllUsers(cancellationToken);
        }
    }
}
