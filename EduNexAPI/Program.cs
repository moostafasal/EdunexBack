using Amazon.S3;
using AuthenticationMechanism.Services;
using AuthenticationMechanism.tokenservice;
using BunnyCDN.Net.Storage;
using CloudinaryDotNet;
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
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduNexAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            builder.Services.AddControllers();
                string connectionStringTemplate = configuration.GetConnectionString("DefaultConnection")!;
       string connectionString = connectionStringTemplate
      .Replace("$MYSQL_HOST", Environment.GetEnvironmentVariable("MYSQL_HOST"))
      .Replace("$MYSQL_PASSWORD", Environment.GetEnvironmentVariable("MYSQL_PASSWORD"));

            builder.Services.AddDbContext<EduNexContext>(
                options => options.UseSqlServer(configuration.GetConnectionString("connectionString"))
            );
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
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]))
                    };
                });

            // Add CORS policy
            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder.WithOrigins("https://edu-nex-front.vercel.app", "https://edu-nex-dashboard.vercel.app") // Add your allowed origin here
                           .AllowAnyMethod()
                           .AllowAnyHeader()
                           .AllowCredentials();
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
                        new string[] { }
                    }
                });
            });

            var app = builder.Build();

            app.UseHttpsRedirection();

            app.UseRouting(); // Add UseRouting here

            app.UseCors(); // Apply the CORS policy

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
