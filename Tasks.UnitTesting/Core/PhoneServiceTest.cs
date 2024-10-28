using Moq;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using Taks.Core.Interfaces.IMapper;
using Taks.Core.Interfaces.IServices;
using Tasks.Core.Entities.General;
using Tasks.Core.Entities.ViewModels;
using Tasks.Core.Interfaces.IRepositories;
using Tasks.Core.Services;

namespace Tasks.UnitTesting.Core
{
    public class PhoneServiceTest
    {
        private Mock<IBaseMapper<Phone, PhoneViewModel>> _phoneViewModelMapperMock;
        private Mock<IBaseMapper<PhoneCreateViewModel, Phone>> _phoneCreateMapperMock;
        private Mock<IBaseMapper<PhoneUpdateViewModel, Phone>> _phoneUpdateMapperMock;
        private Mock<IPhoneRepository> _phoneRepositoryMock;
        private Mock<IUserContext> _userContextMock;

        [SetUp]
        public void Setup()
        {
            _phoneViewModelMapperMock = new Mock<IBaseMapper<Phone, PhoneViewModel>>();
            _phoneCreateMapperMock = new Mock<IBaseMapper<PhoneCreateViewModel, Phone>>();
            _phoneUpdateMapperMock = new Mock<IBaseMapper<PhoneUpdateViewModel, Phone>>();
            _phoneRepositoryMock = new Mock<IPhoneRepository>();
            _userContextMock = new Mock<IUserContext>();
        }

        [Test]
        public async System.Threading.Tasks.Task Create_ValidPhone()
        {
            PhoneService taskService = new(
              _phoneViewModelMapperMock.Object,
              _phoneCreateMapperMock.Object,
              _phoneUpdateMapperMock.Object,
              _phoneRepositoryMock.Object,
              _userContextMock.Object, default, default);

            PhoneCreateViewModel newPhoneCreateViewModel = new()
            {
                Number = "803-234-1305",
                CityCode = "2",
                CountryCode = "+803",
                UserId = 2

            };

            PhoneViewModel newPhoneViewModel = new()
            {
                Number = "803-234-1305",
                CityCode = "2",
                CountryCode = "+803",

            };



            Phone createdPhone = new()
            {
                Number = "803-234-1305",
                CityCode = "2",
                CountryCode = "+803",
                UserId = 2

            };


            _ = _phoneCreateMapperMock.Setup(mapper => mapper.MapModel(newPhoneCreateViewModel))
                          .Returns(createdPhone);

            _ = _phoneRepositoryMock.Setup(repo => repo.Create(createdPhone, It.IsAny<CancellationToken>()))
                                  .ReturnsAsync(createdPhone);

            _ = _phoneViewModelMapperMock.Setup(mapper => mapper.MapModel(createdPhone))
                                       .Returns(newPhoneViewModel);

            // Act
            PhoneViewModel result = await taskService.Create(newPhoneCreateViewModel, It.IsAny<CancellationToken>());

            // Assert
            ClassicAssert.NotNull(result);
            ClassicAssert.That(result.Number, Is.EqualTo(newPhoneCreateViewModel.Number));
            // Additional assertions for other properties

        }

        [Test]
        public async System.Threading.Tasks.Task Create_InvalidPhone()
        {
            PhoneService taskService = new(
              _phoneViewModelMapperMock.Object,
              _phoneCreateMapperMock.Object,
              _phoneUpdateMapperMock.Object,
              _phoneRepositoryMock.Object,
              _userContextMock.Object, default, default);

            PhoneCreateViewModel newPhoneCreateViewModel = new()
            {
                Number = "803-234-1304",
                CityCode = "2",
                CountryCode = "+803",
                UserId = 2

            };

            PhoneViewModel newPhoneViewModel = new()
            {
                Number = "803-234-1305",
                CityCode = "2",
                CountryCode = "+803",

            };



            Phone createdPhone = new()
            {
                Number = "803-234-1306",
                CityCode = "2",
                CountryCode = "+803",
                UserId = 2

            };


            _ = _phoneCreateMapperMock.Setup(mapper => mapper.MapModel(newPhoneCreateViewModel))
                          .Returns(createdPhone);

            _ = _phoneRepositoryMock.Setup(repo => repo.Create(createdPhone, It.IsAny<CancellationToken>()))
                                  .ReturnsAsync(createdPhone);

            _ = _phoneViewModelMapperMock.Setup(mapper => mapper.MapModel(createdPhone))
                                       .Returns(newPhoneViewModel);

            // Act
            PhoneViewModel result = await taskService.Create(newPhoneCreateViewModel, It.IsAny<CancellationToken>());

            // Assert
            ClassicAssert.NotNull(result);
            ClassicAssert.That(result.Number, Is.EqualTo(newPhoneCreateViewModel.Number));
            // Additional assertions for other properties

        }
    }
}
