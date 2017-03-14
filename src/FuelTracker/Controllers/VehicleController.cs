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
            var result = queryBus.Send<ICollection<VehicleDetails>>(query);

            return new JsonResult(result);
        }

        // GET api/values/5
        [HttpGet("{guid}")]
        public ActionResult Get(Guid guid)
        {
            var query = new GetSingleVehicleDetails(guid);
            var result = queryBus.Send<VehicleDetails>(query);

            return new JsonResult(result);
        }

        // POST api/values
        [HttpPost]
        public ActionResult Post([FromBody]PostNewVehicle model)
        {
            if (ModelState.IsValid)
            {
                var command = new AddVehicle(model.ModelId);

                try
                {
                    var commandResult = commandBus.Send(command);

                    if (commandResult == CommandResult.Success)
                        return Get(command.Guid);
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

        // PUT api/values/5
        [HttpPut]
        public ActionResult Put([FromBody]PutUpdateVehicle model)
        {
            if (ModelState.IsValid)
            {
                var command = new UpdateVehicle(model.Guid, model.ProductionYear, model.EngineId);

                try
                {
                    var commandResult = commandBus.Send(command);

                    if (commandResult == CommandResult.Success)
                        return Get(command.Guid);
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

        // DELETE api/values/5
        [HttpDelete("{guid}")]
        public ActionResult Delete(Guid guid)
        {
            var command = new DeleteVehicle(guid);

            try
            {
                var commandResult = commandBus.Send(command);

                if (commandResult == CommandResult.Success)
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
