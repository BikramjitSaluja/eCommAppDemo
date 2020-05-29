using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Mvc.Rendering;
using eCommerce.DataAccess.Data.Repository.IRepository;
using eCommerce.DataAccess.Models;

namespace eCommerce.DataAccess.Data.Repository
{
    public class ServiceRepository : Repository<Service> , IServiceRepository
    {
        private readonly eCommDbContext _db;

        public ServiceRepository(eCommDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Service service)
        {
            var objFromDb = _db.Service.FirstOrDefault(s => s.Id == service.Id);

            objFromDb.Name = service.Name;
            objFromDb.LongDesc = service.LongDesc;
            objFromDb.Price = service.Price;
            objFromDb.ImageUrl = service.ImageUrl;
            objFromDb.FrequencyId = service.FrequencyId;
            objFromDb.CategoryId = service.CategoryId;

            _db.SaveChanges();

        }
    }
}
