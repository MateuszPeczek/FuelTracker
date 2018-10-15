using Commands.FuelStatisticsCommands;
using Commands.VehicleCommands;
using Common.Interfaces;
using Common.Ordering.FuelStatistics;
using Common.Ordering.Shared;
using Common.Ordering.Vehicle;
using Common.Paging;
using FuelTracker.ApiModels.FuelReportApiModels;
using FuelTracker.ApiModels.VehicleApiModels;
using FuelTracker.ApiModels.VehicleApiModels.RESTCommunication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Queries.FuelStatisticsQueries;
using Queries.VehicleDetailsQueries;
using Queries.VehicleQueries;
using System;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace FuelTracker.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/vehicles")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class VehiclesController : Controller
    {
        private readonly ICommandSender commandBus;
        private readonly IQuerySender queryBus;
        private readonly Common.Interfaces.IAuthorizationService authService;

        public VehiclesController(ICommandSender commandBus,
                                  IQuerySender queryBus,
                                  Common.Interfaces.IAuthorizationService authService)
        {
            this.commandBus = commandBus;
            this.queryBus = queryBus;
            this.authService = authService;
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
            var result = queryBus.InvokeQuery<PaginatedList<VehicleDetails>>(query);

            return Ok(result.Data);
        }

        private VehicleDetails GetVehicleDetails(Guid vehicleId)
        {
            var query = new GetSingleVehicleDetails(vehicleId);
            var result = queryBus.InvokeQuery<VehicleDetails>(query);

            return result.Data;
        }

        // GET: api/vehicles/vehicleId
        [HttpGet("{vehicleId}", Name = "GetVehicle")]
        public IActionResult GetVehicle(Guid vehicleId)
        {
            var result = GetVehicleDetails(vehicleId);

            return Ok(result);
        }

        //POST: api/vehicles
        [HttpPost("")]
        public IActionResult PostVehicle([FromBody]PostNewVehicle model)
        {
            if (ModelState.IsValid)
            {
                var command = new AddVehicle(model.ModelId, new Guid()); //TODO: Retrieve current user id 
                commandBus.AddCommand(command);
                commandBus.InvokeCommandsQueue();

                var result = GetVehicleDetails(command.Id);

                return CreatedAtRoute(
                    "GetVehicle",
                    new { vehicleId = command.Id },
                    result
                    );
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
                commandBus.AddCommand(command);
                commandBus.InvokeCommandsQueue();

                return GetVehicle(command.Id);
            }

            return BadRequest();
        }

        //DELETE: api/vehicles/{vehicleId}
        [HttpDelete("")]
        public IActionResult DeleteVehicle(Guid vehicleId)
        {
            {
                var command = new DeleteVehicle(vehicleId);
                commandBus.AddCommand(command);
                commandBus.InvokeCommandsQueue();

                return Ok();
            }
        }

        //GET: api/vehicles/{vehicleId}/fuelsummary
        [HttpGet("{vehicleId}/fuelsummary")]
        public IActionResult GetFuelSummary(Guid vehicleId)
        {
            var query = new GetFuelSummary(vehicleId);
            var result = queryBus.InvokeQuery<FuelSummaryDetails>(query);

            return Ok(result.Data);
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
            var result = queryBus.InvokeQuery<PaginatedList<ConsumptionReportDetails>>(query);

            return Ok(result.Data);
        }

        private ConsumptionReportDetails GetConsumptionReportDetails(Guid vehicleId, Guid fuelReportId)
        {
            var query = new GetSingleConsumptionReport(vehicleId, fuelReportId);
            var result = queryBus.InvokeQuery<ConsumptionReportDetails>(query);

            return result.Data;
        }

        //GET: api/vehicles/{vehicleId}/fuelreports/{fuelreportId}
        [HttpGet("{vehicleId}/fuelreports/{fuelReportId}", Name = "GetConsumptionReport")]
        public IActionResult GetConsumptionReport(Guid vehicleId, Guid fuelReportId)
        {
            var result = GetConsumptionReportDetails(vehicleId, fuelReportId);
            return Ok(result);
        }

        //POST: api/vehicles/{vehicleId}/fuelreports
        [HttpPost("{vehicleId}/fuelreports")]
        public IActionResult PostConsumptionReport(Guid vehicleId, [FromBody]PostFuelReport model)
        {
            if (ModelState.IsValid)
            {
                var command = new CalculateFuelConsumption(vehicleId, model.UserId, model.Distance, model.FuelBurned, model.PricePerUnit);
                commandBus.AddCommand(command); commandBus.InvokeCommandsQueue();

                var result = GetConsumptionReportDetails(vehicleId, command.Id);

                return CreatedAtRoute(
                    "GetConsumptionReport",
                    new { vehicleId = vehicleId, fuelReportId = command.Id },
                    result
                    );
            }

            return BadRequest(ModelState);
        }

        //PUT: api/vehicles/{vehicleId}/fuelReport/{fuelReportId}
        [HttpPut("{vehicleId}/fuelReport/{fuelReportId}")]
        public IActionResult PutConsumptionReport(Guid vehicleId, Guid fuelReportId, [FromBody]PutFuelReport model)
        {
            if (ModelState.IsValid)
            {
                var userData = authService.GetCurrentUserData();
                var command = new UpdateConsumptionReport(vehicleId, fuelReportId, userData.UserId, model.Distance, model.FuelBurned, model.PricePerUnit);
                commandBus.AddCommand(command);
                commandBus.InvokeCommandsQueue();

                return GetConsumptionReport(vehicleId, fuelReportId);
            }

            return BadRequest(ModelState);
        }

        //DELETE: api/vehicles/{vehicleId}/fuelReport/{fuelReportId}
        [HttpDelete("{vehicleId}/fuelReport/{fuelReportId}")]
        public IActionResult DeleteConsumptionReport(Guid vehicleId, Guid fuelReportId)
        {
            var command = new DeleteConsumptionReport(vehicleId, fuelReportId);
            commandBus.AddCommand(command);
            commandBus.InvokeCommandsQueue();

            return Ok();
        }
    }
}
