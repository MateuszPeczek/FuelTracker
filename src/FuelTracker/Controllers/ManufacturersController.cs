using System;
using Microsoft.AspNetCore.Mvc;
using Common.Interfaces;
using Queries.ManufacturerQueries;
using Common.Paging;
using Common.Ordering.Shared;
using Commands.ManudaturerCommands;
using Common.Enums;
using Commands.ManufacturerCommands;
using Queries.ModelQueries;
using FuelTracker.ApiModels.ManufacturerApiModels;
using Commands.ModelCommands;
using Infrastructure.CommunicationModels;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FuelTracker.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/manufacturers")]
    public class ManufacturersController : Controller
    {
        private readonly ICommandSender commandBus;
        private readonly IQuerySender queryBus;

        public ManufacturersController(ICommandSender commandBus, IQuerySender queryBus)
        {
            this.commandBus = commandBus;
            this.queryBus = queryBus;
        }

        // GET: api/manufacturers
        [HttpGet("")]
        [ProducesResponseType(typeof(PaginatedList<ManufacturerDetails>), 200)]
        public IActionResult GetManufacturers([FromQuery]int pageSize = 10,
                                              [FromQuery]int pageNo = 1,
                                              [FromQuery]OrderDirection orderDirection = OrderDirection.Asc)
        {
            var query = new GetManufacturersList(pageSize, pageNo, orderDirection);
            var result = queryBus.Get<PaginatedList<ManufacturerDetails>>(query);

            return Ok(result);
        }

        private ManufacturerDetails GetManufacturerDetails(Guid manufacturerId)
        {
            var query = new GetSingleManufacturer(manufacturerId);
            var result = queryBus.Get<ManufacturerDetails>(query);

            return result.Data;
        }

        // GET api/manufacturers/{manufacturerId}
        [HttpGet("{manufacturerId}", Name = "GetManufacturer")]
        public IActionResult GetManufacturer(Guid manufacturerId)
        {
            var result = GetManufacturerDetails(manufacturerId);

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        // POST api/manufacturers
        [HttpPost("")]
        public IActionResult PostManufacturer([FromBody]PostManufacturer model)
        {
            if (ModelState.IsValid)
            {
                var command = new AddManufacturer(model.Name);
                var commandResult = commandBus.Send(command);

                if (commandResult.Status == ActionStatus.Success)
                {
                    var result = GetManufacturerDetails(command.Id);

                    return CreatedAtRoute(
                        "GetManufacturer",
                        new { manufacturerId = command.Id },
                        result
                        );
                }
                else
                {
                    return StatusCode(500, commandResult.ExceptionMessage);
                }
            }

            return BadRequest(ModelState);
        }

        // PUT api/manufacturers/{manufacturerId}
        [HttpPut("{manufacturerId}")]
        public IActionResult PutManufacturer(Guid manufacturerId, [FromBody]PutManufactuer model)
        {
            if (ModelState.IsValid)
            {
                var command = new UpdateManufacturer(manufacturerId, model.Name);
                var commandResult = commandBus.Send(command);

                if (commandResult.Status == ActionStatus.Success)
                    return GetManufacturer(command.Id);
                else
                    return new JsonResult(commandResult.ExceptionMessage);
            }

            return BadRequest(ModelState);
        }

        // DELETE api/manufacturers/{manufacturerId}
        [HttpDelete("{manufacturerId}")]
        public IActionResult DeleteManufacturer(Guid manufacturerId)
        {
            var command = new DeleteManufacturer(manufacturerId);
            var commandResult = commandBus.Send(command);

            if (commandResult.Status == ActionStatus.Success)
                return Ok();
            else
                return new JsonResult(commandResult.ExceptionMessage);
        }

        // GET: api/manufacturers/{manufacturerId}/models
        [HttpGet("{manufacturerId}/models")]
        [ProducesResponseType(typeof(PaginatedList<ModelDetails>), 200)]
        public IActionResult GetModels(Guid manufacturerId, [FromQuery]int pageSize = 10,
                                                            [FromQuery]int pageNo = 1,
                                                            [FromQuery]OrderDirection orderDirection = OrderDirection.Asc)
        {
            var query = new GetModelsList(manufacturerId, pageSize, pageNo, orderDirection);
            var result = queryBus.Get<PaginatedList<ModelDetails>>(query);

            return Ok(result);
        }

        private ModelDetails GetModelDetails(Guid manufacturerId, Guid modelId)
        {
            var query = new GetSingleModel(manufacturerId, modelId);
            var result = queryBus.Get<ModelDetails>(query);

            return result.Data;
        }

        // GET: api/manufacturers/{manufacturerId}/models
        [HttpGet("{manufacturerId}/models/{modelId}", Name = "GetModel")]
        public IActionResult GetModel(Guid manufacturerId, Guid modelId)
        {
            var result = GetModelDetails(manufacturerId, modelId);
            
            if (result == null)
                return NotFound();

            return Ok(result);
        }

        // POST api/manufacturers/{manufacturerId}/models
        [HttpPost("{manufacturerId}/models")]
        public IActionResult PostModel(Guid manufacturerId, [FromBody]PostModelName model)
        {
            if (ModelState.IsValid)
            {
                var command = new AddModel(manufacturerId, model.ModelName);
                var commandResult = commandBus.Send(command);

                if (commandResult.Status == ActionStatus.Success)
                {
                    var result = GetModelDetails(manufacturerId, command.Id);

                    return CreatedAtRoute(
                        "GetModel",
                        new { manufacturerId = manufacturerId, modelId = command.Id },
                        result
                        );
                }
                else
                {
                    return StatusCode(500, commandResult.ExceptionMessage);
                }
            }

            return BadRequest(ModelState);
        }

        // PUT api/manufacturers/{manufacturerId}/models/{modelId}
        [HttpPut("{manufacturerId}/models{modelId}")]
        public IActionResult PutModel(Guid manufactuerId, Guid modelId, [FromBody]PutModelName model)
        {
            if (ModelState.IsValid)
            {
                var command = new UpdateModel(manufactuerId, modelId, model.ModelName);
                var commandResult = commandBus.Send(command);

                if (commandResult.Status == ActionStatus.Success)
                    return GetModel(manufactuerId, command.Id);
                else
                    return new JsonResult(commandResult.ExceptionMessage);
            }

            return BadRequest(ModelState);
        }

        // DELETE api/manufacturers/{manufacturerId}/models/{modelId}
        [HttpDelete("{manufacturerId}/models/{modelId}")]
        public IActionResult DeleteModel(Guid manufactuerId, Guid modelId)
        {
            var command = new DeleteModel(manufactuerId, modelId);
            var commandResult = commandBus.Send(command);

            if (commandResult.Status == ActionStatus.Success)
                return Ok();
            else
                return new JsonResult(commandResult.ExceptionMessage);
        }
    }
}
