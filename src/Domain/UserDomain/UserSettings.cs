using Common.Interfaces;
using Domain.Common;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.UserDomain
{
    public class UserSettings : IEntity
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        public Units Units { get; set; }
    }
}
