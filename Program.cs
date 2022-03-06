using Serilog;
using Serilog.Sinks.SystemConsole.Themes;
using SWGEmuModManagerApi.Models;

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilog(configureLogger: (hostBuilderContext, loggerConfiguration) => loggerConfiguration
        .WriteTo.Console(theme: AnsiConsoleTheme.Literate)
        .ReadFrom.Configuration(hostBuilderContext.Configuration));

    builder.Services.AddControllers();

    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    builder.Services.AddLogging();

    WebApplication app = builder.Build();

    app.Lifetime.ApplicationStarted.Register(OnStarted);

    app.UseSerilogRequestLogging(options =>
    {
        options.MessageTemplate =
            "{RemoteIpAddress} {RequestScheme} {RequestHost} {RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed:0.0000} ms";
        options.EnrichDiagnosticContext = (
            diagnosticContext,
            httpContext) =>
        {
            diagnosticContext.Set("RequestHost", httpContext.Request.Host.Value);
            diagnosticContext.Set("RequestScheme", httpContext.Request.Scheme);
            diagnosticContext.Set("RemoteIpAddress", httpContext.Connection.RemoteIpAddress);
        };
    });

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
