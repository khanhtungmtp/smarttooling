using AutoMapper;
using SmartTooling_API.Helpers.AutoMapper;

namespace SmartTooling_API.Configurations
{
    public static class AutoMapperRegisterConfig
    {
         public static void AddAutoMapperConfiguration(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            services.AddScoped<IMapper>(sp => {
                return new Mapper(AutoMapperConfig.RegisterMappings());
            });

            services.AddSingleton(AutoMapperConfig.RegisterMappings());
        }
    }
}