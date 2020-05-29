using eCommerce.DataAccess.Data.Repository.IRepository;
using eCommerce.DataAccess.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace eCommerce.DataAccess.Data.Repository
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        private readonly eCommDbContext _dbContext;

        public CategoryRepository(eCommDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<SelectListItem> GetCategoryListForDropDown()
        {
            return _dbContext.Category.Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.Id.ToString()
            });
        }

        public void Update(Category category)
        {
            var existingRecord = _dbContext.Category.FirstOrDefault(c => c.Id == category.Id);
            existingRecord.Name = category.Name;
            existingRecord.DisplayOrder = category.DisplayOrder;

            _dbContext.SaveChanges();
        }
    }
}
