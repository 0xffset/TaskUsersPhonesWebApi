using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taks.Core.Interfaces.IRepositories;
using Tasks.Core.Entities.General;
using Tasks.Core.Entities.ViewModels;

namespace Tasks.Core.Interfaces.IRepositories
{
    public interface IPhoneRepository : IBaseRepository<Phone>
    {
        Task<bool> ExistsPhoneById(int id);
    }
}
