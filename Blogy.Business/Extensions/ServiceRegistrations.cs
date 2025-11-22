using Blogy.Business.Mappings;
using Blogy.Business.Services.AiServices;
using Blogy.Business.Services.BlogServices;
using Blogy.Business.Services.CategoryServices;
using Blogy.Business.Services.CommentServices;
using Blogy.Business.Services.ToxicityServices;
using Blogy.Business.Validators.CategoryValidators;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using Scrutor;
using System.Reflection;

namespace Blogy.Business.Extensions
{
    public static class ServiceRegistrations
    {
        public static void AddServicesExt(this IServiceCollection services)
        {
            // ---------------------------
            // 🔥 SCRUTOR → Business Servisleri Tara
            // ---------------------------
            services.Scan(opt =>
            {
                opt.FromAssemblies(Assembly.GetExecutingAssembly())
                    .AddClasses(classes => classes
                        .Where(type =>
                            type.Namespace != null &&
                            !type.Namespace.Contains("CategoryServices")))  // Category manuel eklenecek
                    .UsingRegistrationStrategy(RegistrationStrategy.Skip)
                    .AsMatchingInterface()
                    .AsImplementedInterfaces()
                    .WithScopedLifetime();
            });

            // ---------------------------
            // 🔥 CATEGORY SERVICE → MANUEL BAĞLANIYOR
            // ---------------------------
            services.AddScoped<ICategoryService, CategoryService>();

            // ---------------------------
            // AUTO MAPPER
            // ---------------------------
            services.AddAutoMapper(typeof(CategoryMappings).Assembly);

            // ---------------------------
            // FLUENT VALIDATION
            // ---------------------------
            services
                .AddFluentValidationAutoValidation()
                .AddFluentValidationClientsideAdapters()
                .AddValidatorsFromAssembly(typeof(CreateCategoryValidator).Assembly);
        }
    }
}
