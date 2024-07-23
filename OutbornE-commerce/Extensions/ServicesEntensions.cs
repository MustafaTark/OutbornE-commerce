using OutbornE_commerce.BAL.AuthServices;
using OutbornE_commerce.BAL.Repositories.Brands;
using OutbornE_commerce.BAL.Repositories.Categories;
using OutbornE_commerce.BAL.Repositories.Colors;
using OutbornE_commerce.BAL.Repositories.Currencies;
using OutbornE_commerce.BAL.Repositories.Headers;
using OutbornE_commerce.BAL.Repositories.HomeSections;
using OutbornE_commerce.BAL.Repositories.ReceivePoints;
using OutbornE_commerce.BAL.Repositories.SEOs;
using OutbornE_commerce.BAL.Repositories.Sizes;
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
            services.AddScoped<IHeaderRepository, HeaderRepository>();
            services.AddScoped<IColorRepository, ColorRepository>();
            services.AddScoped<IHomeSectionRepository, HomeSectionRepository>();
            services.AddScoped<ISizeRepository, SizeRepository>();
            services.AddScoped<ISEORepository, SEORepository>();
            services.AddScoped<IReceivePointsRepository, ReceivePointsRepository>();
            services.AddScoped<ICurrencyRepository, CurrencyRepository>();

        }
    }
}
