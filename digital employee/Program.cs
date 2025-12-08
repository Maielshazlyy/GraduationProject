using Microsoft.EntityFrameworkCore;
using FluentValidation;
using Service_layer;
using DAL.Context;
using Microsoft.AspNetCore.Mvc;

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
            builder.Services.AddOpenApi();

            // -------------------------
            // 3) Database Connection
            // -------------------------
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // -------------------------
            // 4) FluentValidation Registration
            // -------------------------
            builder.Services.AddValidatorsFromAssembly(typeof(AssemblyReference).Assembly);

            // -------------------------
            // 5) Custom Validation Response
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
            // 6) Swagger in Development
            // -------------------------
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            // -------------------------
            // 7) Middlewares
            // -------------------------
            app.UseHttpsRedirection();
            app.UseAuthorization();

            // -------------------------
            // 8) Controllers Endpoint
            // -------------------------
            app.MapControllers();

            app.Run();
        }
    }
}
