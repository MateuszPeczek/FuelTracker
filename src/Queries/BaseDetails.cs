using System;

namespace Queries
{
    public class BaseDetails
    {
        dynamic _id;
        public dynamic Id
        {
            get
            {
                var guid = new Guid((byte[])_id);
                return guid.ToString();
            }
            set
            {
                _id = value;
            }
        }
    }
}
