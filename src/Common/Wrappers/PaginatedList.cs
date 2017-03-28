using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Wrappers
{
    public class PaginatedList<T>
    {
        public int PageNo { get; set; }
        public int PageSize { get; set; }

        public ICollection<T> Data { get; set; }
    }
}
