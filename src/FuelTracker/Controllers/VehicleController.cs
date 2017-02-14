using Commands.VehicleCommands;
using Common.Enums;
using Common.Interfaces;
using Dapper;
using FuelTracker.ApiModels.VehicleApiModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Data.SqlClient;
using System.Linq;
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

        public VehicleController(ICommandSender commandBus)
        {
            this.commandBus = commandBus;
        }

        // GET: api/values
        [HttpGet]
        public ActionResult Get()
        {
            throw new NotImplementedException();
        }

        // GET api/values/5
        [HttpGet("{guid}")]
        public ActionResult Get(Guid guid)
        {
            using (var db = new SqlConnection(@"Server=.;Database=FuelTracker;Trusted_Connection=True;MultipleActiveResultSets=true"))
            {
                var sqlQuery = @"select mf.name as manufacturer, md.name as model, v.productionyear, e.name as enginename, e.power, e.torque, e.cylinders, e.displacement, e.fueltype
                                 from vehicle v
                                 join ModelName md on md.Id = v.ModelNameId
                                 left join Manufacturer mf on mf.Id = md.ManufacturerID
                                 left join Engine e on e.Id = v.EngineId
                                 where v.Guid = @Guid";


                var result = db.Query<VehicleDetailsModel>(sqlQuery, new { Guid = guid }).SingleOrDefault();

                if (result != null)
                    return new JsonResult(result);
                else
                    return NotFound();
            }
        }

        // POST api/values
        [HttpPost]
        public IActionResult Post([FromBody]PostVehicle model)
        {
            if (ModelState.IsValid)
            {
                var command = new AddVehicleCommand(model.ModelId);

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
