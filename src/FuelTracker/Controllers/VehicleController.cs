using Commands.VehicleCommands;
using Common.Enums;
using Common.Interfaces;
using FuelTracker.ApiModels.VehicleApiModels.RESTCommunication;
using Microsoft.AspNetCore.Mvc;
using Queries.VehicleDetailsQueries;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace FuelTracker.Controllers
{
    [Route("api/vehicle")]
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
        public ActionResult Get()
        {
            var query = new GetVehicleDetailsList();
            var result = queryBus.Get<ICollection<VehicleDetails>>(query);

            return new JsonResult(result);
        }

        [HttpGet("{guid}")]
        public ActionResult Get(Guid guid)
        {
            var query = new GetSingleVehicleDetails(guid);
            var result = queryBus.Get<VehicleDetails>(query);

            return new JsonResult(result);
        }

        [HttpPost]
        public ActionResult Post([FromBody]PostNewVehicle model)
        {
            if (ModelState.IsValid)
            {
                var command = new AddVehicle(model.ModelId);

                try
                {
                    var commandResult = commandBus.Send(command);

                    if (commandResult.Status == CommandStatus.Success)
                        return Get(command.Id);
                }
                catch (Exception ex)
                {
                    var message = new HttpResponseMessage(HttpStatusCode.InternalServerError)
                    {
                        Content = new StringContent(ex.Message)
                    };
                    throw new HttpResponseException(message);
                }
            }

            return BadRequest();
        }

        [HttpPut]
        public ActionResult Put([FromBody]PutUpdateVehicle model)
        {
            if (ModelState.IsValid)
            {
                var command = new UpdateVehicle(model.Guid, model.ProductionYear, model.EngineId);

                try
                {
                    var commandResult = commandBus.Send(command);

                    if (commandResult.Status == CommandStatus.Success)
                        return Get(command.Id);
                }
                catch (Exception ex)
                {
                    var message = new HttpResponseMessage(HttpStatusCode.InternalServerError)
                    {
                        Content = new StringContent(ex.Message)
                    };
                    throw new HttpResponseException(message);
                }
            }

            return BadRequest();
        }

        [HttpDelete("{guid}")]
        public ActionResult Delete(Guid guid)
        {
            var command = new DeleteVehicle(guid);

            try
            {
                var commandResult = commandBus.Send(command);

                if (commandResult.Status == CommandStatus.Success)
                    return Get();
                else
                    return BadRequest();
            }
            catch (Exception ex)
            {
                var message = new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent(ex.Message)
                };
                throw new HttpResponseException(message);
            }
        }
    }
}
