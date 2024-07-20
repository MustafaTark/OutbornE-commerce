using Microsoft.EntityFrameworkCore;
using OutbornE_commerce.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace OutbornE_commerce.DAL.Data
{
	public class ApplicationDbContext : DbContext
	{
		public DbSet<User> Users { get; set; }
		public DbSet<Address> Addresses { get; set; }
		public DbSet<UserAddress> UserAddresses { get; set; }
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
	  : base(options) { }
		protected override void OnModelCreating(ModelBuilder builder)
		{
			builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
			builder.ApplyConfiguration(new RoleConfigrations());

			base.OnModelCreating(builder);
		}
	}
}
