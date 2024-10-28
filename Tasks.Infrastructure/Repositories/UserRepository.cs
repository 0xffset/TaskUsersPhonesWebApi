using Microsoft.EntityFrameworkCore;
using Tasks.Core.Entities.General;
using Tasks.Core.Entities.ViewModels;
using Tasks.Core.Interfaces.IRepositories;
using Tasks.Infrastructure.Data;

namespace Tasks.Infrastructure.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        async Task<ICollection<UserViewModel>> IUserRepository.GetAllUsers(CancellationToken cancellationToken)
        {
            ICollection<UserViewModel> response = new List<UserViewModel>();
            List<User> users = await _dbContext.Users.ToListAsync();

            for (int i = 0; i < users.Count; i++)
            {
                // Get phones
                List<PhoneViewModel> phones = await _dbContext.Phones
                     .Where(x => x.UserId == users[i].Id)
                     .Select(phone => new PhoneViewModel
                     {
                         Id = phone.Id,
                         CityCode = phone.CityCode,
                         CountryCode = phone.CountryCode,
                         Number = phone.Number,
                     })
                     .ToListAsync();
                response.Add(new UserViewModel()
                {
                    Id = users[i].Id,
                    FullName = users[i].FullName,
                    Email = users[i].Email,
                    UserName = users[i].UserName,
                    Phones = phones

                });

            }
            return response;
        }

        async Task<bool> IUserRepository.UserExistByEmail(string email)
        {
            return await _dbContext.Users.AnyAsync(x => x.Email == email);
        }

        async Task<bool> IUserRepository.UserExistById(int id)
        {
            return await _dbContext.Users.AnyAsync(x => x.Id == id);
        }

        async Task<bool> IUserRepository.UserUsernameExists(string username)
        {
            return await _dbContext.Users.AnyAsync(x => x.UserName == username);
        }
    }
}
