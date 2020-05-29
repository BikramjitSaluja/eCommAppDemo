using eCommerce.DataAccess.Data.Repository.IRepository;
using eCommerce.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace eCommerce.DataAccess.Data.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly eCommDbContext _dbContext;

        public UnitOfWork(eCommDbContext context)
        {
            _dbContext = context;
            CategoryRepository = new CategoryRepository(_dbContext);
            FrequencyRepository = new FrequencyRepository(_dbContext);
            ServiceRepository = new ServiceRepository(_dbContext);
        }

        public ICategoryRepository CategoryRepository { get; private set; }
        public IFrequencyRepository FrequencyRepository { get; private set; }
        public IServiceRepository ServiceRepository { get; }
        public void Dispose()
        {
            _dbContext.Dispose();
        }

        public void Save()
        {
            _dbContext.SaveChanges();
        }
    }
}
