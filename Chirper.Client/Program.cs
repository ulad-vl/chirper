using Chirper.Client;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Orleans.Configuration;
using Azure.Data.Tables;

Console.Title = "Chirper Client";

await new HostBuilder()
    .UseOrleansClient(clientBuilder =>
    {
        clientBuilder.UseAzureStorageClustering(options =>
                    {
                        options.TableServiceClient = new TableServiceClient("UseDevelopmentStorage=true;");
                    })
                    .Configure<ClusterOptions>(options =>
                    {
                        options.ClusterId = "chirper-cluster";
                        options.ServiceId = "chirper-service";
                    });
    })
    .ConfigureServices(
        services => services
            .AddSingleton<IHostedService, ShellHostedService>()
            .Configure<ConsoleLifetimeOptions>(sp => sp.SuppressStatusMessages = true))
    .ConfigureLogging(builder => builder.AddDebug())
    .RunConsoleAsync();
