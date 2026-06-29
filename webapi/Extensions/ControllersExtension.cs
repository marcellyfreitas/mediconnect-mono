using WebApi.Models;

namespace WebApi.Extensions;

static class ControllersExtension
{
    public static IServiceCollection AddCustomControllers(this IServiceCollection services)
    {
        services.AddControllers().ConfigureApiBehaviorOptions(options =>
        {
            options.SuppressModelStateInvalidFilter = true;
        });

        return services;
    }
}