using data;
using data.DataModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;
    private readonly DeployerContext _deployerContext;

    public WeatherForecastController(ILogger<WeatherForecastController> logger, DeployerContext deployerContext)
    {
        _logger = logger;
        _deployerContext = deployerContext;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public IEnumerable<PipelineDTO> Get()
    {
        return _deployerContext.Pipelines.Include("Versions");
    }
}
