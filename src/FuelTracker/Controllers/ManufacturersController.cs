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

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FuelTracker.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/")]
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
        [HttpGet("manufacturers")]
        [ProducesResponseType(typeof(PaginatedList<ManufacturerDetails>), 200)]
        public IActionResult GetManufacturers([FromQuery]int pageSize = 10,
                                [FromQuery]int pageNo = 1,
                                [FromQuery]OrderDirection orderDirection = OrderDirection.Asc)
        {
            var query = new GetManufacturersList(pageSize, pageNo, orderDirection);
            var result = queryBus.Get<PaginatedList<ManufacturerDetails>>(query);

            return new JsonResult(result);
        }

        // GET api/manufacturers/{manufacturerId}
        [HttpGet("manufacturers/{manufacturerId}")]
        public IActionResult GetManufacturers(Guid manufacturerId)
        {
            var query = new GetSingleManufacturer(manufacturerId);
            var result = queryBus.Get<ManufacturerDetails>(query);

            return new JsonResult(result);
        }

        // POST api/manufacturers
        [HttpPost("manufacturers/")]
        public IActionResult PostManufacturers([FromBody]PostManufacturer model)
        {
            if (ModelState.IsValid)
            {
                var command = new AddManufacturer(model.Name);
                var commandResult = commandBus.Send(command);

                if (commandResult.Status == CommandStatus.Success)
                    return GetManufacturers(command.ModelId);
                else
                    return new JsonResult(commandResult.ExceptionMessage);
            }

            return BadRequest(ModelState);
        }

        // PUT api/manufacturers/{manufacturerId}
        [HttpPut("manufacturers/{manufacturerId}")]
        public IActionResult PutManufacturers(Guid manufacturerId, [FromBody]PutManufactuer model)
        {
            if (ModelState.IsValid)
            {
                var command = new UpdateManufacturer(manufacturerId, model.Name);
                var commandResult = commandBus.Send(command);

                if (commandResult.Status == CommandStatus.Success)
                    return GetManufacturers(command.ModelId);
                else
                    return new JsonResult(commandResult.ExceptionMessage);
            }

            return BadRequest(ModelState);
        }

        // DELETE api/manufacturers/{manufacturerId}
        [HttpDelete("manufacturers/{manufacturerId}")]
        public IActionResult DeleteManufacturers(Guid manufacturerId)
        {
            var command = new DeleteManufacturer(manufacturerId);
            var commandResult = commandBus.Send(command);

            if (commandResult.Status == CommandStatus.Success)
                return Ok();
            else
                return new JsonResult(commandResult.ExceptionMessage);
        }

        // GET: api/manufacturers/{manufacturerId}/models
        [HttpGet("manufacturers/{manufacturerId}/models")]
        [ProducesResponseType(typeof(PaginatedList<ModelDetails>), 200)]
        public IActionResult GetModels(Guid manufacturerId, [FromQuery]int pageSize = 10,
                                                            [FromQuery]int pageNo = 1,
                                                            [FromQuery]OrderDirection orderDirection = OrderDirection.Asc)
        {
            var query = new GetModelsList(manufacturerId, pageSize, pageNo, orderDirection);
            var result = queryBus.Get<PaginatedList<ModelDetails>>(query);

            return new JsonResult(result);
        }

        // GET: api/manufacturers/{manufacturerId}/models
        [HttpGet("manufacturers/{manufacturerId}/models/{modelId}")]
        public IActionResult GetModels(Guid manufacturerId, Guid modelId)
        {
            var query = new GetSingleModel(manufacturerId, modelId);
            var result = queryBus.Get<ModelDetails>(query);

            return new JsonResult(result);
        }

        // POST api/manufacturers/{manufacturerId}/models
        [HttpPost("manufacturers/{manufacturerId}/models")]
        public IActionResult PostModels(Guid manufacturerId, [FromBody]PostModelName model)
        {
            if (ModelState.IsValid)
            {
                var command = new AddModel(manufacturerId, model.ModelName);
                var commandResult = commandBus.Send(command);

                if (commandResult.Status == CommandStatus.Success)
                    return GetModels(command.ModelId);
                else
                    return new JsonResult(commandResult.ExceptionMessage);
            }

            return BadRequest(ModelState);
        }

        // PUT api/manufacturers/{manufacturerId}/models/{modelId}
        [HttpPut("manufacturers/{manufacturerId}/models{modelId}")]
        public IActionResult PutModels(Guid manufactuerId, Guid modelId, [FromBody]PutModelName model)
        {
            if (ModelState.IsValid)
            {
                var command = new UpdateModel(manufactuerId, modelId, model.ModelName);
                var commandResult = commandBus.Send(command);

                if (commandResult.Status == CommandStatus.Success)
                    return GetModels(command.ModelId);
                else
                    return new JsonResult(commandResult.ExceptionMessage);
            }

            return BadRequest(ModelState);
        }

        // DELETE api/manufacturers/{manufacturerId}/models/{modelId}
        [HttpDelete("api/manufacturers/{manufacturerId}/models/{modelId}")]
        public IActionResult DeleteModels(Guid manufactuerId, Guid modelId)
        {
            var command = new DeleteModel(manufactuerId, modelId);
            var commandResult = commandBus.Send(command);

            if (commandResult.Status == CommandStatus.Success)
                return Ok();
            else
                return new JsonResult(commandResult.ExceptionMessage);
        }
    }
}
