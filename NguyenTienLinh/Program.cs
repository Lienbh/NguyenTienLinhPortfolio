
using AppApi.IRepository;
using AppApi.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NguyenTienLinh.Context;
using System.Text;

namespace NguyenTienLinh
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            //gọi đến login controller
            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("nguyentienlinh_2")));
            builder.Services.AddControllers();
            builder.Services.AddTransient<IAboutsRepos, AboutRepo>();
            builder.Services.AddTransient<ICategoriesRepo, CategoriesRepos>();
            builder.Services.AddTransient<IVideosRepo, VideoRepo>();
            builder.Services.AddTransient<IUserRepo, UserRepo>();
            builder.Services.AddTransient<IBackGroundsRepo, BackGroundRepo>();



            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
           
            builder.Services.AddCors(option =>
            {
                option.AddPolicy("AllowForntend",
                    builder =>
                    {
                        builder.WithOrigins("http://localhost:5173")
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

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
                app.UseDeveloperExceptionPage();
            }
            
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
