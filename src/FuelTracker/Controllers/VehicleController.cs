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
            using (var db = new SqlConnection(@"Server=.;Database=FuelTracker;Trusted_Connection=True;MultipleActiveResultSets=true"))
            {
                var sqlQuery = @"select mf.name as manufacturer, md.name as modelname, v.productionyear, e.name, e.power, e.torque, e.cylinders, e.displacement, e.fueltype
                                 from vehicle v
                                 join ModelName md on md.Id = v.ModelNameId
                                 left join Manufacturer mf on mf.Id = md.ManufacturerID
                                 left join Engine e on e.Id = v.EngineId
                                 where v.Guid = @Guid";


                var result = db.Query(sqlQuery, new { Guid = guid }).Select(r => new VehicleDataModel()
                {
                    Engine = new EngineDataModel() { Cylinders = r.cylinders, Displacement = r.displacement, FuelType = (Domain.VehicleDomain.FuelType)r.fueltype, Name = r.name, Power = r.power, Torque = r.torque },
                    Manufacturer = r.manufacturer,
                    Model = r.modelname,
                    ProductionYear = r.productionyear                 
                }).FirstOrDefault();

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
