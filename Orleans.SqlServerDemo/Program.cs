using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;
using Orleans.SqlServerDemo;
using System.Net;

// define the cluster configuration
var host = Host.CreateDefaultBuilder()
    .UseOrleans((builder) =>
    {
        builder.UseLocalhostClustering()
            //.AddMemoryGrainStorageAsDefault()
            .Configure<ClusterOptions>(options =>
            {
                options.ClusterId = "Orleans";
                options.ServiceId = "Orleans";
            })
            .Configure<EndpointOptions>(options => options.AdvertisedIPAddress = IPAddress.Loopback)
            .ConfigureApplicationParts(parts =>
                parts.AddApplicationPart(typeof(ICustomerGrain).Assembly).WithReferences());

        var connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Orleans;Integrated Security=True;Pooling=False;Max Pool Size=200;MultipleActiveResultSets=True";

        builder.AddAdoNetGrainStorageAsDefault(options =>
        {
            options.ConnectionString = connectionString;
            options.UseJsonFormat = true;
        });
    }
    )
    .ConfigureServices(services =>
    {
        services.Configure<ConsoleLifetimeOptions>(options =>
        {
            options.SuppressStatusMessages = true;
        });
    })
    .ConfigureLogging(builder => { builder.AddConsole(); })
    .Build();

await host.StartAsync();

var factory = host.Services.GetRequiredService<IGrainFactory>();

var customerGrain = factory.GetGrain<ICustomerGrain>("1");

await customerGrain.Create("zxc", "13344556677");

Console.WriteLine(await customerGrain.Get());

await host.StopAsync();
