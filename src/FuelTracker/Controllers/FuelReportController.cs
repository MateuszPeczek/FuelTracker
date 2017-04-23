using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Common.Interfaces;
using Domain.FuelStatisticsDomain;
using Common.Paging;
using Commands.FuelStatisticsCommands;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FuelTracker.Controllers
{
    [ApiVersion("1.0")]
    [Route("[controller]")]
    public class FuelReportController : Controller
    {
        private readonly ICommandSender commandBus;
        private readonly IQuerySender queryBus;

        public FuelReportController(ICommandSender commandBus, IQuerySender queryBus)
        {
            this.commandBus = commandBus;
            this.queryBus = queryBus;
        }

        [HttpGet]
        [ProducesResponseType(typeof(PaginatedList<ConsumptionReport>), 200)]
        public IActionResult Get([FromQuery]int pageSize = 10,
                                [FromQuery]int pageNo = 1)//,
                                //[FromQuery]VehicleOrderColumn orderbyColumn = VehicleOrderColumn.Manufacturer,
                                //[FromQuery]OrderDirection orderDirection = OrderDirection.Asc)
        {
            //var query = new GetVehicleDetailsList(pageSize, pageNo, orderbyColumn, orderDirection);
            //var result = queryBus.Get<PaginatedList<VehicleDetails>>(query);
            //
            //return new JsonResult(result);
            return null;
        }

        [HttpGet("{guid}")]
        public IActionResult Get(Guid guid)
        {
            //var query = new GetSingleVehicleDetails(guid);
            //var result = queryBus.Get<VehicleDetails>(query);
            //
            //return new JsonResult(result);
            return null;
        }

        //[HttpPost]
        //public IActionResult Post([FromBody]AddConsumptionReport command)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var command = new AddVehicle(model.ModelId);

        //        var commandResult = commandBus.Send(command);

        //        if (commandResult.Status == CommandStatus.Success)
        //            return await Get(command.Id);
        //    }

        //    return BadRequest(ModelState);
        //    return null;
        //}

        //[HttpPut]
        //public IActionResult Put([FromBody]PutUpdateVehicle model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var command = new UpdateVehicle(model.Guid, model.ProductionYear, model.EngineId);
        //        var commandResult = commandBus.Send(command);

        //        if (commandResult.Status == CommandStatus.Success)
        //            return await Get(command.Id);

        //    }

        //    return BadRequest();
        //    return null;
        //}

        //[HttpDelete("{guid}")]
        //public IActionResult Delete(Guid guid)
        //{
        //    {
        //        var command = new DeleteVehicle(guid);
        //        var commandResult = commandBus.Send(command);

        //        if (commandResult.Status == CommandStatus.Success)
        //            return await Get(pageSize: 10, pageNo: 1);
        //        else
        //            return BadRequest(ModelState);
        //    }
        //}
    }
}
