using EMart.Core.Repository;
using EMart.Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using NetCore.AutoRegisterDi;
using System.Reflection;

namespace EMart.API
{
    public static class ConfigureDI
    {
        public static void AddSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
                c.CustomSchemaIds(type => type.ToString());
            });
        }

        public static void RegisterService(this IServiceCollection services)
        {
            var coreAssembly = Assembly.Load("EMart.Core");
            var infrastructureAssembly = Assembly.Load("EMart.Infrastructure");

            var sqlToScan = Assembly.GetAssembly(typeof(IUnitOfWork));

            Assembly[] assemblies = { coreAssembly, infrastructureAssembly };
            services.RegisterAssemblyPublicNonGenericClasses(assemblies)
                .Where(c => c.Namespace.StartsWith("EMart") && !(c.Name.ToLower().EndsWith("unitofwork")))
                .AsPublicImplementedInterfaces(ServiceLifetime.Transient);

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
        }

        public static void RegisterDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            //SQL Connection String Get and Connection
            var connectionString = configuration.GetConnectionString("EMartDB");
            services.AddDbContext<Core.DatabaseContext>(options => options.UseSqlServer(connectionString, options => options.EnableRetryOnFailure()));
        }

    }
}
