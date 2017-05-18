using Commands.FuelStatisticsCommands;
using Commands.VehicleCommands;
using Common.Enums;
using Common.Interfaces;
using Common.Ordering.FuelStatistics;
using Common.Ordering.Shared;
using Common.Ordering.Vehicle;
using Common.Paging;
using FuelTracker.ApiModels.FuelReportApiModels;
using FuelTracker.ApiModels.VehicleApiModels;
using FuelTracker.ApiModels.VehicleApiModels.RESTCommunication;
using Microsoft.AspNetCore.Mvc;
using Queries.FuelStatisticsQueries;
using Queries.VehicleDetailsQueries;
using Queries.VehicleQueries;
using System;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace FuelTracker.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/vehicles")]
    public class VehiclesController : Controller
    {
        private readonly ICommandSender commandBus;
        private readonly IQuerySender queryBus;

        public VehiclesController(ICommandSender commandBus, IQuerySender queryBus)
        {
            this.commandBus = commandBus;
            this.queryBus = queryBus;
        }

        // GET: api/vehicles
        [HttpGet("")]
        [ProducesResponseType(typeof(PaginatedList<VehicleDetails>), 200)]
        public IActionResult GetVehicles([FromQuery]int pageSize = 10, 
                                         [FromQuery]int pageNo = 1, 
                                         [FromQuery]VehicleOrderColumn orderbyColumn = VehicleOrderColumn.Manufacturer,
                                         [FromQuery]OrderDirection orderDirection = OrderDirection.Asc)
        {
            var query = new GetVehicleDetailsList(pageSize, pageNo, orderbyColumn, orderDirection);
            var result = queryBus.Get<PaginatedList<VehicleDetails>>(query);

            return Ok(result);
        }

        // GET: api/vehicles/vehicleId
        [HttpGet("{vehicleId}")]
        public IActionResult GetVehicles(Guid vehicleId)
        {
            var query = new GetSingleVehicleDetails(vehicleId);
            var result = queryBus.Get<VehicleDetails>(query);

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        //POST: api/vehicles
        [HttpPost("")]
        public IActionResult PostVehicle([FromBody]PostNewVehicle model)
        {
            if (ModelState.IsValid)
            {
                var command = new AddVehicle(model.ModelId);

                var commandResult = commandBus.Send(command);

                if (commandResult.Status == CommandStatus.Success)
                    return GetVehicles(command.Id);
            }

            return BadRequest(ModelState);
        }

        //PUT: api/vehicles/{vehicleId}
        [HttpPut("{vehicleId}")]
        public IActionResult PutVehicle(Guid vehicleId, [FromBody]PutUpdateVehicle model)
        {
            if (ModelState.IsValid)
            {
                var command = new UpdateVehicle(vehicleId, model.ProductionYear, model.EngineId);
                var commandResult = commandBus.Send(command);

                if (commandResult.Status == CommandStatus.Success)
                    return GetVehicles(command.Id);

            }

            return BadRequest();
        }

        //DELETE: api/vehicles/{vehicleId}
        [HttpDelete("")]
        public IActionResult DeleteVehicle(Guid vehicleId)
        {
            {
                var command = new DeleteVehicle(vehicleId);
                var commandResult = commandBus.Send(command);

                if (commandResult.Status == CommandStatus.Success)
                    return Ok();
                else
                    return BadRequest(ModelState);
            }
        }

        //GET: api/vehicles/{vehicleId}/fuelsummary
        [HttpGet("{vehicleId}/fuelsummary")]
        public IActionResult GetFuelSummary(Guid vehicleId)
        {
            var query = new GetFuelSummary(vehicleId);
            var result = queryBus.Get<FuelSummaryDetails>(query);

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        //GET: api/vehicles/{vehicleId}/fuelreports
        [HttpGet("{vehicleId}/fuelreports")]
        [ProducesResponseType(typeof(PaginatedList<ConsumptionReportDetails>), 200)]
        public IActionResult GetConsumptionReports([FromQuery]int pageSize = 10,
                                                       [FromQuery]int pageNo = 1,
                                                       [FromQuery]OrderDirection orderDirection = OrderDirection.Asc,
                                                       [FromQuery]ConsumptionReportOrderColumn orderColumn = ConsumptionReportOrderColumn.Id,
                                                       [FromQuery]DateTime? startDate = null,
                                                       [FromQuery]DateTime? endDate = null)
        {
            var query = new GetConsumptionReportsList(pageSize, pageNo, orderDirection, orderColumn, startDate, endDate);
            var result = queryBus.Get<PaginatedList<ConsumptionReportDetails>>(query);

            return Ok(result);
        }

        //GET: api/vehicles/{vehicleId}/fuelreports/{fuelreportId}
        [HttpGet("{vehicleId}/fuelreports/{fuelReportId}")]
        public IActionResult GetConsumptionReport(Guid vehicleId, Guid fuelReportId)
        {
            var query = new GetSingleConsumptionReport(vehicleId, fuelReportId);
            var result = queryBus.Get<ConsumptionReportDetails>(query);

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        //POST: api/vehicles/{vehicleId}/fuelreports
        [HttpPost("{vehicleId}/fuelreports")]
        public IActionResult Post(Guid vehicleId, [FromBody]PostFuelReport model)
        {
            if (ModelState.IsValid)
            {
                var command = new CalculateFuelConsumption(vehicleId, model.UserId, model.Distance, model.FuelBurned, model.PricePerUnit);

                var commandResult = commandBus.Send(command);

                if (commandResult.Status == CommandStatus.Success)
                    return GetConsumptionReport(vehicleId, command.Id);
                else
                    return new JsonResult(commandResult);
            }

            return BadRequest(ModelState);
        }

        //PUT: api/vehicles/{vehicleId}/fuelReport/{fuelReportId}
        [HttpPut("{vehicleId}/fuelReport/{fuelReportId}")]
        public IActionResult Put(Guid vehicleId, Guid fuelReportId, [FromBody]PutFuelReport model)
        {
            if (ModelState.IsValid)
            {
                var command = new UpdateConsumptionReport(vehicleId, fuelReportId, model.Distance, model.FuelBurned, model.PricePerUnit);
                var commandResult = commandBus.Send(command);

                if (commandResult.Status == CommandStatus.Success)
                    return GetConsumptionReport(vehicleId, fuelReportId);
                else
                    return new JsonResult(commandResult);
            }

            return BadRequest(ModelState);
        }

        //DELETE: api/vehicles/{vehicleId}/fuelReport/{fuelReportId}
        [HttpDelete("{vehicleId}/fuelReport/{fuelReportId}")]
        public IActionResult Delete(Guid vehicleId, Guid fuelReportId)
        {
            {
                var command = new DeleteConsumptionReport(vehicleId, fuelReportId);
                var commandResult = commandBus.Send(command);

                if (commandResult.Status == CommandStatus.Success)
                    return Ok();
                else
                    return new JsonResult(commandResult);
            }
        }
    }
}
