using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Diagnostics;
using VerifikationsKodProvider.Data.Context;

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .ConfigureServices(services =>
    {
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();
        services.AddDbContext<DataContext>(x => x.UseSqlServer(Environment.GetEnvironmentVariable("LearDataBase")));
    })
    .Build();

//using (var scope = host.Services.CreateScope())
//{
//    try
//    {
//        var context = scope.ServiceProvider.GetRequiredService<DataContext>();
//        var migration = context.Database.GetPendingMigrations();
//        if (migration != null && migration.Any())
//        {
//            context.Database.Migrate();
//        }
//    }
//    catch(Exception ex)
//    {
//        Debug.WriteLine($"Error : Program.cs :: {ex.Message}");
//    }
   
//}

host.Run();
