﻿using Common.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Interfaces
{
    public interface ICommandResult
    {
        ActionStatus Status { get; set; }
        string ExceptionMessage { get; set; }
    }
}
