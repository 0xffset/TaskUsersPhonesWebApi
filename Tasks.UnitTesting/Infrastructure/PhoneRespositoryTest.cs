using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Moq;
using NUnit.Framework;
using Tasks.Infrastructure.Data;
using Tasks.Infrastructure.Repositories;

namespace Tasks.UnitTesting.Infrastructure
{
    public class PhoneRespositoryTest
    {
        private Mock<ApplicationDbContext> _dbContextMock;
        private PhoneRepository _phoneRepository;

        [SetUp]
        public void Setup()
        {
            _dbContextMock = new Mock<ApplicationDbContext>(new DbContextOptions<ApplicationDbContext>());
            _phoneRepository = new PhoneRepository(_dbContextMock.Object);
        }

        [Test]
        public async System.Threading.Tasks.Task AddAsyncValidPhoneReturnsAddedPhone()
        {

            // Arrange
            Tasks.Core.Entities.General.Phone newPhone = new()
            {
                CityCode = "1",
                Number = "909-000-2313",
                CountryCode = "+909",
                UserId = 2
            };

            Mock<DbSet<Tasks.Core.Entities.General.Phone>> phoneDbSetMock = new();

            _ = _dbContextMock.Setup(db => db.Set<Tasks.Core.Entities.General.Phone>())
                          .Returns(phoneDbSetMock.Object);

            _ = phoneDbSetMock.Setup(dbSet => dbSet.AddAsync(newPhone, default))
                            .ReturnsAsync((EntityEntry<Tasks.Core.Entities.General.Phone>)null);

            // Act
            Tasks.Core.Entities.General.Phone result = await _phoneRepository.Create(newPhone, It.IsAny<CancellationToken>());


            // Assert
            NUnit.Framework.Legacy.ClassicAssert.NotNull(result);
            NUnit.Framework.Legacy.ClassicAssert.That(result, Is.EqualTo(newPhone));
        }
    }
}
