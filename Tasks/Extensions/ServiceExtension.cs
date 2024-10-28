using Taks.Core.Interfaces.IRepositories;
using Taks.Core.Interfaces.IServices;
using Taks.Core.Services;
using Tasks.Core.Interfaces.IRepositories;
using Tasks.Core.Interfaces.IServices;
using Tasks.Core.Services;
using Tasks.Infrastructure.Repositories;

namespace Tasks.API.Extensions
{
    public static class ServiceExtension
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            #region Services
            _ = services.AddSingleton<IUserContext, UserContext>();
            _ = services.AddScoped<IAuthService, AuthService>();
            _ = services.AddScoped<IUserService, UserService>();
            _ = services.AddScoped<IPhoneService, PhoneService>();
            #endregion

            #region Repositories
            _ = services.AddTransient<IAuthRepository, AuthRepository>();
            _ = services.AddTransient<IUserRepository, UserRepository>();
            _ = services.AddTransient<IPhoneRepository, PhoneRepository>();

            #endregion

            return services;
        }

    }
}
