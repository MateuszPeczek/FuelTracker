using Common.Interfaces;
using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Vehicle
{
    public class AddVehicleCommand : ICommand
    {
        public string Name { get; set; }
    }

    public class AddVehicleCommandHandler : ICommandHandler
    {
        private readonly ApplicationContext context;


        public AddVehicleCommandHandler(ApplicationContext context)
        {
            this.context = context;
        }

        public void Handle(ICommand command)
        {
            
        }
    }
}
