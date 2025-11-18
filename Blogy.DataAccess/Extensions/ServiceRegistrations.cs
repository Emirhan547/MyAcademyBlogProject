using Blogy.DataAccess.Context;
using Blogy.Entity.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Scrutor;

public static class ServiceRegistrations
{
    public static void AddRepositoriesExt(this IServiceCollection services, IConfiguration configuration)
    {
        // -------------------------------
        // SCRUTOR - SADECE DATAACCESS TARA
        // -------------------------------
        services.Scan(opt =>
        {
            opt.FromAssemblyOf<AppDbContext>()   // 🔥 SADECE Blogy.DataAccess taranır
                .AddClasses(publicOnly: false)
                .UsingRegistrationStrategy(RegistrationStrategy.Skip)
                .AsMatchingInterface()
                .AsImplementedInterfaces()
                .WithScopedLifetime();
        });

        // -------------------------------
        // DB CONTEXT
        // -------------------------------
        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            options.UseLazyLoadingProxies();
        });

        // -------------------------------
        // IDENTITY
        // -------------------------------
        services.AddIdentity<AppUser, AppRole>(options =>
        {
            options.User.RequireUniqueEmail = true;
        })
        .AddEntityFrameworkStores<AppDbContext>();
    }
}
