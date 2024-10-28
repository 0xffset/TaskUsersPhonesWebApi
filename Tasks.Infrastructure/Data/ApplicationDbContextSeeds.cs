using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Tasks.Core.Entities.General;
using Task = System.Threading.Tasks.Task;

namespace Tasks.Infrastructure.Data
{
    public class ApplicationDbContextSeeds
    {
        public static async Task SeedsAsync(IServiceProvider serviceProvider, ILoggerFactory loggerFactory, int? retry = 0)
        {
            int retryFor = retry ?? 0;
            ApplicationDbContext appContext = serviceProvider.GetRequiredService<ApplicationDbContext>();
            UserManager<User> UserManager = serviceProvider.GetRequiredService<UserManager<User>>();

            try
            {
                // Adding users 
                if (!appContext.Users.Any())
                {
                    User defaultUser = new()
                    {
                        FullName = "Rogger G. Díaz",
                        UserName = "rogger",
                        Email = "roggergarciadiaz@gmail.com",
                        Created = DateTime.Now,
                        LastLogin = DateTime.Now,
                        IsActive = true,

                    };
                    IdentityResult identityResult = await UserManager.CreateAsync(defaultUser, "Passw0rd#");

                    if (identityResult.Succeeded)
                    {
                        // Assing the new role
                        _ = await UserManager.AddToRoleAsync(defaultUser, "ADMIN");
                    }

                }

                // Adding phones
                if (!appContext.Phones.Any())
                {
                    User? defaultuser = await UserManager.FindByEmailAsync("roggergarciadiaz@gmail.com");

                    using Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction transaction = appContext.Database.BeginTransaction();
                    appContext.Phones.AddRange(GenerateRandomPhones(10, 1));
                    _ = await appContext.SaveChangesAsync();
                    transaction.Commit();
                }

            }
            catch (Exception ex)
            {

                if (retryFor < 10)
                {
                    retryFor++;
                    ILogger<ApplicationDbContextSeeds> log = loggerFactory.CreateLogger<ApplicationDbContextSeeds>();
                    log.LogError(ex.Message);
                    await SeedsAsync(serviceProvider, loggerFactory, retryFor);
                }
            }
        }


        private static ICollection<Phone> GenerateRandomPhones(int numberOfPhones, int idUser)
        {
            List<Phone> phones = [];

            for (int i = 0; i < numberOfPhones; i++)
            {
                phones.Add(new Phone
                {
                    Number = Faker.Phone.Number().ToString(),
                    CityCode = GetRandomCityCode(),
                    CountryCode = GetRandomCountryCode(),
                    EntryBy = 1,
                    EntryDate = DateTime.Now,
                    UserId = idUser


                });

            }

            return phones;
        }

        private static string GetRandomCityCode()
        {
            Random _random = new();
            return _random.Next(100, 999).ToString();
        }

        private static string GetRandomCountryCode()
        {

            Random _random = new();
            return _random.Next(1, 100).ToString();
        }
    }
}


