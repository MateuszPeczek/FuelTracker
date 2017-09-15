using Commands.EngineCommands;
using Common.Interfaces;
using Ploeh.AutoFixture;
using Xunit;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Domain.VehicleDomain;
using CustomExceptions.Engine;

namespace Tests.Validators.Engine
{
    [ExcludeFromCodeCoverage]
    public class AddEngineValidatorTests
    {
        private readonly ICommandValidator<AddEngine> addEngineCommandValidator;
        private readonly Fixture fixture;

        public AddEngineValidatorTests()
        {
            addEngineCommandValidator = new AddEngineValidator();
        }

        #region Setup

        private AddEngine GetStubAddEngineCommand()
        {
            return fixture.Create<AddEngine>();
        }

        #endregion

        [Fact]
        private void FuelTypeValueOutOfEnumRange_ThrowsInvalidFuelTypeException()
        {
            var stubCommand = GetStubAddEngineCommand();
            stubCommand.FuelType = (FuelType)3;
            
            Assert.Throws<FuelTypeOutOfRangeException>(() => addEngineCommandValidator.Validate(stubCommand));
        }
        
    }
}
