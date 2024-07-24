﻿using Microsoft.AspNetCore.Identity;
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
		public DbSet<UserAddress> UserAddresses { get; set; }
		public DbSet<Brand> Brands { get; set; }
		public DbSet<Category> Categories { get; set; }
        public DbSet<Header> Headers { get; set; }
        public DbSet<HomeSection> HomeSections { get; set; }
        public DbSet<Color> Colors { get; set; }
        public DbSet<Size> Sizes { get; set; }
        public DbSet<SEO> SEOs {  get; set; }
        public DbSet<ReceivePoints> ReceivePoints { get; set; }
        public DbSet<Currency> Currencies { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<SMTPServer> SMTPServers { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
	  : base(options) { }
		protected override void OnModelCreating(ModelBuilder builder)
		{
			builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            builder.ApplyConfiguration(new RoleConfigrations());

            builder.Entity<Category>()
           .HasOne(c => c.ParentCategory)
           .WithMany(c => c.SubCategories)
           .HasForeignKey(c => c.ParentCategoryId)
           .OnDelete(DeleteBehavior.Restrict);
			
			builder.Entity<Brand>()
           .HasOne(c => c.ParentBrand)
           .WithMany(c => c.SubBrands)
           .HasForeignKey(c => c.ParentBrandId)
           .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(builder);
			builder.ApplyConfiguration(new RoleConfigrations());
            builder.Entity<IdentityRole>().ToTable("Roles");
            builder.Entity<IdentityUserRole<string>>().ToTable("UserRoles");
            builder.Entity<IdentityUserClaim<string>>().ToTable("UserClaims");
            builder.Entity<IdentityUserLogin<string>>().ToTable("UserLogins");
            builder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaims");
            builder.Entity<IdentityUserToken<string>>().ToTable("UserTokens");
            base.OnModelCreating(builder);
		}
	}
}
