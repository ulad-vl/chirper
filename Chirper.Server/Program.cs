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
            .UseKubernetesHosting()
            .UseAzureStorageClustering(options =>
            {
                options.TableServiceClient = new TableServiceClient("AccountName=devstoreaccount1;AccountKey=Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw==;DefaultEndpointsProtocol=http;BlobEndpoint=http://192.168.0.16:10000/devstoreaccount1;QueueEndpoint=http://192.168.0.16:10001/devstoreaccount1;TableEndpoint=http://192.168.0.16:10002/devstoreaccount1;");
                options.TableName = "ClusterMembershipTable";
            })
            .Configure<ClusterOptions>(options =>
            {
                options.ClusterId = "chirper-cluster";
                options.ServiceId = "chirper-service";
            })
            .ConfigureEndpoints(siloPort: 11111, gatewayPort: 30000, listenOnAnyHostAddress: true)
            .AddAzureTableGrainStorage("AccountState", options =>
            {
                options.TableServiceClient = new TableServiceClient("AccountName=devstoreaccount1;AccountKey=Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw==;DefaultEndpointsProtocol=http;BlobEndpoint=http://192.168.0.16:10000/devstoreaccount1;QueueEndpoint=http://192.168.0.16:10001/devstoreaccount1;TableEndpoint=http://192.168.0.16:10002/devstoreaccount1;");
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
