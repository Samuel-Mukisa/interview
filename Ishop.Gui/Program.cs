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


            // Add services to the container.
            builder.Services.AddScoped<ICategoryManagementService, CategoryManagementService>();
            builder.Services.AddScoped<IAuthenticationService, AuthenticationManagementService>();
            builder.Services.AddScoped<IShoppingCartService, ShoppingCartManagementService>();
            builder.Services.AddHttpClient<IPaymentService, PaymentManagementService>();



            builder.Services.AddControllers();
            
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddInfrastructure();
           
           builder.Host.UseSerilog((ctx, lc) => lc
   .MinimumLevel.Debug()
   .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
   .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
   .Enrich.FromLogContext()
   .WriteTo.File("./logs/IshopLogs-log-.txt", rollingInterval: RollingInterval.Day, retainedFileCountLimit: 186, fileSizeLimitBytes: null, shared:true)
   .WriteTo.Console());
          
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
