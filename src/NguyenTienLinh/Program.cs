using System.Net;
using AppApi.IRepository;
using AppApi.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NguyenTienLinh.Context;
using System.Text;
using NguyenTienLinh.Repository;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.AspNetCore.Server.IISIntegration;

namespace NguyenTienLinh
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Force TLS 1.2 for SQL Server connections
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            var builder = WebApplication.CreateBuilder(args);

            // Configure request body size limits
            builder.Services.Configure<IISServerOptions>(options =>
            {
                options.MaxRequestBodySize = null; // No limit
            });

            builder.Services.Configure<KestrelServerOptions>(options =>
            {
                options.Limits.MaxRequestBodySize = null; // No limit
            });

            builder.Services.Configure<FormOptions>(options =>
            {
                options.MultipartBodyLengthLimit = long.MaxValue; // No limit
                options.ValueLengthLimit = int.MaxValue; // No limit
                options.ValueCountLimit = int.MaxValue; // No limit
            });

            // Add services
            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("Conn"), sqlOptions =>
                {
                    sqlOptions.EnableRetryOnFailure(
                        maxRetryCount: 3,
                        maxRetryDelay: TimeSpan.FromSeconds(30),
                        errorNumbersToAdd: null);
                    sqlOptions.CommandTimeout(60);
                });
            });
            builder.Services.AddControllers();
            builder.Services.AddTransient<IAboutsRepos, AboutRepo>();
            builder.Services.AddTransient<ICategoriesRepo, CategoriesRepos>();
            builder.Services.AddTransient<IVideosRepo, VideoRepo>();
            builder.Services.AddTransient<IUserRepo, UserRepo>();
            builder.Services.AddTransient<IBackGroundsRepo, BackGroundRepo>();
            builder.Services.AddTransient<IGalleryRepo, GalleryRepo>();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowFrontend",
                    policy =>
                    {
                        policy.AllowAnyOrigin()
                            .AllowAnyHeader()
                            .AllowAnyMethod();
                    });
            });

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(option =>
                {
                    option.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = "Jwt:Issuer",
                        ValidAudience = "Jwt:Audience",
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("Jwt:Key"))
                    };
                });

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseCors("AllowFrontend");
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}
