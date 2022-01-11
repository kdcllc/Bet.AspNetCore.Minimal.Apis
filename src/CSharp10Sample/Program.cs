var hostBuilder = Host.CreateDefaultBuilder(args)
               .ConfigureServices((hostContext, services) =>
               {
                   services.AddHostedService<Worker>();
               });

var host = hostBuilder.Build();

await host.RunAsync();
