using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Felicity.API.Data;

namespace Felicity.API
{
    public class LocalEntryPoint
    {
        public static void Main(string[] args)
        {
            //var host = BuildWebHost(args);

            //using (var scope = host.Services.CreateScope())
            //{
            //    var services = scope.ServiceProvider;

            //    var context = services.GetRequiredService<DataContext>();
            //    DbInitializer.Initialize(context);
            //}

            //host.Run();

            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();
    }
}
