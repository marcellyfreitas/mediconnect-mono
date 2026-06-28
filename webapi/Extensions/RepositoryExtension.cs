using WebApi.Models;
using WebApi.Repositories;

namespace WebApi.Extensions;

static class RepositoryExtension
{
    public static IServiceCollection AddCustomRepository(this IServiceCollection services)
    {
        services.AddTransient<IAuthenticationRepository<Administrator>, AdminAuthRepository>();
        services.AddTransient<IAuthenticationRepository<User>, UserAuthRepository>();
        services.AddTransient<IUserRepository<User>, UserRepository>();
        services.AddTransient<IRepository<Address>, AddressRepository>();
        services.AddTransient<IUserRepository<Administrator>, AdministratorRepository>();
        services.AddTransient<IRepository<Doctor>, DoctorRepository>();
        services.AddTransient<IRepository<Specialization>, SpecializationRepository>();
        services.AddTransient<IUserRepository<MedicalCenter>, MedicalCenterRepository>();
        services.AddTransient<IRepository<Appointment>, AppointmentRepository>();
        services.AddTransient<IRepository<AppointmentRating>, AppointmentRatingRepository>();
        services.AddTransient<IRepository<HealthPlan>, HealthPlanRepository>();
        services.AddTransient<IRepository<MedicalAgreement>, MedicalAgreementRepository>();
        services.AddTransient<IRepository<MedicalExam>, MedicalExamRepository>();

        return services;
    }
}