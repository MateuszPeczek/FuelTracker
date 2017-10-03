using Commands.ModelCommands;
using Common.Interfaces;
using FakeItEasy;
using System;
using System.Linq;
using Xunit;

namespace UnitTests.CommandsHandlers.ModelNameHandler
{
    public class DeleteModelNameCommandHandlerTests : BaseHandlersUnitTests
    {
        protected readonly ICommandValidator<DeleteModelName> deleteModelNameValidator;
        protected readonly ICommandHandler<DeleteModelName>  deleteModelNameHandler;

        public DeleteModelNameCommandHandlerTests()
        {
            deleteModelNameValidator = A.Fake<ICommandValidator<DeleteModelName>>();
            deleteModelNameHandler = new DeleteModelNameHandler(unitOfWork, deleteModelNameValidator);
        }

        [Fact]
        public void CommandValid_ManufacturerDeleted()
        {
            var manufacturerId = InsertManufacturerToDatabase();
            var modelId = InsertModelToDatabase(manufacturerId);
            var deleteModelNameCommand = new DeleteModelName(manufacturerId,modelId);

            A.CallTo(() => deleteModelNameValidator.Validate(deleteModelNameCommand)).DoesNothing();

            deleteModelNameHandler.Handle(deleteModelNameCommand);

            context.SaveChanges();
            var result = context.ModelName.FirstOrDefault(m => m.Id == deleteModelNameCommand.Id);

            A.CallTo(() => deleteModelNameValidator.Validate(deleteModelNameCommand)).MustHaveHappened(Repeated.Exactly.Once);
            Assert.Null(result);
        }

        [Fact]
        public void ValidatorThrowsException_HandlerThrowsException()
        {
            var manufacturerId = InsertManufacturerToDatabase();
            var modelId = InsertModelToDatabase(manufacturerId);
            var deleteModelNameCommand = new DeleteModelName(manufacturerId,modelId);

            A.CallTo(() => deleteModelNameValidator.Validate(deleteModelNameCommand)).Throws<Exception>();

            Assert.ThrowsAny<Exception>(() => deleteModelNameHandler.Handle(deleteModelNameCommand));
            A.CallTo(() => deleteModelNameValidator.Validate(deleteModelNameCommand)).MustHaveHappened(Repeated.Exactly.Once);

            context.SaveChanges();
            var result = context.ModelName.FirstOrDefault(m => m.Id == deleteModelNameCommand.Id);
            Assert.NotNull(result);
            Assert.Equal(result.Id, modelId);
            Assert.Equal(result.Name, expectedModelName);
        }
    }
}
