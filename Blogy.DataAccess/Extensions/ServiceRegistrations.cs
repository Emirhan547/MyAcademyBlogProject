using Blogy.DataAccess.Context;
using Blogy.DataAccess.Repositories.CategoryRepositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Scrutor;

public static class ServiceRegistrations
{
    public static void AddRepositoriesExt(this IServiceCollection services, IConfiguration configuration)
    {
        // ---------------------------
        // 🔥 SCRUTOR → TÜM *Repository* sınıflarını tara (Category hariç)
        // ---------------------------
        services.Scan(scan => scan
            .FromAssemblyOf<AppDbContext>()
            .AddClasses(classes =>
                classes.Where(type =>
                    type.Name.EndsWith("Repository") &&
                    type != typeof(CategoryRepository)))        // Category hariç!
            .UsingRegistrationStrategy(RegistrationStrategy.Skip)
            .AsImplementedInterfaces()
            .WithScopedLifetime()
        );

        // ---------------------------
        // 🔥 CATEGORY REPOSITORY → MANUEL EKLE
        // ---------------------------
        services.AddScoped<ICategoryRepository, CategoryRepository>();

        // ---------------------------
        // DB CONTEXT
        // ---------------------------
        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            options.UseLazyLoadingProxies();
        });
    }
}
