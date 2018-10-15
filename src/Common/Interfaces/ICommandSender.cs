using System;
using System.Collections.Generic;

namespace Common.Interfaces
{
    public interface ICommandSender
    {
        void AddCommand(ICommand command);
        void InvokeCommandsQueue();

        IEnumerable<Guid> CommandIds { get; }
    }
}
