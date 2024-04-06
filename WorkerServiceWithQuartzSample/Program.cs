using Quartz;
using Serilog;
using WorkerServiceWithQuartzSample;

var builder = Host.CreateDefaultBuilder(args)
    .UseWindowsService()
    .ConfigureServices((context, services) =>
    {
        services.AddQuartz(configurater =>
        {
            configurater
            .AddJob<MyJob>(jobConfigurater => jobConfigurater
                .WithIdentity("Myjob"))
            .AddTrigger(triggerConfigurater => triggerConfigurater
                .WithIdentity("MyTrigger")
                .ForJob("Myjob")
                .StartNow()
                .WithSimpleSchedule(scheduleBuilder => scheduleBuilder
                    .WithIntervalInSeconds(10)
                    .RepeatForever()));
        });

        services.AddQuartzHostedService(serviceOptions => { serviceOptions.WaitForJobsToComplete = true; });

        Log.Logger = new LoggerConfiguration()
                        .WriteTo.Console()
                        .CreateLogger();

        services.AddSerilog(Log.Logger);
    });

var host = builder.Build();
host.Run();
