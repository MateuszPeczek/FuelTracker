using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Interfaces
{
    public interface ICommandValidator<T> where T : ICommand
    {
        void Validate(T command);
    }
}
