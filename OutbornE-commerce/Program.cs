using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using OutbornE_commerce.BAL.AuthServices;
using OutbornE_commerce.BAL.Dto;
using OutbornE_commerce.BAL.Repositories.BaseRepositories;
using OutbornE_commerce.BAL.Repositories.Brands;
using OutbornE_commerce.BAL.Repositories.Categories;
using OutbornE_commerce.DAL.Data;
using OutbornE_commerce.DAL.Models;
using OutbornE_commerce.Extensions;
using OutbornE_commerce.FilesManager;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//	.AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));
string[] CorsOrigins;
CorsOrigins = builder.Configuration["Cors_Origins"].Split(",");

builder.Services.Configure<JWT>(builder.Configuration.GetSection("JWT"));
builder.Services.AddCors(options => {
    options.AddPolicy("_myAllowSpecificOrigins", builder =>
    builder.AllowAnyMethod()
    .AllowAnyHeader()
    .AllowCredentials()
    .SetIsOriginAllowed((hosts) => true));
});


builder.Services.AddControllers();

builder.Services.AddIdentity<User, IdentityRole>()
				.AddEntityFrameworkStores<ApplicationDbContext>()
				.AddUserManager<UserManager<User>>()
				.AddRoles<IdentityRole>()
				.AddDefaultTokenProviders()
				.AddSignInManager<SignInManager<User>>();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
				options.UseSqlServer(
					builder.Configuration.GetConnectionString("DefaultConnection"),
						b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));

builder.Services.AddAuthentication(options =>
{
	options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
	options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
				.AddJwtBearer(o =>
				{
					o.RequireHttpsMetadata = false;
					o.SaveToken = false;
					o.TokenValidationParameters = new TokenValidationParameters
					{
						ValidateIssuerSigningKey = true,
						ValidateIssuer = true,
						ValidateAudience = true,
						ValidateLifetime = true,
						ValidIssuer = builder.Configuration["JWT:Issuer"],
						ValidAudience = builder.Configuration["JWT:Audience"],
						IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"])),
						ClockSkew = TimeSpan.Zero
					};
				});

builder.Services.AddSwaggerGen(c => {
	c.SwaggerDoc("v1", new OpenApiInfo
	{
		Title = "Outborn_API",
		Version = "v1"
	});
	c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
	{
		Name = "Authorization",
		Type = SecuritySchemeType.Http,
		Scheme = "Bearer",
		BearerFormat = "JWT",
		In = ParameterLocation.Header,
		Description = "Add your valid token to be able to Outborn-Api",
	});
	c.AddSecurityRequirement(new OpenApiSecurityRequirement {
		{
			new OpenApiSecurityScheme {
				Reference = new OpenApiReference {
					Type = ReferenceType.SecurityScheme,
						Id = "Bearer"
				}
			},
			new string[] {}
		}
	});
});

builder.Services.ConfigureLifeTime();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
	app.UseSwagger();
	app.UseSwaggerUI();
//}
app.UseCors("_myAllowSpecificOrigins");
app.UseStaticFiles();
app.UseHttpsRedirection();

//app.UseJwtExpirationHandling();

// End Localization 
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
