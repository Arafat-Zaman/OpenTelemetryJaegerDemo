using Microsoft.AspNetCore.Mvc;
using OpenTelemetry.Trace;
using OpenTelemetryJaegerDemo;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;
    private readonly Tracer _tracer;

    public WeatherForecastController(ILogger<WeatherForecastController> logger, TracerProvider tracerProvider)
    {
        _logger = logger;
        _tracer = tracerProvider.GetTracer("WeatherForecastControllerTracer");
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public IEnumerable<WeatherForecast> Get()
    {
        using (var span = _tracer.StartActiveSpan("WeatherForecast Get API"))
        {
            span.SetAttribute("custom-attribute", "custom-value");

            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}
