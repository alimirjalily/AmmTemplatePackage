using AmmFramework.EndPoints.Web.Extensions.DependencyInjection;
using AmmFramework.EndPoints.Web.Extensions.ModelBinding;
using AmmFramework.Extensions.DependencyInjection;
using AmmFramework.Extensions.Events.PollingPublisher.Dal.Dapper.Extensions.DependencyInjection;
using AmmFramework.Extensions.MessageBus.MessageInbox.Dal.Dapper.Extensions.DependencyInjection;
using AmmFramework.Extensions.ObjectMappers.AutoMapper.Extensions.DependencyInjection;
using AmmFramework.Extensions.Serializers.Microsoft.Extensions.DependencyInjection;
using AmmFramework.Extensions.Translations.Parrot.Extensions.DependencyInjection;
using AmmFramework.Extensions.UsersManagement.Extensions.DependencyInjection;
using AmmFramework.Infra.Data.Sql.Commands.Interceptors;
using AmmFramework.Utilities.ScalarRegistration.Extensions.DependencyInjection;
using AmmTemplate.Infra.Data.Sql.Commands.Common;
using AmmTemplate.Infra.Data.Sql.Queries.Common;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace AmmTemplate.Endpoints.API.Extensions;

public static class HostingExtensions
{
    public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
    {
        IConfiguration configuration = builder.Configuration;

        
        builder.Services.AddFrameworkApiCore("Amm", "AmmTemplate");

        //microsoft
        builder.Services.AddEndpointsApiExplorer();

        //Framework
        builder.Services.AddFrameworkWebUserInfoService(configuration, "WebUserInfo", true);

        //Framework
        builder.Services.AddFrameworkParrotTranslator(configuration, "ParrotTranslator");

        //Framework
        //builder.Services.AddSoftwarePartDetector(configuration, "SoftwarePart");

        //Framework
        builder.Services.AddNonValidatingValidator();

        //Framework
        builder.Services.AddFrameworkMicrosoftSerializer();

        //Framework
        builder.Services.AddFrameworkAutoMapperProfiles(configuration, "AutoMapper");

        //Framework
        builder.Services.AddFrameworkInMemoryCaching();
        //builder.Services.AddFrameworkSqlDistributedCache(configuration, "SqlDistributedCache");

        //CommandDbContext
        builder.Services.AddDbContext<DbContextNameCommandDbContext>(c => c.UseSqlServer(configuration.GetConnectionString("CommandDb_ConnectionString"))
            .AddInterceptors(new SetPersianYeKeInterceptor(), new AddAuditDataInterceptor()));

        //QueryDbContext
        builder.Services.AddDbContext<DbContextNameQueryDbContext>(c => c.UseSqlServer(configuration.GetConnectionString("QueryDb_ConnectionString")));

        //PollingPublisher
        builder.Services.AddFrameworkPollingPublisherDalSql(configuration, "PollingPublisherSqlStore");
        //builder.Services.AddFrameworkPollingPublisher(configuration, "PollingPublisher");

        //MessageInbox
        builder.Services.AddFrameworkMessageInboxDalSql(configuration, "MessageInboxSqlStore");
        //builder.Services.AddFrameworkMessageInbox(configuration, "MessageInbox");

        //builder.Services.AddFrameworkRabbitMqMessageBus(configuration, "RabbitMq");

        //builder.Services.AddFrameworkTraceJeager(configuration, "OpenTeletmetry");

        //API Documentation
        builder.Services.AddFrameworkScalar(builder.Configuration, "Scalar");




        return builder.Build();
    }

    public static WebApplication ConfigurePipeline(this WebApplication app)
    {
        //Framework
        app.UseFrameworkApiExceptionHandler();

        //Serilog
        app.UseSerilogRequestLogging();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            app.UseFrameworkScalar();
        }

        app.UseStatusCodePages();

        app.UseCors(delegate (CorsPolicyBuilder builder)
        {
            builder.AllowAnyOrigin();
            builder.AllowAnyHeader();
            builder.AllowAnyMethod();
        });

        app.UseHttpsRedirection();

        //app.Services.ReceiveEventFromRabbitMqMessageBus(new KeyValuePair<string, string>("MiniAggregateName", "AggregateNameCreated"));

        //var useIdentityServer = app.UseIdentityServer("OAuth");

        var controllerBuilder = app.MapControllers();

        //if (useIdentityServer)
        //    controllerBuilder.RequireAuthorization();

        //app.Services.GetService<SoftwarePartDetectorService>()?.Run();

        return app;
    }
}