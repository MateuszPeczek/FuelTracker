using Microsoft.EntityFrameworkCore;
using Persistence;

namespace UnitTests.CommandsHandlers
{
    public abstract class BaseHandlersUnitTests
    {
        protected ApplicationContext context;

        protected DbContextOptions<ApplicationContext> GetContextBuilderOptions()
        {
            return new DbContextOptionsBuilder<ApplicationContext>()
                            .UseInMemoryDatabase(databaseName: "TestDatabase")
                            .Options;
        }
    }
}
