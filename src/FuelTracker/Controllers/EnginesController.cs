using System;
using Microsoft.AspNetCore.Mvc;
using Common.Paging;
using FuelTracker.ApiModels.EngineApiModels;
using Common.Interfaces;
using Commands.EngineCommands;
using Common.Enums;
using Domain.VehicleDomain;
using Queries.EngineQueries;
using Common.Ordering.Engine;
using Common.Ordering.Shared;

namespace FuelTracker.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/engines")]
    public class EnginesController : Controller
    {

        private readonly ICommandSender commandBus;
        private readonly IQuerySender queryBus;

        public EnginesController(ICommandSender commandBus, IQuerySender queryBus)
        {
            this.commandBus = commandBus;
            this.queryBus = queryBus;
        }


        //GET: api/engines
        [HttpGet("")]
        [ProducesResponseType(typeof(PaginatedList<EngineDetails>), 200)]
        public IActionResult Get([FromQuery]int pageSize = 10,
                                 [FromQuery]int pageNo = 1,
                                 [FromQuery]EngineOrderColumn orderbyColumn = EngineOrderColumn.FuelType,
                                 [FromQuery]OrderDirection orderDirection = OrderDirection.Asc)
        {
            var query = new GetEnginesList(pageSize, pageNo, orderbyColumn, orderDirection);
            var result = queryBus.Get<PaginatedList<EngineDetails>>(query);

            return Ok(result);
        }

        //GET: api/engines/{engineId}
        [HttpGet("{engineId}")]
        public IActionResult Get(Guid engineId)
        {
            var query = new GetSingleEngine(engineId);
            var result = queryBus.Get<EngineDetails>(query);

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        //POST: api/engines
        [HttpPost("")]
        public IActionResult Post([FromBody]PostEngine model)
        {
            if (ModelState.IsValid)
            {
                var command = new AddEngine(model.FuelType);

                var commandResult = commandBus.Send(command);

                if (commandResult.Status == CommandStatus.Success)
                    return Get(command.Id);
            }

            return BadRequest(ModelState);
        }

        //PUT: api/engines/{engineId}
        [HttpPut("{engineId}")]
        public IActionResult Put(Guid engineId, [FromBody]PutEngine model)
        {
            if (ModelState.IsValid)
            {
                var command = new UpdateEngine(engineId, model.Name, model.Power, model.Torque, model.Cylinders, model.Displacement);
                var commandResult = commandBus.Send(command);

                if (commandResult.Status == CommandStatus.Success)
                    return Get(command.Id);

            }

            return BadRequest(ModelState);
        }

        //DELETE: api/engines/{engineId}}
        [HttpDelete("{engineId}")]
        public IActionResult Delete(Guid engineId)
        {
            {
                var command = new DeleteEngine(engineId);
                var commandResult = commandBus.Send(command);

                if (commandResult.Status == CommandStatus.Success)
                    return Ok();
                else
                    return BadRequest(ModelState);
            }
        }
    }
}
