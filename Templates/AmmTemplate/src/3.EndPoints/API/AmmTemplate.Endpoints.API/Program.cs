using AmmFramework.Utilities.SerilogRegistration.Extensions;
using AmmFramework.Utilities.SerilogRegistration.Extensions.DependencyInjection;
using AmmTemplate.Endpoints.API.Extensions;

SerilogExtensions.RunWithSerilogExceptionHandling(() =>
{
    var builder = WebApplication.CreateBuilder(args);
    var app = builder.AddFrameworkSerilog(o =>
    {
        o.ApplicationName = builder.Configuration.GetValue<string>("ApplicationName");
        o.ServiceId = builder.Configuration.GetValue<string>("ServiceId");
        o.ServiceName = builder.Configuration.GetValue<string>("ServiceName");
        o.ServiceVersion = builder.Configuration.GetValue<string>("ServiceVersion");
    }).ConfigureServices().ConfigurePipeline();
    app.Run();
});
