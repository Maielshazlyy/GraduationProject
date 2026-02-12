using Microsoft.EntityFrameworkCore;
using FluentValidation;
using Service_layer;
using DAL.Context;
using Microsoft.AspNetCore.Mvc;
using Domain_layer.Models;
using Domain_layer.Constants;
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
            
            // CORS Configuration
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader();
                });
            });

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
                    Description = "Enter your JWT token. Example: \"Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...\"\n\nYou can get the token from /api/Auth/login or /api/Auth/register endpoints."
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
            builder.Services.AddIdentity<User, IdentityRole>(options =>
            {
                
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 6;

                options.User.RequireUniqueEmail = true;
            })
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
                options.RequireHttpsMetadata = false; // خليها true في الإنتاج مع HTTPS
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,
                    ValidAudience = builder.Configuration["JWT:Audience"],
                    ValidIssuer = builder.Configuration["JWT:Issuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"] ?? throw new InvalidOperationException("JWT:Key is not configured")))
                };
            });

            // -------------------------
            // 6) Authorization Policies (صلاحيات حسب الدور)
            // -------------------------
            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminOnly", policy =>
                    policy.RequireRole(Roles.Admin));

                options.AddPolicy("OwnerOrAdmin", policy =>
                    policy.RequireRole(Roles.Owner, Roles.Admin));

                options.AddPolicy("AgentOrOwnerOrAdmin", policy =>
                    policy.RequireRole(Roles.Agent, Roles.Owner, Roles.Admin));
            });

            // -------------------------
            // 7) Dependency Injection (تسجيل الخدمات - جديد)
            // -------------------------
            
            // Generic Repository
            builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            
            // Specific Repositories
            builder.Services.AddScoped<IBusinessRepository, BusinessRepository>();
            builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
            builder.Services.AddScoped<IOrderRepository, OrderRepository>();
            builder.Services.AddScoped<IOrderItemRepository, OrderItemRepository>();
            builder.Services.AddScoped<ITicketRepository, TicketRepository>();
            builder.Services.AddScoped<IMenuItemRepository, MenuItemRepository>();
            builder.Services.AddScoped<IMenuCategoryRepository, MenuCategoryRepository>();
            builder.Services.AddScoped<IWorkingHoursRepository, WorkingHoursRepository>();
            builder.Services.AddScoped<IFeedbackRepository, FeedbackRepository>();
            builder.Services.AddScoped<IMessageRepository, MessageRepository>();
            builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
            builder.Services.AddScoped<IReportRepository, ReportRepository>();
            builder.Services.AddScoped<IKnowledgeBaseRepository, KnowledgeBaseRepository>();
            builder.Services.AddScoped<IInteractionRepository, InteractionRepository>();
            builder.Services.AddScoped<ISubscriptionRepository, SubscriptionRepository>();
            builder.Services.AddScoped<IPaymentTransactionRepository, PaymentTransactionRepository>();
            builder.Services.AddScoped<ISettingRepository, SettingRepository>();
            builder.Services.AddScoped<IIntegrationRepository, IntegrationRepository>();
            builder.Services.AddScoped<IAuditLogRepository, AuditLogRepository>();
            builder.Services.AddScoped<ISentimentRepository, SentimentRepository>();
            
            // UnitOfWork
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            
            // Auth Services
            builder.Services.AddScoped<IAuthService, AuthService>();
            
            // Business Services
            builder.Services.AddScoped<IBusinessService, BusinessService>();
            
            // Ticket Services
            builder.Services.AddScoped<ITicketService, TicketService>();
            
            // Order Services
            builder.Services.AddScoped<IOrderService, OrderService>();
            
            // Feedback Services
            builder.Services.AddScoped<IFeedbackService, FeedbackService>();
            
            // MenuItem Services
            builder.Services.AddScoped<IMenuItemService, MenuItemService>();
            
            // MenuCategory Services
            builder.Services.AddScoped<IMenuCategoryService, MenuCategoryService>();
            
            // Message Services
            builder.Services.AddScoped<IMessageService, MessageService>();
            
            // Notification Services
            builder.Services.AddScoped<INotificationService, NotificationService>();
            
            // KnowledgeBase Services
            builder.Services.AddScoped<IKnowledgeBaseService, KnowledgeBaseService>();
            
            // Report Services
            builder.Services.AddScoped<IReportService, ReportService>();
            
            // Customer Services
            builder.Services.AddScoped<ICustomerService, CustomerService>();
            
            // Interaction Services
            builder.Services.AddScoped<IInteractionService, InteractionService>();
            
            // Subscription Services
            builder.Services.AddScoped<ISubscriptionService, SubscriptionService>();
            
            // PaymentTransaction Services
            builder.Services.AddScoped<IPaymentTransactionService, PaymentTransactionService>();
            
            // Setting Services
            builder.Services.AddScoped<ISettingService, SettingService>();
            
            // Integration Services
            builder.Services.AddScoped<IIntegrationService, IntegrationService>();
            
            // AuditLog Services
            builder.Services.AddScoped<IAuditLogService, AuditLogService>();
            
            // Sentiment Services
            builder.Services.AddScoped<ISentimentService, SentimentService>();
            
            // User Services
            builder.Services.AddScoped<IUserService, UserService>();

            // Business Analytics Services
            builder.Services.AddScoped<IBusinessAnalyticsService, BusinessAnalyticsService>();

            // Chatbot Services
            builder.Services.AddScoped<IChatbotService, ChatbotService>();
            builder.Services.AddScoped<IIntentDetectionService, IntentDetectionService>();
            builder.Services.AddScoped<ICustomerChatService, CustomerChatService>();

            // Dashboard Services
            builder.Services.AddScoped<IDashboardService, DashboardService>();

            // -------------------------
            // 8) FluentValidation Registration
            // -------------------------
            builder.Services.AddFluentValidationAutoValidation();
            builder.Services.AddValidatorsFromAssembly(typeof(AssemblyReference).Assembly);

            // -------------------------
            // 9) Custom Validation Response
            // -------------------------
            builder.Services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    var errors = context.ModelState
                        .Where(e => e.Value?.Errors.Count > 0)
                        .Select(e => new
                        {
                            Field = e.Key,
                            Error = e.Value?.Errors.FirstOrDefault()?.ErrorMessage ?? "Validation error"
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
            app.UseCors("AllowAll");
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
