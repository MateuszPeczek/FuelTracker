using Commands.EngineCommands;
using Common.Interfaces;
using Common.Ordering.Engine;
using Common.Ordering.Shared;
using Common.Paging;
using FuelTracker.ApiModels.EngineApiModels;
using FuelTracker.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Queries.EngineQueries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace FuelTracker.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/engines")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class EnginesController : Controller
    {

        private readonly ICommandSender commandBus;
        private readonly IQuerySender queryBus;

        public EnginesController(ICommandSender commandBus, IQuerySender queryBus)
        {
            this.commandBus = commandBus;
            this.queryBus = queryBus;

        }

        private EngineDetails GetEngineDetails(Guid engineId)
        {
            var query = new GetSingleEngine(engineId);
            var result = queryBus.InvokeQuery<EngineDetails>(query);

            return result.Data;
        }

        private IEnumerable<EngineDetails> GetEngineDetails(IEnumerable<Guid> enginesIds)
        {
            var query = new GetEnginesByIds(enginesIds);
            var result = queryBus.InvokeQuery<IEnumerable<EngineDetails>>(query);

            return result.Data;
        }

        //GET: api/engines/{engineId}
        [HttpGet("{engineId}", Name = "GetEngine")]
        public IActionResult GetEngine([ModelBinder(BinderType = typeof(CollectionModelBinder))]Guid engineId)
        {
            var result = GetEngineDetails(engineId);
            return Ok(result);
        }

        //GET: api/engines
        [HttpGet()]
        [ProducesResponseType(typeof(PaginatedList<EngineDetails>), 200)]
        public IActionResult GetEngines([FromQuery]int pageSize = 10,
                                        [FromQuery]int pageNo = 1,
                                        [FromQuery]EngineOrderColumn orderbyColumn = EngineOrderColumn.FuelType,
                                        [FromQuery]OrderDirection orderDirection = OrderDirection.Asc)
        {
            var query = new GetEnginesList(pageSize, pageNo, orderbyColumn, orderDirection);
            var result = queryBus.InvokeQuery<PaginatedList<EngineDetails>>(query);

            return Ok(result.Data);
        }

        //GET: api/engines/(engineId, engineId, ...)
        [HttpGet("({ids})", Name = "GetEnginesByIds")]
        [ProducesResponseType(typeof(IEnumerable<EngineDetails>), 200)]
        public IActionResult GetEnginesByIds([ModelBinder(BinderType = typeof(CollectionModelBinder))] IEnumerable<Guid> ids)
        {
            var query = new GetEnginesByIds(ids);
            var result = queryBus.InvokeQuery<IEnumerable<EngineDetails>>(query);

            return Ok(result.Data);
        }

        //POST: api/engines
        [HttpPost("")]
        public IActionResult PostEngine([FromBody]PostEngine model)
        {
            if (ModelState.IsValid)
            {
                var command = new AddEngine(model.Name, model.Power, model.Torque, model.Cylinders, model.Displacement, model.FuelType);
                commandBus.AddCommand(command);
                commandBus.InvokeCommandsQueue();

                var result = GetEngineDetails(command.Id);
                return CreatedAtRoute(
                    "GetEngine",
                    new { engineId = command.Id },
                    result
                    );
            }

            return BadRequest(ModelState);
        }

        //POST: api/engines/collection
        [HttpPost("collection")]
        public IActionResult PostEnginesCollection([FromBody]IEnumerable<PostEngine> modelsCollection)
        {
            if (ModelState.IsValid)
            {
                if (!modelsCollection.Any())
                    return BadRequest("Empty collection");

                foreach (var model in modelsCollection)
                {
                    var command = new AddEngine(model.Name, model.Power, model.Torque, model.Cylinders, model.Displacement, model.FuelType);
                    commandBus.AddCommand(command);
                }

                commandBus.InvokeCommandsQueue();

                var result = GetEngineDetails(commandBus.CommandIds);
                return CreatedAtRoute(
                    "GetEnginesByIds",
                    new { ids = string.Join(",", commandBus.CommandIds) },
                    result
                    );
            }

            return BadRequest(ModelState);
        }

        //PUT: api/engines/{engineId}
        [HttpPut("{engineId}")]
        public IActionResult PutEngine(Guid engineId, [FromBody]PutEngine model)
        {
            if (ModelState.IsValid)
            {
                var command = new UpdateEngine(engineId, model.Name, model.Power, model.Torque, model.Cylinders, model.Displacement);
                commandBus.AddCommand(command);
                commandBus.InvokeCommandsQueue();

                return GetEngine(command.Id);
            }

            return BadRequest(ModelState);
        }

        //DELETE: api/engines/{engineId}}
        [HttpDelete("{engineId}")]
        public IActionResult DeleteEngine(Guid engineId)
        {
            var command = new DeleteEngine(engineId);
            commandBus.AddCommand(command);
            commandBus.InvokeCommandsQueue();

            return Ok();
        }
    }
}
