using System;

namespace Common.DataTransferObjects
{
    public class RefreshTokenCredentials
    {
        public Guid UserId { get; set; }
        public string Token { get; set; }
    }
}
