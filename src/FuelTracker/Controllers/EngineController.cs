using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Common.Paging;
using FuelTracker.ApiModels.EngineApiModels;
using Common.Interfaces;
using Commands.EngineCommands;
using Common.Enums;
using Domain.VehicleDomain;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

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


        //[HttpGet]
        //[ProducesResponseType(typeof(PaginatedList<EngineDetails>), 200)]
        //public IActionResult Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}


        [HttpGet("{guid}")]
        public IActionResult Get(Guid guid)
        {
            return null;
        }


        [HttpPost]
        public IActionResult AddEngine([FromBody]FuelType fuelType)
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


        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]string value)
        {
            return null;
        }


        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            return null;
        }
    }
}
