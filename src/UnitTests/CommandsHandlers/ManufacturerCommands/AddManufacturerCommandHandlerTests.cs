using Commands.ManufacturerCommands;
using Common.Interfaces;
using FakeItEasy;
using System;
using System.Linq;
using Xunit;

namespace UnitTests.CommandsHandlers.ManufacturerCommands
{
    public class AddManufacturerCommandHandlerTests : BaseHandlersUnitTests
    {
        protected readonly ICommandValidator<AddManufacturer> addManufacturerValidator;
        protected readonly ICommandHandler<AddManufacturer> addManufacturerHandler;

        public AddManufacturerCommandHandlerTests()
        {
            addManufacturerValidator = A.Fake<ICommandValidator<AddManufacturer>>();
            addManufacturerHandler = new AddManufacturerHandler(unitOfWork, addManufacturerValidator);
        }

        [Fact]
        public void CommandValid_ManufacturerAdded()
        {
            var addManufacturerCommand = new AddManufacturer(expectedManufacturerName);

            A.CallTo(() => addManufacturerValidator.Validate(addManufacturerCommand)).DoesNothing();

            addManufacturerHandler.Handle(addManufacturerCommand);

            context.SaveChanges();
            var result = context.Manufacturer.FirstOrDefault(m => m.Id == addManufacturerCommand.Id);

            A.CallTo(() => addManufacturerValidator.Validate(addManufacturerCommand)).MustHaveHappened(Repeated.Exactly.Once);
            Assert.NotNull(result);
            Assert.Equal(result.Id, addManufacturerCommand.Id);
            Assert.Equal(result.Name, addManufacturerCommand.Name);
        }

        [Fact]
        public void ValidatorThrowsException_HandlerThrowsException()
        {
            var addManufacturerCommand = new AddManufacturer(expectedManufacturerName);

            A.CallTo(() => addManufacturerValidator.Validate(addManufacturerCommand)).Throws<Exception>();

            Assert.ThrowsAny<Exception>(() => addManufacturerHandler.Handle(addManufacturerCommand));
            A.CallTo(() => addManufacturerValidator.Validate(addManufacturerCommand)).MustHaveHappened(Repeated.Exactly.Once);

            context.SaveChanges();
            var result = context.Manufacturer.FirstOrDefault(m => m.Id == addManufacturerCommand.Id);
            Assert.Null(result);
        }
    }
}
