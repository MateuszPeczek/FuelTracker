using Commands.ManufacturerCommands;
using Commands.ModelCommands;
using Common.Enums;
using Common.Interfaces;
using Common.Ordering.Shared;
using Common.Paging;
using FuelTracker.ApiModels.ManufacturerApiModels;
using FuelTracker.Helpers;
using Microsoft.AspNetCore.Mvc;
using Queries.ManufacturerQueries;
using Queries.ModelQueries;
using System;
using System.Collections.Generic;
using System.Linq;

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
            var result = queryBus.InvokeQuery<PaginatedList<ManufacturerDetails>>(query);

            return Ok(result);
        }

        private ManufacturerDetails GetManufacturerDetails(Guid manufacturerId)
        {
            var query = new GetSingleManufacturer(manufacturerId);
            var result = queryBus.InvokeQuery<ManufacturerDetails>(query);

            return result.Data;
        }

        private IEnumerable<ManufacturerDetails> GetManufacturersDetails(IEnumerable<Guid> manufacturersIds)
        {
            var query = new GetManufacturersByIds(manufacturersIds);
            var result = queryBus.InvokeQuery<IEnumerable<ManufacturerDetails>>(query);

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

        // GET api/manufacturers/{manufacturerId}
        [HttpGet("({ids})", Name = "GetManufacturersByIds")]
        public IActionResult GetManufacturersByIds([ModelBinder(BinderType = typeof(CollectionModelBinder))]IEnumerable<Guid> manufacturersIds)
        {
            var query = new GetManufacturersByIds(manufacturersIds);
            var result = queryBus.InvokeQuery<IEnumerable<ManufacturerDetails>>(query);

            if (result.QueryStatus == ActionStatus.Success)
                return Ok(result);

            if (result.QueryStatus == ActionStatus.BadRequest)
                return BadRequest(result.ExceptionMessage);

            return StatusCode(500, result.ExceptionMessage);
            
        }

        // POST api/manufacturers
        [HttpPost("")]
        public IActionResult PostManufacturer([FromBody]PostManufacturer model)
        {
            if (ModelState.IsValid)
            {
                var command = new AddManufacturer(model.Name, model.ModelsNames);
                commandBus.AddCommand(command);

                var commandResult = commandBus.InvokeCommandsQueue();

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

        [HttpPost("collection")]
        public IActionResult PostManufacturersCollection([FromBody]IEnumerable<PostManufacturer> manufacturersCollection)
        {
            if (ModelState.IsValid)
            {
                if (!manufacturersCollection.Any())
                    return BadRequest("Empty collection");

                foreach (var manufacturer in manufacturersCollection)
                {
                    var command = new AddManufacturer(manufacturer.Name, manufacturer.ModelsNames);
                    commandBus.AddCommand(command);
                }

                var commandResult = commandBus.InvokeCommandsQueue();

                if (commandResult.Status == ActionStatus.Success)
                {
                    var result = GetManufacturersDetails(commandBus.CommandIds);

                    return CreatedAtRoute(
                        "GetManufacturersByIds",
                        new { ids = string.Join(",", commandBus.CommandIds) },
                        result);
                }
                else
                {
                    return StatusCode(500, commandResult.ExceptionMessage);
                }
            }
            else
            {
                return BadRequest(manufacturersCollection);
            }
        }

        // PUT api/manufacturers/{manufacturerId}
        [HttpPut("{manufacturerId}")]
        public IActionResult PutManufacturer(Guid manufacturerId, [FromBody]PutManufactuer model)
        {
            if (ModelState.IsValid)
            {
                var command = new UpdateManufacturer(manufacturerId, model.Name);
                commandBus.AddCommand(command);

                var commandResult = commandBus.InvokeCommandsQueue();

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
            commandBus.AddCommand(command);

            var commandResult = commandBus.InvokeCommandsQueue();

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
            var result = queryBus.InvokeQuery<PaginatedList<ModelDetails>>(query);

            return Ok(result);
        }

        private ModelDetails GetModelDetails(Guid manufacturerId, Guid modelId)
        {
            var query = new GetSingleModel(manufacturerId, modelId);
            var result = queryBus.InvokeQuery<ModelDetails>(query);

            return result.Data;
        }

        private IEnumerable<ModelDetails> GetModelDetailsByIdsData(Guid manufacturerId, IEnumerable<Guid> ids)
        {
            var query = new GetModelsDetailsByIds(manufacturerId, ids);
            var result = queryBus.InvokeQuery<IEnumerable<ModelDetails>>(query);

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

        // GET: api/manufacturers/{manufacturerId}/models
        [HttpGet("{manufacturerId}/models/({ids})", Name = "GetModelsByIds")]
        public IActionResult GetModelsByIds(Guid manufacturerId, IEnumerable<Guid> ids)
        {
            var query = new GetModelsDetailsByIds(manufacturerId, ids);
            var result = queryBus.InvokeQuery<IEnumerable<ModelDetails>>(query);

            if (result.QueryStatus == ActionStatus.Success)
                return Ok(result);

            if (result.QueryStatus == ActionStatus.BadRequest)
                return BadRequest(result.ExceptionMessage);

            return StatusCode(500, result.ExceptionMessage);
        }

        // POST api/manufacturers/{manufacturerId}/models
        [HttpPost("{manufacturerId}/models")]
        public IActionResult PostModel(Guid manufacturerId, [FromBody]PostModelName model)
        {
            if (ModelState.IsValid)
            {
                var command = new AddModelName(manufacturerId, model.ModelName);
                commandBus.AddCommand(command);

                var commandResult = commandBus.InvokeCommandsQueue();

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

        // POST api/manufacturers/{manufacturerId}/models/collection
        [HttpPost("{manufacturerId}/models/collection")]
        public IActionResult PostModelsCollection(Guid manufacturerId, [FromBody]IEnumerable<PostModelName> modelsCollection)
        {
            if (ModelState.IsValid)
            {
                if (!modelsCollection.Any())
                    return BadRequest("Empty collection");

                foreach (var model in modelsCollection)
                {
                    var command = new AddModelName(manufacturerId, model.ModelName);
                    commandBus.AddCommand(command);
                }

                var commandResult = commandBus.InvokeCommandsQueue();

                if (commandResult.Status == ActionStatus.Success)
                {
                    var result = GetModelDetailsByIdsData(manufacturerId, commandBus.CommandIds);

                    return CreatedAtRoute(
                        "GetModelsByIds",
                        new { ids = string.Join(",", commandBus.CommandIds) },
                        result);
                }
                else
                {
                    return StatusCode(500, commandResult.ExceptionMessage);
                }
            }
            else
            {
                return BadRequest(modelsCollection);
            }
        }

        // PUT api/manufacturers/{manufacturerId}/models/{modelId}
        [HttpPut("{manufacturerId}/models{modelId}")]
        public IActionResult PutModel(Guid manufactuerId, Guid modelId, [FromBody]PutModelName model)
        {
            if (ModelState.IsValid)
            {
                var command = new UpdateModelName(modelId, manufactuerId, model.ModelName);
                commandBus.AddCommand(command);

                var commandResult = commandBus.InvokeCommandsQueue();

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
            var command = new DeleteModelName(manufactuerId, modelId);
            commandBus.AddCommand(command);

            var commandResult = commandBus.InvokeCommandsQueue();

            if (commandResult.Status == ActionStatus.Success)
                return Ok();
            else
                return new JsonResult(commandResult.ExceptionMessage);
        }
    }
}
