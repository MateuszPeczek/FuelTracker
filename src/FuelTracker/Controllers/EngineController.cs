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
    [Route("[controller]")]
    public class EngineController : Controller
    {

        private readonly ICommandSender commandBus;
        private readonly IQuerySender queryBus;

        public EngineController(ICommandSender commandBus, IQuerySender queryBus)
        {
            this.commandBus = commandBus;
            this.queryBus = queryBus;
        }


        [HttpGet]
        [ProducesResponseType(typeof(PaginatedList<EngineDetails>), 200)]
        public IActionResult Get([FromQuery]int pageSize = 10,
                                 [FromQuery]int pageNo = 1,
                                 [FromQuery]EngineOrderColumn orderbyColumn = EngineOrderColumn.FuelType,
                                 [FromQuery]OrderDirection orderDirection = OrderDirection.Asc)
        {
            var query = new GetEnginesList(pageSize, pageNo, orderbyColumn, orderDirection);
            var result = queryBus.Get<PaginatedList<EngineDetails>>(query);

            return new JsonResult(result);
        }


        [HttpGet("{guid}")]
        public IActionResult Get(Guid guid)
        {
            var query = new GetSingleEngine(guid);
            var result = queryBus.Get<EngineDetails>(query);

            return new JsonResult(result);
        }

        [HttpPost]
        public IActionResult Post([FromBody]FuelType fuelType)
        {
            if (ModelState.IsValid)
            {
                var command = new AddEngine(fuelType);

                var commandResult = commandBus.Send(command);

                if (commandResult.Status == CommandStatus.Success)
                    return Get(command.Id);
            }

            return BadRequest(ModelState);
        }

        [HttpPut]
        public IActionResult Put([FromBody]PutEngine model)
        {
            if (ModelState.IsValid)
            {
                var command = new UpdateEngine(model.Id, model.Name, model.Power, model.Torque, model.Cylinders, model.Displacement);
                var commandResult = commandBus.Send(command);

                if (commandResult.Status == CommandStatus.Success)
                    return Get(command.Id);

            }

            return BadRequest();
        }

        [HttpDelete("{guid}")]
        public IActionResult Delete(Guid guid)
        {
            {
                var command = new DeleteEngine(guid);
                var commandResult = commandBus.Send(command);

                if (commandResult.Status == CommandStatus.Success)
                    return Ok();
                else
                    return BadRequest(ModelState);
            }
        }
    }
}
