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
                        options.TableServiceClient = new TableServiceClient("AccountName=devstoreaccount1;AccountKey=Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw==;DefaultEndpointsProtocol=http;BlobEndpoint=http://192.168.0.16:10000/devstoreaccount1;QueueEndpoint=http://192.168.0.16:10001/devstoreaccount1;TableEndpoint=http://192.168.0.16:10002/devstoreaccount1;");
                        options.TableName = "ClusterMembershipTable";
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
