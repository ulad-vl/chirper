// chirper server

using Microsoft.Extensions.Hosting;

Console.Title = "Chirper Server";

var builder = Host.CreateDefaultBuilder(args)
    .UseOrleans(
        builder => builder
            .UseLocalhostClustering()
            .AddMemoryGrainStorage("AccountState")
            .UseDashboard(options =>
            {
                options.Port = 8080;
                //options.BasePath = "/dashboard";
            })
            );

await builder.RunConsoleAsync();
