using Microsoft.EntityFrameworkCore.Storage;
using Persistence;
using System;
using System.Collections.Generic;
using System.Text;

namespace Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private ApplicationContext context;

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
