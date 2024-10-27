using Microsoft.AspNetCore.Identity.UI.Services;
using OutbornE_commerce.BAL.AuthServices;
using OutbornE_commerce.BAL.EmailServices;
using OutbornE_commerce.BAL.Repositories.AboutUs;
using OutbornE_commerce.BAL.Repositories.Address;
using OutbornE_commerce.BAL.Repositories.Brands;
using OutbornE_commerce.BAL.Repositories.Categories;
using OutbornE_commerce.BAL.Repositories.Cities;
using OutbornE_commerce.BAL.Repositories.Colors;
using OutbornE_commerce.BAL.Repositories.ContactUs;
using OutbornE_commerce.BAL.Repositories.ContactUsSetups;
using OutbornE_commerce.BAL.Repositories.Countries;
using OutbornE_commerce.BAL.Repositories.Currencies;
using OutbornE_commerce.BAL.Repositories.FAQs;
using OutbornE_commerce.BAL.Repositories.Hashtags;
using OutbornE_commerce.BAL.Repositories.Headers;
using OutbornE_commerce.BAL.Repositories.HomeSections;
using OutbornE_commerce.BAL.Repositories.InquiryTypes;
using OutbornE_commerce.BAL.Repositories.Newsletters;
using OutbornE_commerce.BAL.Repositories.NewsletterSubscribers;
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
            services.AddScoped<IAddressRepository, AddressRepository>();
            services.AddScoped<IHashtagRepository, HashtagRepository>();
            services.AddScoped<IFAQRepository , FAQRepository>();
            services.AddScoped<IAboutUsRepository, AboutUsRepository>();
            services.AddScoped<IContactUsSetupRepository, ContactUsSetupRepository>();
            services.AddScoped<IInquiryTypeRepository, InquiryTypeRepository>();
            services.AddScoped<IContactUsRepository, ContactUsRepository>();

            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IProductSizeRepository, ProductSizeRepository>();
            services.AddScoped<IProductColorRepository, ProductColorRepository>();
            services.AddScoped<IProductImageRepository, ProductImageRepository>(); 
            
            services.AddScoped<IProductCategoryRepository, ProductCategoryRepository>();

            services.AddScoped<INewsletterRepository, NewsletterRepository>();
            services.AddScoped<INewsletterSubscriberRepository, NewsletterSubscriberRepository>();

            services.AddScoped<IEmailSenderCustom, EmailSender>();

        }
    }
}
