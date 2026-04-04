using OnlineWebManagement.Repositories.Services;
using OnlineWebManagement.Services.Services;

namespace OnlineWebManagement.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();
            builder.Services.AddOpenApi();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // ========= CORS NI YOQISH (Frontend ulanishi uchun) =========
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    policy =>
                    {
                        policy.AllowAnyOrigin()      // Har qanday manzildan
                              .AllowAnyMethod()      // Har qanday metod (GET, POST, ...)
                              .AllowAnyHeader();     // Har qanday header
                    });
            });

            // ========= QATLAMLARNI ULASH =========
            builder.Services.AddScoped<IStorageRepositorys, LocalStorageRepo>();
            builder.Services.AddScoped<IStorageService, LocalStorageService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            // ========= CORS NI ISHLATISH =========
            app.UseCors("AllowAll");

            // ========= STATIC FAYLLAR (FRONTEND) UCHUN =========
            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}