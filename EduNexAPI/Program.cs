using Amazon.S3;
using AuthenticationMechanism.Services;
using AuthenticationMechanism.tokenservice;
using BunnyCDN.Net.Storage;
using CloudinaryDotNet;
using EduNexAPI.Helpers;
using EduNexBL;
using EduNexBL.AutoMapper;
using EduNexBL.Base;
using EduNexBL.IRepository;
using EduNexBL.Repository;
using EduNexBL.Services;
using EduNexBL.UnitOfWork;
using EduNexDB.Context;
using EduNexDB.Entites;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace EduNexAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var configuration = builder.Configuration;

                    builder.Services.Configure<DatabaseSettings>(
             builder.Configuration.GetSection("Database")
         );

            builder.Services.AddSingleton<IConnectionStringProvider, ConnectionStringProvider>();
            builder.Services.AddDbContext<EduNexContext>((serviceProvider, options) =>
            {
                var provider = serviceProvider.GetRequiredService<IConnectionStringProvider>();
                var connectionString = provider.GetConnectionString();
                options.UseSqlServer(connectionString);
            });

            builder.Services.AddControllers();
            builder.Services.AddAutoMapper(typeof(MappingProfile));
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IAdminRepository, AdminRepository>();
            builder.Services.AddScoped<IStorageService, StorageService>();

            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequiredLength = 8;
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
            })
            .AddEntityFrameworkStores<EduNexContext>()
            .AddDefaultTokenProviders();

            builder.Services.AddScoped<TokenService>();
            builder.Services.AddScoped<IFiles, CloudinaryService>();
            builder.Services.AddScoped<IWallet, WalletRepo>();
            builder.Services.AddScoped<ITransaction, TransactionRepo>();
            builder.Services.AddScoped<CouponService>();

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = "http://localhost:5293/",
                        ValidAudience = "http://localhost:4200/",
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(configuration["Jwt:Key"] ?? "DefaultJWTKeyMustBeChanged")
                        )
                    };
                });

            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader();
                });
            });

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "EduNex", Version = "v1" });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });

            var app = builder.Build();

            app.UseHttpsRedirection();
            app.Urls.Add("http://0.0.0.0:5293");

            app.UseRouting();
            app.UseCors();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "EduNex API v1"));

            app.Run();
        }
    }
}
