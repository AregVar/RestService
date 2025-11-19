using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using RestServiceFinal.Models;
using System;

namespace RestServiceFinal
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.WebHost.UseUrls("http://localhost:7213");


            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();
            builder.Services.AddDbContext<ServiceContext>(opt =>
               opt.UseInMemoryDatabase("UserList"));
            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<IServiceScopeFactory>();
                var InfoPuller = new InfoPull.InfoPuller(dbContext);
                InfoPuller.Start();
            }

                // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
