﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Paging
{
    public class PaginatedList<T>
    {
        public int PageNo { get; set; }
        public int PageSize { get; set; }

        public ICollection<T> Data { get; set; }
    }
}
