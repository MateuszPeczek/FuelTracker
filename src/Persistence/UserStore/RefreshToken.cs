using Common.Interfaces;
using Domain.UserDomain;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Persistence.UserStore
{
    public class RefreshToken : IEntity
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        public string Token { get; set; }
    }
}
