using Commands.EngineCommands;
using Common.Interfaces;
using CustomExceptions.Engine;
using CustomExceptions.Vehicle;
using Domain.VehicleDomain;
using System;
using UnitTests.Extensions;
using Xunit;

namespace UnitTests.Validators.Engine
{
    public class AddEngineValidatorTests
    {
        private readonly ICommandValidator<AddEngine> addEngineCommandValidator;

        public AddEngineValidatorTests()
        {
            addEngineCommandValidator = new AddEngineValidator();
        }

        #region Setup

        private AddEngine GetStubValidCommand()
        {
            return new AddEngine("TestEngine",
                                 100,
                                 200,
                                 4,
                                 2000,
                                 FuelType.Diesel);    
        }

        #endregion

        [Fact]
        public void FuelTypeValueOutOfEnumRange_ThrowsInvalidFuelTypeException()
        {
            var stubCommand = GetStubValidCommand();
            stubCommand.FuelType = (FuelType)3;
            
            Assert.Throws<FuelTypeOutOfRangeException>(() => addEngineCommandValidator.Validate(stubCommand));
        }
        
        [Fact]
        public void EmptyGuidValue_ThrowsInvalidEngineIdException()
        {
            var stubCommand = GetStubValidCommand();
            stubCommand.Id = new Guid();

            Assert.Throws<InvalidEngineIdException>(() => addEngineCommandValidator.Validate(stubCommand));
        }

        [Fact]
        public void ValidCommand_DoNotThrowsException()
        {
            var stubCommand = GetStubValidCommand();
            
            ExtendedAssert.NotThrowsAnyExceptions(() => addEngineCommandValidator.Validate(stubCommand));
        }
    }
}
