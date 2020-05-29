using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;
using eCommerce.DataAccess.Models;

namespace eCommerce.DataAccess.Data.Repository.IRepository
{
    public interface IServiceRepository : IRepository<Service>
    {

        void Update(Service service);
    }
}
