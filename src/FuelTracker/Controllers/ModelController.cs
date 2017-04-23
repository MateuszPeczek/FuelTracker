using System;
using Microsoft.AspNetCore.Mvc;
using Common.Interfaces;
using Common.Paging;
using Queries.ModelQueries;
using Common.Ordering.Shared;
using Commands.ModelCommands;
using FuelTracker.ApiModels.ModelApiModels;
using Common.Enums;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FuelTracker.Controllers
{
    [ApiVersion("1.0")]
    [Route("[controller]")]
    public class ModelController : Controller
    {
        private readonly ICommandSender commandBus;
        private readonly IQuerySender queryBus;

        public ModelController(ICommandSender commandBus, IQuerySender queryBus)
        {
            this.commandBus = commandBus;
            this.queryBus = queryBus;
        }

        // GET: api/values
        [HttpGet]
        [ProducesResponseType(typeof(PaginatedList<ModelDetails>), 200)]
        public IActionResult Get([FromQuery]int pageSize = 10,
                                [FromQuery]int pageNo = 1,
                                [FromQuery]OrderDirection orderDirection = OrderDirection.Asc)
        {
            var query = new GetModelsList(pageSize, pageNo, orderDirection);
            var result = queryBus.Get<PaginatedList<ModelDetails>>(query);

            return new JsonResult(result);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public IActionResult Get(Guid id)
        {
            var query = new GetSingleModel(id);
            var result = queryBus.Get<ModelDetails>(query);

            return new JsonResult(result);
        }

        // POST api/values
        [HttpPost]
        public IActionResult Post([FromBody]PostModelName model)
        {
            if (ModelState.IsValid)
            {
                var command = new AddModel(model.ManufactuerId, model.ModelName);
                var commandResult = commandBus.Send(command);

                if (commandResult.Status == CommandStatus.Success)
                    return Get(command.Id);
                else
                    return new JsonResult(commandResult.ExceptionMessage);
            }

            return BadRequest(ModelState);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public IActionResult Put([FromBody]PutModelName model)
        {
            if (ModelState.IsValid)
            {
                var command = new UpdateModel(model.Id, model.ModelName, model.ManufactuerId);
                var commandResult = commandBus.Send(command);

                if (commandResult.Status == CommandStatus.Success)
                    return Get(command.Id);
                else
                    return new JsonResult(commandResult.ExceptionMessage);
            }

            return BadRequest(ModelState);
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            var command = new DeleteModel(id);
            var commandResult = commandBus.Send(command);

            if (commandResult.Status == CommandStatus.Success)
                return Ok();
            else
                return new JsonResult(commandResult.ExceptionMessage);
        }
    }
}
