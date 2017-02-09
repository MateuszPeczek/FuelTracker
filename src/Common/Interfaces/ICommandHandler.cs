using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Common.Interfaces
{
    public interface ICommandHandler<in T> where T : ICommand
    {
        void Handle(T command);
    }
}
