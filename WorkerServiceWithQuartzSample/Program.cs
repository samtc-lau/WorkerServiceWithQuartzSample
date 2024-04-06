using Quartz;
using Serilog;
using WorkerServiceWithQuartzSample;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddWindowsService(options =>
{
    options.ServiceName = "MyService";
});

builder.Services.AddQuartz(configurater =>
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

builder.Services.AddQuartzHostedService(serviceOptions => { serviceOptions.WaitForJobsToComplete = true; });

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();
builder.Services.AddSerilog(Log.Logger);

var host = builder.Build();
host.Run();
