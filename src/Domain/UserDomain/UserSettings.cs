using Common.Interfaces;
using Domain.Common;
using Domain.VehicleDomain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.UserDomain
{
    public class UserSettings : IEntity
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public FuelType FuelType { get; set; }
        public Units Units { get; set; }
    }
}
