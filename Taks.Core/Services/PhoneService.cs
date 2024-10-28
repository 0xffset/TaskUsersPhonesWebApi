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
    public class PhoneService : BaseService<Phone, PhoneViewModel>, IPhoneService
    {
        private readonly IBaseMapper<Phone, PhoneViewModel> _phoneViewModelMapper;
        private readonly IBaseMapper<PhoneCreateViewModel, Phone> _phoneCreateViewModelMapper;
        private readonly IBaseMapper<PhoneUpdateViewModel, Phone> _phoneUpdateViewModelMapper;
        private readonly IPhoneRepository _phoneRepository;
        private readonly IUserContext _userContext;
        private readonly IServiceProvider _serviceProvider;
        private readonly ILoggerFactory _loggerFactory;

        public PhoneService(IBaseMapper<Phone, PhoneViewModel> phoneViewModelMapper,
                            IBaseMapper<PhoneCreateViewModel, Phone> phoneCreateViewModelMapper,
                            IBaseMapper<PhoneUpdateViewModel, Phone> phoneUpdateViewModelMapper,
                            IPhoneRepository phoneRepository,
                            IUserContext userContext,
                            IServiceProvider serviceProvider,
                            ILoggerFactory loggerFactory)
            : base(phoneViewModelMapper, phoneRepository)
        {
            _phoneViewModelMapper = phoneViewModelMapper;
            _phoneCreateViewModelMapper = phoneCreateViewModelMapper;
            _phoneUpdateViewModelMapper = phoneUpdateViewModelMapper;
            _phoneRepository = phoneRepository;
            _userContext = userContext;
            _serviceProvider = serviceProvider;
            _loggerFactory = loggerFactory;
        }

        public async Task<PhoneViewModel> Create(PhoneCreateViewModel model, CancellationToken cancellationToken)
        {
            Phone entry = _phoneCreateViewModelMapper.MapModel(model);
            entry.EntryBy = Convert.ToInt32(_userContext.UserId);
            entry.EntryDate = DateTime.Now;

            return _phoneViewModelMapper.MapModel(await _phoneRepository.Create(entry, cancellationToken));
        }



        async Task IPhoneService.Delete(int id, CancellationToken cancellationToken)
        {
            Phone entity = await _phoneRepository.GetById(id, cancellationToken);
            await _phoneRepository.Delete(entity, cancellationToken);
        }

        async Task<bool> IPhoneService.ExistsPhoneById(int id, System.Threading.CancellationToken cancellationToken)
        {
            return await _phoneRepository.ExistsPhoneById(id);
        }

        async Task IPhoneService.Update(PhoneUpdateViewModel phoneViewModel, CancellationToken cancellationToken)
        {
            Phone existingData = await _phoneRepository.GetById(phoneViewModel.Id, cancellationToken);
            // Mapping
            _ = _phoneUpdateViewModelMapper.MapModel(phoneViewModel, existingData);

            existingData.UpdatedDate = DateTime.Now;
            existingData.UpdatedBy = Convert.ToInt32(_userContext.UserId);

            await _phoneRepository.Update(existingData, cancellationToken);
        }
    }
}
