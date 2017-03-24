using Common.Enums;
using Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Common.Interfaces
{
    public interface ICommandSender
    {
        ICommandResult Send(ICommand command);
    }
}
