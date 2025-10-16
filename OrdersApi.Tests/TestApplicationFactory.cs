using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OrdersApi.Infrastructure.Data;

namespace OrdersApi.Tests
{
    public class IntegrationTestsApplication : WebApplicationFactory<Program> 
    {
        private string _databasePath;
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            _databasePath = Path.Combine(Path.GetTempPath(), $"integrationTests_{Guid.NewGuid()}.db");

            builder.ConfigureServices(services =>
            {                

                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<OrdersApiDbContext>));

                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                services.AddDbContext<OrdersApiDbContext>(options =>
                {
                    options.UseSqlite($"DataSource={_databasePath}");
                });         
            });
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);           
            if (disposing && File.Exists(_databasePath))
            {
                try
                {
                    File.Delete(_databasePath);
                }
                catch
                {                    
                }
            }
        }
    }
}
