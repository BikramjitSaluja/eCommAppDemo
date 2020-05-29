using System;
using System.Collections.Generic;
using System.Text;

namespace eCommerce.DataAccess.Data.Repository.IRepository
{
    // Unit of work represents what you do in a single transaction or single batch.
    // Unit of work will have access to all your repositories as well as a safe method (that will push all the changes to the database)
    public interface IUnitOfWork : IDisposable
    {
        ICategoryRepository CategoryRepository { get; }
        IFrequencyRepository FrequencyRepository { get; }
        IServiceRepository ServiceRepository { get; }
        void Save();
    }
}
