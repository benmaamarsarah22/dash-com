using Anade.Data.Abstractions;
using Anade.Domain.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;

namespace Anade.Data.Core
{
    public class UnitOfWork<TContext> : IUnitOfWork<TContext> where TContext : DbContext
    {
        private IDbContextTransaction _transaction;
        private readonly TContext _dbContext;
        private readonly Dictionary<Type, object> _repositories = new Dictionary<Type, object>();

        public UnitOfWork(TContext dbContext)
        {
            _dbContext = dbContext;
        }

        public int SaveChanges()
        {
            return _dbContext.SaveChanges();
        }

        public IRepository<T, TKey> GetRepository<T, TKey>() where T : class, IEntity<TKey>
        {
            if (!_repositories.ContainsKey(typeof(T)))
                _repositories[typeof(T)] = new GenericRepository<T, TKey>(_dbContext);

            return _repositories[typeof(T)] as IRepository<T, TKey>;
        }
        public void AddListOnTransaction<T>(IEnumerable<T> entities) where T : class
        {
            try
            {
                // Begin transaction if not already started
                if (_transaction == null)
                {
                    _transaction = _dbContext.Database.BeginTransaction();
                }

                // Add all entities to the context
                _dbContext.Set<T>().AddRange(entities);

                // Save changes
                _dbContext.SaveChanges();

                // Commit transaction
                _transaction.Commit();

            }
            catch
            {
                // Rollback if any error occurs
                _transaction?.Rollback();
                throw; // Re-throw the exception
            }
        }

        public object Repository<T>()
        {
            throw new NotImplementedException();
        }
    }
}
