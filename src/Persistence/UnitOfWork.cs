namespace Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationContext context;

        public UnitOfWork(ApplicationContext context)
        {
            this.context = context;
        }

        public ApplicationContext Context { get { return context; } }

        public int SaveChanges()
        {
            return context.SaveChanges();
        }
    }

    public interface IUnitOfWork
    {
        ApplicationContext Context { get; }
        int SaveChanges();
    }
}
