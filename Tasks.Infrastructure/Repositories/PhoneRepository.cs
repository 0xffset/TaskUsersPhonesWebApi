using Microsoft.EntityFrameworkCore;
using Tasks.Core.Entities.General;
using Tasks.Core.Interfaces.IRepositories;
using Tasks.Infrastructure.Data;

namespace Tasks.Infrastructure.Repositories
{
    public class PhoneRepository : BaseRepository<Phone>, IPhoneRepository
    {
        public PhoneRepository(ApplicationDbContext dbContext) : base(dbContext)
        {

        }

        public async Task<bool> ExistsPhoneById(int id)
        {
            return await _dbContext.Phones.AnyAsync(x => x.Id == id);
        }
    }
}
