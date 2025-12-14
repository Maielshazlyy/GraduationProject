using Microsoft.EntityFrameworkCore;
using FluentValidation;
using Service_layer;
using DAL.Context;
using Microsoft.AspNetCore.Mvc;
using Domain_layer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using DAL.UnitOfWork;
using Domain_layer.Interfaces;
using Service_layer.Services;
using Service_layer.ServicesInterfaces;
using Service_layer.Services_Interfaces;
using DAL.Repositories;
using FluentValidation.AspNetCore;
using Microsoft.OpenApi.Models;

namespace digital_employee
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // -------------------------
            // 1) Controllers
            // -------------------------
            builder.Services.AddControllers();

            // -------------------------
            // 2) OpenAPI / Swagger
            // -------------------------
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                // إعدادات الـ Documentation
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Digital Employee API",
                    Version = "v1",
                    Description = "API for Graduation Project"
                });

                // إعدادات زرار القفل (Authorize) عشان التوكن
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter your JWT Token here without 'Bearer' prefix."
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
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
                        new string[] {}
                    }
                });
            });
            // -------------------------
            // 3) Database Connection
            // -------------------------
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // -------------------------
            // 4) Identity Configuration (نظام الهوية)
            // -------------------------
            builder.Services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();
            // -------------------------
            // 5) JWT Authentication (إعدادات التوكن - جديد)
            // -------------------------
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = builder.Configuration["JWT:Audience"],
                    ValidIssuer = builder.Configuration["JWT:Issuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]))
                };
            });
            // -------------------------
            // 6) Dependency Injection (تسجيل الخدمات - جديد)
            // -------------------------
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            builder.Services.AddScoped<IBusinessRepository, BusinessRepository>();
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IBusinessService, BusinessService>();

            // -------------------------
            // 7) FluentValidation Registration
            // -------------------------
            builder.Services.AddFluentValidationAutoValidation();
            builder.Services.AddValidatorsFromAssembly(typeof(AssemblyReference).Assembly);

            // -------------------------
            // 8) Custom Validation Response
            // -------------------------
            builder.Services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    var errors = context.ModelState
                        .Where(e => e.Value.Errors.Count > 0)
                        .Select(e => new
                        {
                            Field = e.Key,
                            Error = e.Value.Errors.First().ErrorMessage
                        });

                    return new BadRequestObjectResult(new
                    {
                        Message = "Validation Failed",
                        Errors = errors
                    });
                };
            });

            var app = builder.Build();

            // -------------------------
            // 9) Swagger in Development
            // -------------------------
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();

                // بيعمل صفحة الويب الملونة
                app.UseSwaggerUI();
            }

            // -------------------------
            // 10) Middlewares
            // -------------------------
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();

            // -------------------------
            // 8) Controllers Endpoint
            // -------------------------
            app.MapControllers();

            app.Run();
        }
    }
}
