using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Common.Interfaces;
using Queries.ManufacturerQueries;
using Common.Paging;
using Common.Ordering.Shared;
using Commands.ManudaturerCommands;
using Common.Enums;
using Commands.ManufacturerCommands;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FuelTracker.Controllers
{
    [ApiVersion("1.0")]
    [Route("[controller]")]
    public class ManufacturerController : Controller
    {
        private readonly ICommandSender commandBus;
        private readonly IQuerySender queryBus;

        public ManufacturerController(ICommandSender commandBus, IQuerySender queryBus)
        {
            this.commandBus = commandBus;
            this.queryBus = queryBus;
        }

        // GET: api/values
        [HttpGet]
        [ProducesResponseType(typeof(PaginatedList<ManufacturerDetails>), 200)]
        public IActionResult Get([FromQuery]int pageSize = 10,
                                [FromQuery]int pageNo = 1,
                                [FromQuery]OrderDirection orderDirection = OrderDirection.Asc)
        {
            var query = new GetManufacturersList(pageSize, pageNo, orderDirection);
            var result = queryBus.Get<PaginatedList<ManufacturerDetails>>(query);

            return new JsonResult(result);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public IActionResult Get(Guid id)
        {
            var query = new GetSingleManufacturer(id);
            var result = queryBus.Get<ManufacturerDetails>(query);

            return new JsonResult(result);
        }

        // POST api/values
        [HttpPost]
        public IActionResult Post([FromBody]string name)
        {
            if (ModelState.IsValid)
            {
                var command = new AddManufacturer(name);
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
        public IActionResult Put(Guid id, [FromBody]string name)
        {
            if (ModelState.IsValid)
            {
                var command = new UpdateManufacturer(id, name);
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
            var command = new DeleteManufacturer(id);
            var commandResult = commandBus.Send(command);

            if (commandResult.Status == CommandStatus.Success)
                return Ok();
            else
                return new JsonResult(commandResult.ExceptionMessage);
        }
    }
}
