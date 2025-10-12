namespace Microsoft.eShopOnDapr.BlazorClient.Host.Controllers;

[Route("[controller]")]
[ApiController]
public sealed class SettingsController : Controller
{
    [HttpGet]
    public IActionResult GetSettings([FromServices] IOptions<Settings> settings)
    {
        ArgumentNullException.ThrowIfNull(settings);

        return Ok(settings.Value);
    }
}
