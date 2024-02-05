using Ishop.Application.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.OpenApi.Models;
using Ishop.Application.Services;
using Ishop.Infrastructure.IoC;
using Ishop.Infrastructure.ServiceImplementations;
using MySqlConnector;
using Serilog;
using Serilog.Events;

namespace Ishop.Gui
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddTransient(x =>
                new MySqlConnection(builder.Configuration.GetConnectionString("MySqlConnection")));

            builder.Services.AddScoped<ICategoryManagementService, CategoryManagementService>();
            builder.Services.AddScoped<IAuthenticationService, AuthenticationManagementService>();
            builder.Services.AddScoped<IShoppingCartService, ShoppingCartManagementService>();
            builder.Services.AddHttpClient<IPaymentService, PaymentManagementService>();
            builder.Services.AddScoped<IProductManagementService, ProductManagementService>();
            builder.Services.AddScoped<IOrderManagementService, OrderManagementService>();
            builder.Services.AddScoped<IPromotionManagement, PromotionManagementService>();
            builder.Services.AddScoped<IDeliveryAgentManagement, DeliveryAgentManagementService>();
            builder.Services.AddScoped<IRetailerAuthenticationService, RetailerAuthenticationManagementService>();

            
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddInfrastructure();

            builder.Host.UseSerilog((ctx, lc) => lc
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
                .Enrich.FromLogContext()
                .WriteTo.File("./logs/IshopLogs-log-.txt", rollingInterval: RollingInterval.Day, retainedFileCountLimit: 186, fileSizeLimitBytes: null, shared: true)
                .WriteTo.Console());

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            var httpsPort = 8080; // Change this to your actual HTTPS port

            app.UseHttpsRedirection(); // No parameters here

            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}
