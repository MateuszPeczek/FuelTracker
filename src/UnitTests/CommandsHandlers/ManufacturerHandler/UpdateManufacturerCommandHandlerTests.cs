using Commands.ManufacturerCommands;
using Common.Interfaces;
using FakeItEasy;
using System;
using System.Linq;
using Xunit;

namespace UnitTests.CommandsHandlers.ManufacturerHandler
{
    public class UpdateManufacturerCommandHandlerTests : BaseHandlersUnitTests
    {
        protected readonly ICommandValidator<UpdateManufacturer> updateManufacturerValidator;
        protected readonly ICommandHandler<UpdateManufacturer> updateManufacturerHandler;

        public UpdateManufacturerCommandHandlerTests()
        {
            updateManufacturerValidator = A.Fake<ICommandValidator<UpdateManufacturer>>();
            updateManufacturerHandler = new UpdateManufacturerHandler(unitOfWork, updateManufacturerValidator);
        }

        [Fact]
        public void CommandValid_ManufacturerUpdated()
        {
            var manufacturerId = InsertManufacturerToDatabase();
            var updateManufacturerCommand = new UpdateManufacturer(manufacturerId, "UpdatedName");

            A.CallTo(() => updateManufacturerValidator.Validate(updateManufacturerCommand)).DoesNothing();

            updateManufacturerHandler.Handle(updateManufacturerCommand);

            context.SaveChanges();
            var result = context.Manufacturer.FirstOrDefault(m => m.Id == updateManufacturerCommand.Id);

            A.CallTo(() => updateManufacturerValidator.Validate(updateManufacturerCommand)).MustHaveHappened(Repeated.Exactly.Once);
            Assert.NotNull(result);
            Assert.Equal(result.Id, updateManufacturerCommand.Id);
            Assert.Equal(result.Name, updateManufacturerCommand.Name);
        }

        [Fact]
        public void ValidatorThrowsException_HandlerThrowsException()
        {
            var manufacturerId = InsertManufacturerToDatabase();
            var updateManufacturerCommand = new UpdateManufacturer(manufacturerId, "UpdatedName");

            A.CallTo(() => updateManufacturerValidator.Validate(updateManufacturerCommand)).Throws<Exception>();

            Assert.ThrowsAny<Exception>(() => updateManufacturerHandler.Handle(updateManufacturerCommand));
            A.CallTo(() => updateManufacturerValidator.Validate(updateManufacturerCommand)).MustHaveHappened(Repeated.Exactly.Once);

            context.SaveChanges();
            var result = context.Manufacturer.FirstOrDefault(m => m.Id == updateManufacturerCommand.Id);
            Assert.NotNull(result);
            Assert.Equal(manufacturerId, result.Id);
            Assert.Equal(expectedManufacturerName, result.Name);
        }
    }
}
