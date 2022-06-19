using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Logic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ApplicationController : ControllerBase
    {
        private readonly PipelineLogic _pipelineLogic;

        public ApplicationController(PipelineLogic pipelineLogic)
        {
            _pipelineLogic = pipelineLogic;
        }
        
        [HttpPost("{id}/start/{reference}")]
        public async Task<IActionResult> StartPipeline([FromRoute] string id, [FromRoute] string reference)
        {
            await _pipelineLogic.Start(id, reference);
            return Ok();
        }
    }
}
