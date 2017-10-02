using Commands.ManufacturerCommands;
using Common.Interfaces;
using FakeItEasy;
using System;
using System.Linq;
using Xunit;

namespace UnitTests.CommandsHandlers.ManufacturerCommands
{
    public class DeleteManufacturerCommandHandlerTests : BaseHandlersUnitTests
    {
        protected readonly ICommandValidator<DeleteManufacturer> deleteManufacturerValidator;
        protected readonly ICommandHandler<DeleteManufacturer> deleteManufacturerHandler;

        public DeleteManufacturerCommandHandlerTests()
        {
            deleteManufacturerValidator = A.Fake<ICommandValidator<DeleteManufacturer>>();
            deleteManufacturerHandler = new DeleteManufacturerHandler(unitOfWork, deleteManufacturerValidator);
        }

        [Fact]
        public void CommandValid_ManufacturerDeleted()
        {
            var manufacturerId = InsertManufacturerToDatabase();
            var deleteManufacturerCommand = new DeleteManufacturer(manufacturerId);

            A.CallTo(() => deleteManufacturerValidator.Validate(deleteManufacturerCommand)).DoesNothing();

            deleteManufacturerHandler.Handle(deleteManufacturerCommand);

            context.SaveChanges();
            var result = context.Manufacturer.FirstOrDefault(m => m.Id == deleteManufacturerCommand.Id);

            A.CallTo(() => deleteManufacturerValidator.Validate(deleteManufacturerCommand)).MustHaveHappened(Repeated.Exactly.Once);
            Assert.Null(result);
        }

        [Fact]
        public void ValidatorThrowsException_HandlerThrowsException()
        {
            var manufacturerId = InsertManufacturerToDatabase();
            var deleteManufacturerCommand = new DeleteManufacturer(manufacturerId);

            A.CallTo(() => deleteManufacturerValidator.Validate(deleteManufacturerCommand)).Throws<Exception>();

            Assert.ThrowsAny<Exception>(() => deleteManufacturerHandler.Handle(deleteManufacturerCommand));
            A.CallTo(() => deleteManufacturerValidator.Validate(deleteManufacturerCommand)).MustHaveHappened(Repeated.Exactly.Once);

            context.SaveChanges();
            var result = context.Manufacturer.FirstOrDefault(m => m.Id == deleteManufacturerCommand.Id);
            Assert.NotNull(result);
            Assert.Equal(result.Id, manufacturerId);
            Assert.Equal(result.Name, expectedManufacturerName);
        }
    }
}
