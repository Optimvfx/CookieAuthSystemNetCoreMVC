using BLL.AutoMapper.Profiles;
using BLL.Models.UserModels;
using BLL.Services;
using Microsoft.Extensions.DependencyInjection;

namespace BLL;

public static class DependencyInjection
{
    public static IServiceCollection AddServices(this IServiceCollection
        services)
    {
        services.AddScoped<UserService>();
        services.AddAutoMapper(typeof(AutoMapperProfile).Assembly);
        services.AddMemoryCache();
        
        return services;
    }
}
