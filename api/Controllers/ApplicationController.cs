using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Logic;
using data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ApplicationController : ControllerBase
    {
        private readonly PipelineLogic _pipelineLogic;
        private readonly ApplicationLogic _applicationLogic;

        public ApplicationController(PipelineLogic pipelineLogic, ApplicationLogic applicationLogic)
        {
            _pipelineLogic = pipelineLogic;
            _applicationLogic = applicationLogic;
        }
        
        [HttpPost("{id}/start/{reference}")]
        public async Task<IActionResult> StartPipeline([FromRoute] string id, [FromRoute] string reference)
        {
            await _pipelineLogic.Start(id, reference);
            return Ok();
        }

        [HttpGet]
        public async Task<List<Application>> GetAll()
        {
            return await _applicationLogic.GetAll();
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Application application)
        {
            await _applicationLogic.Create(application);
            return Ok();
        }
        
        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromBody] Application application, [FromRoute] Guid id)
        {
            await _applicationLogic.Update(application, id);
            return Ok();
        }

        // GET all
        // GET one
        // POST
        // PUT
        // DELETE
    }
}
