// chirper server

using Microsoft.Extensions.Hosting;
using Azure.Data.Tables;
using Microsoft.Extensions.Logging;
using Orleans.Configuration;

Console.Title = "Chirper Server";

var builder = Host.CreateDefaultBuilder(args)
    .UseOrleans(siloBuilder =>
    {
        siloBuilder
            .UseAzureStorageClustering(options =>
            {
                options.TableServiceClient = new TableServiceClient("UseDevelopmentStorage=true;");
                //options.TableName = "ClusterMembershipTable";
            })
            .Configure<ClusterOptions>(options =>
            {
                options.ClusterId = "chirper-cluster";
                options.ServiceId = "chirper-service";
            })
            .ConfigureEndpoints(siloPort: 11111, gatewayPort: 30000)
            .AddAzureTableGrainStorage("AccountState", options =>
            {
                options.TableServiceClient = new TableServiceClient("UseDevelopmentStorage=true;");
                options.TableName = "AccountState";
            })
            .UseDashboard(options =>
            {
                options.Port = 8080;
                //options.BasePath = "/dashboard";
            });
    })
    .ConfigureLogging(logging => logging.AddConsole());


await builder.RunConsoleAsync();
