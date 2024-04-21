
using AuthenticationMechanism.Services;
using AuthenticationMechanism.tokenservice;
using CloudinaryDotNet;
using EduNexBL.AutoMapper;
using EduNexBL.Base;
using EduNexBL.IRepository;
using EduNexBL.Repository;
using EduNexBL.UnitOfWork;
using EduNexDB.Context;
using EduNexDB.Entites;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace EduNexAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddDbContext<EduNexContext>(
            options => options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer"))
            );
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddAutoMapper(typeof(MappingProfile));
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IExam, ExamRepo>();
            builder.Services.AddScoped<IStudent, StudentRepo>();
            builder.Services.AddScoped<IStudentExam, StudentExamRepo>();

           

            //configure identity
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>

            {

                options.Password.RequiredLength = 8;

                options.Password.RequireDigit = true;

                options.Password.RequireLowercase = true;

            })

                .AddEntityFrameworkStores<EduNexContext>()

                .AddDefaultTokenProviders();

            //inject the token service 
            builder.Services.AddScoped<TokenService>();
            //inject cloudinary service 
            // Add the Cloudinary service to the DI container
           // builder.Services.AddScoped<CloudinaryService>();
            builder.Services.AddScoped<ICloudinaryService, CloudinaryService>();


            // Configure the Cloudinary service

            builder.Services.Configure<CloudinarySettings>(builder.Configuration.GetSection("Cloudinary"));


            // Register the Cloudinary service

            builder.Services.AddSingleton(x =>

            {

                var settings = x.GetRequiredService<IOptions<CloudinarySettings>>().Value;

                var account = new Account(settings.CloudName, settings.ApiKey, settings.ApiSecret);

                return new Cloudinary(account);

            });

            //configure jwt
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = "https://localhost:7156", // Specify the issuer of the token
                    ValidAudience = "https://localhost:7156", // Specify the audience for the token
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("D6AE1F1B3F45D32D2E18B5E9F1D301298C1C87223578F8D063DAC8E2E255971B")) // Specify the secret key used to sign the token
                };
            });
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
