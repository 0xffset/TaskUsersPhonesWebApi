using AutoMapper;
using Taks.Core.Interfaces.IMapper;
using Taks.Core.Mapper;
using Tasks.Core.Entities.General;
using Tasks.Core.Entities.ViewModels;

namespace Task.API.Extensions
{
    public static class MapperExtension
    {
        public static IServiceCollection RegisterMapperService(this IServiceCollection services)
        {

            #region Mapper
            _ = services.AddSingleton(sp => new MapperConfiguration(cfg =>
            {

                _ = cfg.CreateMap<User, UserViewModel>();
                _ = cfg.CreateMap<UserViewModel, User>();
                _ = cfg.CreateMap<UserCreateViewModel, User>();
                _ = cfg.CreateMap<UserUpdateViewModel, User>();

                _ = cfg.CreateMap<Phone, PhoneViewModel>();
                _ = cfg.CreateMap<PhoneViewModel, Phone>();
                _ = cfg.CreateMap<PhoneCreateViewModel, Phone>();
                _ = cfg.CreateMap<PhoneUpdateViewModel, Phone>();

            }).CreateMapper());


            // User Mapper
            _ = services.AddSingleton<IBaseMapper<User, UserViewModel>, BaseMapper<User, UserViewModel>>();
            _ = services.AddSingleton<IBaseMapper<UserViewModel, User>, BaseMapper<UserViewModel, User>>();
            _ = services.AddSingleton<IBaseMapper<UserCreateViewModel, User>, BaseMapper<UserCreateViewModel, User>>();
            _ = services.AddSingleton<IBaseMapper<UserUpdateViewModel, User>, BaseMapper<UserUpdateViewModel, User>>();
            // Phone Mapper
            _ = services.AddSingleton<IBaseMapper<Phone, PhoneViewModel>, BaseMapper<Phone, PhoneViewModel>>();
            _ = services.AddSingleton<IBaseMapper<PhoneViewModel, Phone>, BaseMapper<PhoneViewModel, Phone>>();
            _ = services.AddSingleton<IBaseMapper<PhoneCreateViewModel, Phone>, BaseMapper<PhoneCreateViewModel, Phone>>();
            _ = services.AddSingleton<IBaseMapper<PhoneUpdateViewModel, Phone>, BaseMapper<PhoneUpdateViewModel, Phone>>();
            #endregion 
            return services;
        }
    }
}
