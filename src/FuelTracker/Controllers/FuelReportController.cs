using System;
using Microsoft.AspNetCore.Mvc;
using Common.Interfaces;
using Common.Paging;
using Commands.FuelStatisticsCommands;
using Queries.FuelStatisticsQueries;
using Common.Ordering.Shared;
using Common.Ordering.FuelStatistics;
using FuelTracker.ApiModels.FuelReportApiModels;
using Common.Enums;

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

        [HttpGet("{vehicleId}")]
        public IActionResult GetFuelSummary(Guid vehicleId)
        {
            var query = new GetFuelSummary(vehicleId);
            var result = queryBus.Get<FuelSummaryDetails>(query);

            return new JsonResult(result);
        }

        [HttpGet]
        [ProducesResponseType(typeof(PaginatedList<ConsumptionReportDetails>), 200)]
        public IActionResult GetConsumptionReportsList([FromQuery]int pageSize = 10,
                                [FromQuery]int pageNo = 1,
                                [FromQuery]OrderDirection orderDirection = OrderDirection.Asc,
                                [FromQuery]ConsumptionReportOrderColumn orderColumn = ConsumptionReportOrderColumn.Id,
                                [FromQuery]DateTime? startDate = null,
                                [FromQuery]DateTime? endDate = null)
        {
            var query = new GetConsumptionReportsList(pageSize, pageNo, orderDirection, orderColumn, startDate, endDate);
            var result = queryBus.Get<PaginatedList<ConsumptionReportDetails>>(query);
            
            return new JsonResult(result);
        }

        [HttpGet("{id}")]
        public IActionResult GetSingleConsumptionReport(Guid id)
        {
            var query = new GetSingleConsumptionReport(id);
            var result = queryBus.Get<ConsumptionReportDetails>(query);
            
            return new JsonResult(result);
        }

        [HttpPost]
        public IActionResult Post([FromBody]PostNewFuelReport model)
        {
            if (ModelState.IsValid)
            {
                var command = new CalculateFuelConsumption(model.VehicleId, model.UserId, model.Distance, model.FuelBurned, model.PricePerUnit);

                var commandResult = commandBus.Send(command);

                if (commandResult.Status == CommandStatus.Success)
                    return GetSingleConsumptionReport(command.Id);
            }

            return BadRequest(ModelState);
        }

        [HttpPut]
        public IActionResult Put([FromBody]PutUpdateFuelReport model)
        {
            if (ModelState.IsValid)
            {
                var command = new UpdateConsumptionReport(model.Id, model.Distance, model.FuelBurned, model.PricePerUnit);
                var commandResult = commandBus.Send(command);

                if (commandResult.Status == CommandStatus.Success)
                    return GetSingleConsumptionReport(command.Id);
            }

            return BadRequest(ModelState);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            {
                var command = new DeleteConsumptionReport(id);
                var commandResult = commandBus.Send(command);

                if (commandResult.Status == CommandStatus.Success)
                    return GetConsumptionReportsList(pageSize: 10, pageNo: 1);
                else
                    return BadRequest(ModelState);
            }
        }
    }
}
