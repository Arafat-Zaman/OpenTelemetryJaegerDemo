using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

var builder = WebApplication.CreateBuilder(args);

// Configure OpenTelemetry and Jaeger Exporter
builder.Services.AddOpenTelemetry()
    .WithTracing(tracerProviderBuilder =>
    {
        tracerProviderBuilder
            .SetResourceBuilder(ResourceBuilder.CreateDefault()
                .AddService("OpenTelemetryJaegerDemo"))
            .AddAspNetCoreInstrumentation()    // Instrument incoming HTTP requests
            .AddHttpClientInstrumentation()    // Instrument outgoing HTTP requests
            .AddJaegerExporter(jaegerOptions =>
            {
                jaegerOptions.AgentHost = "localhost";  // Jaeger host
                jaegerOptions.AgentPort = 6831;         // Jaeger agent port
            });
    });

// Add services to the container.
builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
