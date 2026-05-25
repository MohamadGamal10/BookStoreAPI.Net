
using AutoMapper;
using BooKStore.Data;
using BooKStore.Interfaces;
using BooKStore.Middlewares;
using BooKStore.Models;
using BooKStore.Profiles;
using BooKStore.Repositories;
using BooKStore.Services;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace BooKStore
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Configure Entity Framework Core with SQL Server
            builder.Services.AddDbContext<Data.ApplicationDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Register repositories and unit of work
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            // AutoMapper
            builder.Services.AddAutoMapper(typeof(Mapping));
            //builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            // Services
            builder.Services.AddScoped<BookService>();
            builder.Services.AddScoped<AuthorService>();

            // Identity
            //builder.Services.AddIdentityApiEndpoints<ApplicationUser>()
            //    .AddEntityFrameworkStores<ApplicationDbContext>();

            builder.Services.AddIdentity<ApplicationUser>()
                .AddEntityFrameworkStores<ApplicationDbContext>();


            // Configure Serilog
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.Console()
                .WriteTo.File("Logs/log.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();

            builder.Host.UseSerilog();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }


            // To make all apis Auth auto
            //app.MapIdentityApi<ApplicationUser>();

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseMiddleware<GlobalExceptionMiddleware>();

            app.MapControllers();

            app.Run();
        }
    }
}
