using Common.Enums;
using Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.CommunicationModels
{
    public class CommandResult : ICommandResult
    {
        public CommandStatus Status { get; set; }
        public string ExceptionMessage { get; set; }
    }
}
