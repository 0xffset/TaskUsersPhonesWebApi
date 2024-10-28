using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taks.Core.Interfaces.IServices;
using Tasks.Core.Entities.ViewModels;

namespace Tasks.Core.Interfaces.IServices
{
    public interface IPhoneService : IBaseService<PhoneViewModel>
    {
        Task<PhoneViewModel> Create(PhoneCreateViewModel model, CancellationToken cancellationToken);
        
        Task Update(PhoneUpdateViewModel phoneViewModel, CancellationToken cancellationToken);

        Task Delete(int id, CancellationToken cancellationToken);

        Task<bool> ExistsPhoneById(int id, CancellationToken cancellationToken);

    }
}
