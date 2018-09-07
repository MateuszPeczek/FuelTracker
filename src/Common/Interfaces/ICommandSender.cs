using System;
using System.Collections.Generic;

namespace Common.Interfaces
{
    public interface ICommandSender
    {
        void AddCommand(ICommand command);
        ICommandResult InvokeCommandsQueue();

        IEnumerable<Guid> CommandIds { get; }
    }
}
