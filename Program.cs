using Serilog;
using SWGEmuModManagerApi.Models;

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilog(configureLogger: (hostBuilderContext, loggerConfiguration) => loggerConfiguration
        .WriteTo.Console()
        .ReadFrom.Configuration(hostBuilderContext.Configuration));

    builder.Services.AddControllers();

    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    builder.Services.AddLogging();

    WebApplication app = builder.Build();

    app.Lifetime.ApplicationStarted.Register(OnStarted);

    app.UseSerilogRequestLogging();

    app.UseHttpLogging();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}
catch (Exception e)
{
    Log.Fatal(e, "Unhandled Exception");
}
finally
{
    Log.Information("Shutdown complete");
    Log.CloseAndFlush();
}

void OnStarted()
{
    DatabaseConnection.Initialize().Wait();
}
