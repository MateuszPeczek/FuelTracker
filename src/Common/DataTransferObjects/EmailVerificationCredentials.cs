using System;

namespace Common.DataTransferObjects
{
    public class EmailConfirmationCredentials
    {
        public Guid UserId { get; set; }
        public string Code { get; set; }
    }
}
