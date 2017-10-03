using Commands.ModelCommands;
using Common.Interfaces;
using FakeItEasy;
using System;
using System.Linq;
using Xunit;

namespace UnitTests.CommandsHandlers.ModelNameHandler
{
    public class UpdateModelNameCommandHndlerTests : BaseHandlersUnitTests
    {
        protected readonly ICommandValidator<UpdateModelName> updateModelNameValidator;
        protected readonly ICommandHandler<UpdateModelName> updateModelNameHandler;

        public UpdateModelNameCommandHndlerTests()
        {
            updateModelNameValidator = A.Fake<ICommandValidator<UpdateModelName>>();
            updateModelNameHandler = new UpdateModelNameHandler(unitOfWork, updateModelNameValidator);
        }

        [Fact]
        public void CommandValid_ModelNameUpdated()
        {
            var manufacturerId = InsertManufacturerToDatabase();
            var modelId = InsertModelToDatabase();
            var updateModelNameCommand = new UpdateModelName(modelId, manufacturerId, "UpdatedName");

            A.CallTo(() => updateModelNameValidator.Validate(updateModelNameCommand)).DoesNothing();

            updateModelNameHandler.Handle(updateModelNameCommand);

            context.SaveChanges();
            var result = context.ModelName.FirstOrDefault(m => m.Id == updateModelNameCommand.Id);

            A.CallTo(() => updateModelNameValidator.Validate(updateModelNameCommand)).MustHaveHappened(Repeated.Exactly.Once);
            Assert.NotNull(result);
            Assert.Equal(result.Id, updateModelNameCommand.Id);
            Assert.Equal(result.Name, updateModelNameCommand.Name);
        }

        [Fact]
        public void ValidatorThrowsException_HandlerThrowsException()
        {
            var manufacturerId = InsertManufacturerToDatabase();
            var modelId = InsertModelToDatabase();
            var updateModelNameCommand = new UpdateModelName(modelId, manufacturerId, "UpdatedName");

            A.CallTo(() => updateModelNameValidator.Validate(updateModelNameCommand)).Throws<Exception>();

            Assert.ThrowsAny<Exception>(() => updateModelNameHandler.Handle(updateModelNameCommand));
            A.CallTo(() => updateModelNameValidator.Validate(updateModelNameCommand)).MustHaveHappened(Repeated.Exactly.Once);

            context.SaveChanges();
            var result = context.ModelName.FirstOrDefault(m => m.Id == updateModelNameCommand.Id);
            Assert.NotNull(result);
            Assert.Equal(modelId, result.Id);
            Assert.Equal(expectedModelName, result.Name);
        }
    }
}
