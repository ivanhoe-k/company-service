using Microsoft.AspNetCore.Mvc;

namespace CompanyService.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class EmptyController : ControllerBase
{
    public EmptyController()
    {
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public IActionResult Get()
    {
        return Ok("Hello!");
    }
}
