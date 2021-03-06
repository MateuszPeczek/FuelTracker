﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Common.Interfaces
{
    public interface IQueryHandler<TQuery, TResult> where TQuery : IQuery
    { 
        TResult Handle(TQuery query);
    }
}
