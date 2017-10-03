using Commands.ModelCommands;
using Common.Interfaces;
using FakeItEasy;
using System;
using System.Linq;
using Xunit;

namespace UnitTests.CommandsHandlers.ModelNameHandler
{
    public class AddModelNameCommandHandlerTests : BaseHandlersUnitTests
    {
        protected readonly ICommandValidator<AddModelName> addModelNameValidator;
        protected readonly ICommandHandler<AddModelName> addModelNameHandler;

        public AddModelNameCommandHandlerTests()
        {
            addModelNameValidator = A.Fake<ICommandValidator<AddModelName>>();
            addModelNameHandler = new AddModelNameHandler(unitOfWork, addModelNameValidator);
        }

        [Fact]
        public void CommandValid_ModelNameAdded()
        {
            var manufacturerId = InsertManufacturerToDatabase();
            var addModelNameCommand = new AddModelName(manufacturerId, expectedModelName);

            A.CallTo(() => addModelNameValidator.Validate(addModelNameCommand)).DoesNothing();

            addModelNameHandler.Handle(addModelNameCommand);
            context.SaveChanges();

            var result = context.ModelName.FirstOrDefault(m => m.Id == addModelNameCommand.Id);

            A.CallTo(() => addModelNameValidator.Validate(addModelNameCommand)).MustHaveHappened(Repeated.Exactly.Once);
            Assert.NotNull(result);
            Assert.Equal(addModelNameCommand.Id, result.Id);
            Assert.Equal(expectedModelName, result.Name);
            Assert.Equal(manufacturerId, result.ManufacturerId);
        }

        [Fact]
        public void ValidatorThrowsException_HandlerThrowsException()
        {
            var manufacturerId = InsertManufacturerToDatabase();
            var addModelNameCommand = new AddModelName(manufacturerId, expectedModelName);

            A.CallTo(() => addModelNameValidator.Validate(addModelNameCommand)).Throws<Exception>();

            Assert.ThrowsAny<Exception>(() => addModelNameHandler.Handle(addModelNameCommand));

            context.SaveChanges();
            var result = context.ModelName.FirstOrDefault(m => m.Id == addModelNameCommand.Id);

            A.CallTo(() => addModelNameValidator.Validate(addModelNameCommand)).MustHaveHappened(Repeated.Exactly.Once);
            Assert.Null(result);
        }
    }
}
