using Commands.VehicleCommands;
using Common.Enums;
using Common.Interfaces;
using FuelTracker.ApiModels.Vehicle;
using Microsoft.AspNetCore.Mvc;
using System;
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

        public VehicleController(ICommandSender commandSender)
        {
            this.commandBus = commandSender;
        }

        // GET: api/values
        [HttpGet]
        public ActionResult Get()
        {
            throw new NotImplementedException();
        }

        // GET api/values/5
        [HttpGet("{guid}")]
        public IActionResult Get(Guid guid)
        {
            throw new NotImplementedException();
        }

        // POST api/values
        [HttpPost]
        public IActionResult Post(PostVehicle model)
        {
            if (ModelState.IsValid)
            {
                var command = new AddVehicleCommand(model.ModelID, model.ManufacturerID);

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
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]string value)
        {
            throw new NotImplementedException();
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}
