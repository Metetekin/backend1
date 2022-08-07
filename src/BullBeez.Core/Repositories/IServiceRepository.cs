using BullBeez.Core.Entities;

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BullBeez.Core.Repositories
{
    public interface IServiceRepository : IRepository<Service>
    {
        Task<IEnumerable<Service>> GetAllServiceList();
        Task<IEnumerable<Service>> GetServiceById(int id);
        Task<IEnumerable<Service>> GetAll();
        ValueTask<Service> GetById(int id);
        Task<IEnumerable<Service>> GetAllFilter(Expression<Func<Service, bool>> predicate);
    }
}