using eCommerce.DataAccess.Data.Repository.IRepository;
using eCommerce.DataAccess.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace eCommerce.DataAccess.Data.Repository
{
    public class FrequencyRepository : Repository<Frequency>, IFrequencyRepository
    {
        private readonly eCommDbContext _dbContext;
      
        public FrequencyRepository(eCommDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<SelectListItem> GetFrequencyListForDropDown()
        {
            return _dbContext.Frequency.Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.Id.ToString()
            });
        }

        public void Update(Frequency frequency)
        {
            var existingRecord = _dbContext.Frequency.FirstOrDefault(c => c.Id == frequency.Id);
            existingRecord.Name = frequency.Name;
            existingRecord.FrequencyCount = frequency.FrequencyCount;

            _dbContext.SaveChanges();
        }
    }
}
