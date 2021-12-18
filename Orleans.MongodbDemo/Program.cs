using Orleans.MongodbDemo;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;
using System.Net;

var hostBuilder = new HostBuilder();

var connectionString = "mongodb://192.168.40.128:27017/Huanbao";

var createShardKey = false;

hostBuilder.UseOrleans(builder =>
{
    builder.UseMongoDBClient(connectionString);

    builder.UseMongoDBClustering(options =>
    {
        options.DatabaseName = "Huanbao";
        options.CreateShardKeyForCosmos = createShardKey;
    });

    builder.Configure<ClusterOptions>(options =>
    {
        options.ClusterId = "huanbaocluster";
        options.ServiceId = "huanbaocluster";
    });

    builder.ConfigureEndpoints(IPAddress.Loopback, 11111, 30000);

    builder.AddMongoDBGrainStorage("MongoDBStore", options =>
    {
        options.DatabaseName = "Huanbao";
        options.CreateShardKeyForCosmos = createShardKey;
        options.ConfigureJsonSerializerSettings = settings =>
        {
            settings.NullValueHandling = NullValueHandling.Include;
            settings.ObjectCreationHandling = ObjectCreationHandling.Replace;
            settings.DefaultValueHandling = DefaultValueHandling.Populate;
        };
    });
    builder.AddSimpleMessageStreamProvider("OrleansTestStream", options =>
      {
          options.FireAndForgetDelivery = true;
          options.OptimizeForImmutableData = true;
          options.PubSubType = Orleans.Streams.StreamPubSubType.ExplicitGrainBasedOnly;
      });
    builder.AddMongoDBGrainStorage("PubSubStore", options =>
     {
         options.DatabaseName = "OrleansTestAppPubSubStore";
         options.CreateShardKeyForCosmos = createShardKey;

         options.ConfigureJsonSerializerSettings = settings =>
         {
             settings.NullValueHandling = NullValueHandling.Include;
             settings.ObjectCreationHandling = ObjectCreationHandling.Replace;
             settings.DefaultValueHandling = DefaultValueHandling.Populate;
         };

     });
    builder.ConfigureServices(services =>
    {
        services.Configure<ConsoleLifetimeOptions>(options =>
        {
            options.SuppressStatusMessages = true;
        });
    });

    builder.ConfigureLogging(logging => logging.AddConsole());
});

var host = hostBuilder.Build();

await host.StartAsync();

var factory = host.Services.GetRequiredService<IGrainFactory>();

var customerGrain = factory.GetGrain<ICustomerGrain>(4);

await customerGrain.Create("zxc", "13344556677");

Console.WriteLine(await customerGrain.Get());

await host.StopAsync();