using eCommerce.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eCommerceApp.ViewModels
{
    public class FrequencyViewModel
    {
        //public int Id { get; set; }
        //public string Name { get; set; }
        //public int FrequencyCount { get; set; }

        //pagination
        public int TotalCount { get; set; }
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public int PagerCount { get; set; }

        public List<Frequency> Frequencies { get; set; }
    }
}
