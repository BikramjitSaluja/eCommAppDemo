using eCommerce.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eCommerceApp.ViewModels
{
    public class CategoryViewModel
    {
        //public int Id { get; set; }
        //public string Name { get; set; }
        //public int DisplayOrder { get; set; }

        //pagination
        public int TotalCount { get; set; }
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public int PagerCount { get; set; }

        public List<Category> Categories { get; set; }
    }
}
