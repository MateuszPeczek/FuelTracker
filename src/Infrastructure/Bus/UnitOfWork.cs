using Microsoft.EntityFrameworkCore.Storage;
using Persistence;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Bus
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationContext context;
        private IDbContextTransaction currentTransaction;

        public UnitOfWork(ApplicationContext context)
        {
            this.context = context;
            currentTransaction = null;
        }

        public IDbContextTransaction BeginTransaction()
        {
            currentTransaction = context.Database.BeginTransaction();
            return currentTransaction;
        }

        public void Commit()
        {
            if (currentTransaction != null)
            {
                currentTransaction.Commit();
            }
            else
            {
                throw new ArgumentNullException("Transaction not initialized");
            }
        }

        public void Rollback()
        {
            if (currentTransaction != null)
            {
                currentTransaction.Rollback();
            }
            else
            {
                throw new ArgumentNullException("Transaction not initialized");
            }
        }

        public int SaveChanges()
        {
            return context.SaveChanges();
        }
    }

    public interface IUnitOfWork
    {
        int SaveChanges();
        IDbContextTransaction BeginTransaction();
        void Commit();
        void Rollback();
    }
}
