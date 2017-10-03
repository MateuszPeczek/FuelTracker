using Commands.ManufacturerCommands;
using Common.Interfaces;
using FakeItEasy;
using System;
using System.Linq;
using Xunit;

namespace UnitTests.CommandsHandlers.ManufacturerHandler
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
        public void CommandValid_AddParentOnly_ManufacturerAdded()
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
        public void CommandValid_AddWithChildren_ManufacturerWithModelNamesAdded()
        {
            var modelNames = new string[] { "TestModel1", "TestModel2", "TestModel3" };
            var addManufacturerCommand = new AddManufacturer(expectedManufacturerName, modelNames);

            A.CallTo(() => addManufacturerValidator.Validate(addManufacturerCommand)).DoesNothing();

            addManufacturerHandler.Handle(addManufacturerCommand);

            context.SaveChanges();
            var result = context.Manufacturer.FirstOrDefault(m => m.Id == addManufacturerCommand.Id);

            A.CallTo(() => addManufacturerValidator.Validate(addManufacturerCommand)).MustHaveHappened(Repeated.Exactly.Once);
            Assert.NotNull(result);
            Assert.Equal(addManufacturerCommand.Id, result.Id);
            Assert.Equal(addManufacturerCommand.Name, result.Name);
            Assert.Equal(modelNames.Count(), result.ModelNames.Count);
            Assert.True(StringCollectionsEqual(result.ModelNames.Select(s => s.Name).AsEnumerable(), modelNames));
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

