using api.Logic;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

[ApiController]
[Route("[controller]")]
public class UtilController : ControllerBase
{
    private readonly UtilLogic _utilLogic;

    public UtilController(UtilLogic utilLogic)
    {
        _utilLogic = utilLogic;
    }
    
    [HttpGet("fill")]
    public async Task Fill()
    {
        await _utilLogic.Seed();
    }
}