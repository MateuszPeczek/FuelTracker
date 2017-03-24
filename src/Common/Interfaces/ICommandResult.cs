using Common.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Interfaces
{
    public interface ICommandResult
    {
        CommandStatus Status { get; set; }
        Exception CommandException { get; set; }
    }
}
