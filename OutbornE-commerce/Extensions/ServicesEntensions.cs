﻿using OutbornE_commerce.BAL.AuthServices;
using OutbornE_commerce.BAL.Repositories.Brands;
using OutbornE_commerce.BAL.Repositories.Categories;
using OutbornE_commerce.BAL.Repositories.Cities;
using OutbornE_commerce.BAL.Repositories.Colors;
using OutbornE_commerce.BAL.Repositories.Countries;
using OutbornE_commerce.BAL.Repositories.Currencies;
using OutbornE_commerce.BAL.Repositories.Headers;
using OutbornE_commerce.BAL.Repositories.HomeSections;
using OutbornE_commerce.BAL.Repositories.ProductCateories;
using OutbornE_commerce.BAL.Repositories.ProductColors;
using OutbornE_commerce.BAL.Repositories.ProductImages;
using OutbornE_commerce.BAL.Repositories.Products;
using OutbornE_commerce.BAL.Repositories.ProductSizes;
using OutbornE_commerce.BAL.Repositories.ReceivePoints;
using OutbornE_commerce.BAL.Repositories.SEOs;
using OutbornE_commerce.BAL.Repositories.Sizes;
using OutbornE_commerce.BAL.Repositories.SMTP_Server;
using OutbornE_commerce.BAL.Repositories.Tickets;
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
            services.AddScoped<ICityRepository, CityRepository>();
            services.AddScoped<ICountryRepository, CountryRepository>();
            services.AddScoped<ISMTPRepository, SMTPRepository>();
            services.AddScoped<ITicketRepository, TicketRepository>();

            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IProductSizeRepository, ProductSizeRepository>();
            services.AddScoped<IProductColorRepository, ProductColorRepository>();
            services.AddScoped<IProductImageRepository, ProductImageRepository>(); 
            
            services.AddScoped<IProductCategoryRepository, ProductCategoryRepository>();

        }
    }
}
