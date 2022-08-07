using BullBeez.Core.Entities;

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BullBeez.Core.Repositories
{
    public interface ICityRepository : IRepository<City>
    {
        Task<IEnumerable<City>> GetAll();
        ValueTask<City> GetById(int id);
        Task<IEnumerable<City>> GetAllFilter(Expression<Func<City, bool>> predicate);
    }
}