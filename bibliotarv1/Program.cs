
using bibliotar.Context;
using bibliotar.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace bibliotar
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer("(local);Database=BibliotarDb;Trusted_Connection=True;TrustServerCertificate=True;"); //Mati\SQLEXPRESS (local)
            });

            builder.Services.AddScoped<IBibliotarService, BookService>();
            
            
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "BibliotarApp API", Version = "v1" });
            });



            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "BibliotarApp API v1"));

            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
