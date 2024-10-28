using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using Taks.Core.Interfaces.IMapper;
using Taks.Core.Interfaces.IServices;
using Task.API.Controllers.V1;
using Tasks.Core.Entities.General;
using Tasks.Core.Entities.ViewModels;
using Tasks.Core.Interfaces.IRepositories;
using Tasks.Core.Interfaces.IServices;
using Tasks.Core.Services;

namespace Tasks.UnitTesting.API
{
    public class PhoneControllerTest
    {
        private Mock<IBaseMapper<Phone, PhoneViewModel>> _phoneViewModelMapperMock;
        private Mock<IBaseMapper<PhoneCreateViewModel, Phone>> _phoneCreateMapperMock;
        private Mock<IBaseMapper<PhoneUpdateViewModel, Phone>> _phoneUpdateMapperMock;
        private Mock<IPhoneRepository> _phoneRepositoryMock;
        private Mock<IUserContext> _userContextMock;
        private Mock<IPhoneService> _phoneServiceMock;
        private Mock<IMemoryCache> _memoryCacheServiceMock;
        private Mock<IUserService> _userServiceMock;
        private PhoneController _phoneController;
        private Mock<ILogger<PhoneController>> _loggerMock;


        [SetUp]
        public void Setup()
        {
            _phoneViewModelMapperMock = new Mock<IBaseMapper<Phone, PhoneViewModel>>();
            _phoneCreateMapperMock = new Mock<IBaseMapper<PhoneCreateViewModel, Phone>>();
            _phoneUpdateMapperMock = new Mock<IBaseMapper<PhoneUpdateViewModel, Phone>>();
            _phoneRepositoryMock = new Mock<IPhoneRepository>();
            _userContextMock = new Mock<IUserContext>();
            _phoneServiceMock = new Mock<IPhoneService>();
            _userServiceMock = new Mock<IUserService>();
            _memoryCacheServiceMock = new Mock<IMemoryCache>();
            _phoneController = new PhoneController(default, default, default, default);
            _loggerMock = new Mock<ILogger<PhoneController>>();

        }
        [Test]
        public async System.Threading.Tasks.Task Create_ValidPhone()
        {
            PhoneService phoneService = new(

                _phoneViewModelMapperMock.Object,
                _phoneCreateMapperMock.Object,
                _phoneUpdateMapperMock.Object,
                _phoneRepositoryMock.Object,
                _userContextMock.Object,
                default,
                default
            );

            PhoneCreateViewModel phoneCreateViewModel = new()
            {
                Number = "803-234-1305",
                CityCode = "2",
                CountryCode = "+803",
                UserId = 2
            };

            PhoneViewModel phoneViewModel = new()
            {
                Number = "803-234-1305",
                CityCode = "2",
                CountryCode = "+803",
            };

            Phone phone = new()
            {
                Number = "803-234-1305",
                CityCode = "2",
                CountryCode = "+803",
            };


            _ = _phoneCreateMapperMock.Setup(mapper => mapper.MapModel(phoneCreateViewModel))
                            .Returns(phone);

            _ = _phoneRepositoryMock.Setup(repo => repo.Create(phone, It.IsAny<CancellationToken>()))
                                  .ReturnsAsync(phone);

            _ = _phoneViewModelMapperMock.Setup(mapper => mapper.MapModel(phone))
                                       .Returns(phoneViewModel);

            // Act
            PhoneViewModel result = await phoneService.Create(phoneCreateViewModel, It.IsAny<CancellationToken>());

            // Assert
            ClassicAssert.NotNull(result);
            ClassicAssert.That(result.Number, Is.EqualTo(phoneCreateViewModel.Number));


        }

        [Test]
        public async System.Threading.Tasks.Task Create_InvalidPhone()
        {
            PhoneService phoneService = new(

                _phoneViewModelMapperMock.Object,
                _phoneCreateMapperMock.Object,
                _phoneUpdateMapperMock.Object,
                _phoneRepositoryMock.Object,
                _userContextMock.Object,
                default,
                default
            );

            PhoneCreateViewModel phoneCreateViewModel = new()
            {
                Number = "803-234-1305",
                CityCode = "2",
                CountryCode = "+803",
                UserId = 2
            };

            PhoneViewModel phoneViewModel = new()
            {
                Number = "803-234-1306",
                CityCode = "2",
                CountryCode = "+803",
            };

            Phone phone = new()
            {
                Number = "803-234-1307",
                CityCode = "2",
                CountryCode = "+803",
            };


            _ = _phoneCreateMapperMock.Setup(mapper => mapper.MapModel(phoneCreateViewModel))
                            .Returns(phone);

            _ = _phoneRepositoryMock.Setup(repo => repo.Create(phone, It.IsAny<CancellationToken>()))
                                  .ReturnsAsync(phone);

            _ = _phoneViewModelMapperMock.Setup(mapper => mapper.MapModel(phone))
                                       .Returns(phoneViewModel);

            // Act
            PhoneViewModel result = await phoneService.Create(phoneCreateViewModel, It.IsAny<CancellationToken>());

            // Assert
            ClassicAssert.NotNull(result);
            ClassicAssert.That(result.Number, Is.EqualTo(phoneCreateViewModel.Number));


        }



    }
}
