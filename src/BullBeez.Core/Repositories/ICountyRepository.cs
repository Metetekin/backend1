using BullBeez.Core.Entities;

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BullBeez.Core.Repositories
{
    public interface ICountyRepository : IRepository<County>
    {
        Task<IEnumerable<County>> GetAll();
        ValueTask<County> GetById(int id);
        Task<IEnumerable<County>> GetAllFilter(Expression<Func<County, bool>> predicate);
    }
}