using Microsoft.AspNetCore.Mvc;

using TaxiSim.Api.Features.WeatherForecast.Models;
using TaxiSim.Api.SharedContext;

namespace TaxiSim.Api.Features.WeatherForecast;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ApiControllerBase
{
    private static readonly string[] Summaries =
    [
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    ];

    [HttpGet]
    public IActionResult Get()
    {
        var result = Enumerable.Range(1, 5).Select(index => new WeatherForecastDto
        {
            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToArray();

        return new ObjectResult(result) { StatusCode = 200 };
    }
}
