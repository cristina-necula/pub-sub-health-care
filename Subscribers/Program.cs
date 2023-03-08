using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using SubscriberApp;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddSingleton(serviceProvider =>
        {
            return new ConnectionFactory { HostName = "localhost" };
        });
        services.AddHostedService<Subscriber>();
    })
    .Build();

Console.WriteLine("Staring subscriber app...");
await host.RunAsync();
