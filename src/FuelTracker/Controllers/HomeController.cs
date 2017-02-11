﻿using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Common.Interfaces;
using Application.VehicleService;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace FuelTracker.Controllers
{
    [Route("api/[controller]")]
    public class HomeController : Controller
    {
        private readonly ICommandSender commandBus;
   
        public HomeController(ICommandSender commandBus)
        {
            this.commandBus = commandBus;
        }

        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            var command = new AddVehicleCommand();

            commandBus.Send(command);

            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}