using api.Logic;
using api.Models;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

[ApiController]
[Route("[controller]")]
public class PipelineController : ControllerBase
{
    private readonly PipelineLogic _pipelineLogic;

    public PipelineController(PipelineLogic pipelineLogic)
    {
        _pipelineLogic = pipelineLogic;
    }
    
    [HttpPost("version")]
    public async Task<IActionResult> UpsertVersion([FromBody] PipelineVersion version)
    {
        Console.WriteLine("what in the phucl");
        await _pipelineLogic.UpsertVersion(version);
        return Ok();
    }
}