
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PatientSimulator;

Console.WriteLine("New patient in, alive and breathing...");

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddHostedService<Patient>();

    })
    .Build();

Console.WriteLine("Staring recording vital signs...");
await host.RunAsync();