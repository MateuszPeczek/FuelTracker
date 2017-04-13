using Commands.VehicleCommands;
using Common.Enums;
using Common.Interfaces;
using Common.Ordering.Shared;
using Common.Ordering.Vehicle;
using Common.Paging;
using FuelTracker.ApiModels.VehicleApiModels.RESTCommunication;
using Microsoft.AspNetCore.Mvc;
using Queries.VehicleDetailsQueries;
using System;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace FuelTracker.Controllers
{
    [ApiVersion("1.0")]
    [Route("[controller]")]
    public class VehicleController : Controller
    {
        private readonly ICommandSender commandBus;
        private readonly IQuerySender queryBus;

        public VehicleController(ICommandSender commandBus, IQuerySender queryBus)
        {
            this.commandBus = commandBus;
            this.queryBus = queryBus;
        }

        [HttpGet]
        [ProducesResponseType(typeof(PaginatedList<VehicleDetails>), 200)]
        public async Task<IActionResult> Get([FromQuery]int pageSize = 10, 
                                [FromQuery]int pageNo = 1, 
                                [FromQuery]VehicleOrderColumn orderbyColumn = VehicleOrderColumn.Manufacturer,
                                [FromQuery]OrderDirection orderDirection = OrderDirection.Asc)
        {
            var query = new GetVehicleDetailsList(pageSize, pageNo, orderbyColumn, orderDirection);
            var result = queryBus.Get<PaginatedList<VehicleDetails>>(query);

            return new JsonResult(result);
        }

        [HttpGet("{guid}")]
        public async Task<IActionResult> Get(Guid guid)
        {
            var query = new GetSingleVehicleDetails(guid);
            var result = queryBus.Get<VehicleDetails>(query);

            return new JsonResult(result);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]PostNewVehicle model)
        {
            if (ModelState.IsValid)
            {
                var command = new AddVehicle(model.ModelId);

                var commandResult = commandBus.Send(command);

                if (commandResult.Status == CommandStatus.Success)
                    return await Get(command.Id);
            }

            return BadRequest(ModelState);
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody]PutUpdateVehicle model)
        {
            if (ModelState.IsValid)
            {
                var command = new UpdateVehicle(model.Guid, model.ProductionYear, model.EngineId);
                var commandResult = commandBus.Send(command);

                if (commandResult.Status == CommandStatus.Success)
                    return await Get(command.Id);

            }

            return BadRequest();
        }

        [HttpDelete("{guid}")]
        public async Task<IActionResult> Delete(Guid guid)
        {
            {
                var command = new DeleteVehicle(guid);
                var commandResult = commandBus.Send(command);

                if (commandResult.Status == CommandStatus.Success)
                    return await Get(pageSize: 10, pageNo: 1);
                else
                    return BadRequest(ModelState);
            }
        }
    }
}
