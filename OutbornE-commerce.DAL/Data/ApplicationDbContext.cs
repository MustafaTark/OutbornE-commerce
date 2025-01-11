using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OutbornE_commerce.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace OutbornE_commerce.DAL.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Header> Headers { get; set; }
        public DbSet<HomeSection> HomeSections { get; set; }
        public DbSet<Color> Colors { get; set; }
        public DbSet<Size> Sizes { get; set; }
        //public DbSet<SEO> SEOs { get; set; }
        public DbSet<ReceivePoints> ReceivePoints { get; set; }
        //public DbSet<Currency> Currencies { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Country> Countries { get; set; }
        //public DbSet<SMTPServer> SMTPServers { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductColor> ProductColors { get; set; }
        public DbSet<ProductSize> ProductSizes { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        //public DbSet<Newsletter> Newsletters { get; set; }
        //public DbSet<NewsletterSubscriber> NewsletterSubscribers { get; set; }
        //public DbSet<Hashtag> Hashtags { get; set; }
        public DbSet<FAQ> FAQs { get; set; }
        public DbSet<AboutUs> AboutUs { get; set; }
        public DbSet<ContactUsSetup> ContactUsSetups { get; set; }
        public DbSet<InquiryType> InquiryTypes { get; set; }
        public DbSet<ContactUs> ContactUs { get; set; }
        public DbSet<BagItem> BagItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
      : base(options) { }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            //builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            //         builder.ApplyConfiguration(new RoleConfigrations());

            builder.Entity<Category>()
           .HasOne(c => c.ParentCategory)
           .WithMany(c => c.SubCategories)
           .HasForeignKey(c => c.ParentCategoryId)
           .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(builder);
            builder.ApplyConfiguration(new RoleConfigrations());
            builder.Entity<IdentityRole>().ToTable("Roles");
            builder.Entity<IdentityUserRole<string>>().ToTable("UserRoles"); 
            builder.Entity<IdentityUserClaim<string>>().ToTable("UserClaims");
            builder.Entity<IdentityUserLogin<string>>().ToTable("UserLogins");
            builder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaims");
            builder.Entity<IdentityUserToken<string>>().ToTable("UserTokens");


            builder.Entity<Product>().HasQueryFilter(p => !p.IsDeleted).HasIndex(p=>p.IsDeleted);

            base.OnModelCreating(builder);
        }
        public async Task EnsureRolesAsync(RoleManager<IdentityRole> roleManager)
        {
            string[] roleNames = { "Admin", "User" };
            foreach (var roleName in roleNames)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }
        }
    }
}
