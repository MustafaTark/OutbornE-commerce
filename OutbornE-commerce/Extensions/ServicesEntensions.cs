using OutbornE_commerce.BAL.AuthServices;
using OutbornE_commerce.BAL.Repositories.Brands;
using OutbornE_commerce.BAL.Repositories.Categories;
using OutbornE_commerce.FilesManager;

namespace OutbornE_commerce.Extensions
{
    public static class ServicesEntensions
    {
        public static void ConfigureLifeTime(this IServiceCollection services)
        {
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IBrandRepository, BrandRepository>();
            services.AddScoped<IFilesManager, OutbornE_commerce.FilesManager.FilesManager>();

        }
    }
}
