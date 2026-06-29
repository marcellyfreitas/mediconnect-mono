using WebApi.Models;
using WebApi.Services;

namespace WebApi.Extensions;

static class ServiceExtension
{
    public static IServiceCollection AddCustomServices(this IServiceCollection services)
    {
        services.AddTransient<IAuthenticationService<Administrator>, AdminAuthService>();
        services.AddTransient<IAuthenticationService<User>, UserAuthService>();
        services.AddTransient<IUserService<User>, UserService>();
        services.AddTransient<IService<Address>, AddressService>();
        services.AddTransient<IUserService<Administrator>, AdministratorService>();
        services.AddTransient<IService<Doctor>, DoctorService>();
        services.AddTransient<IService<Specialization>, SpecializationService>();
        services.AddTransient<IUserService<MedicalCenter>, MedicalCenterService>();
        services.AddTransient<IService<Appointment>, AppointmentService>();
        services.AddTransient<IService<AppointmentRating>, AppointmentRatingService>();
        services.AddTransient<IService<HealthPlan>, HealthPlanService>();
        services.AddTransient<IService<MedicalAgreement>, MedicalAgreementService>();
        services.AddTransient<IService<MedicalExam>, MedicalExamService>();

        return services;
    }
}