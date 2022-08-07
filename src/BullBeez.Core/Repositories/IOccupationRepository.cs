using BullBeez.Core.Entities;

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BullBeez.Core.Repositories
{
    public interface IOccupationRepository : IRepository<Occupation>
    {
        Task<IEnumerable<Occupation>> GetAll();
        ValueTask<Occupation> GetById(int id);
        Task<IEnumerable<Occupation>> GetAllFilter(Expression<Func<Occupation, bool>> predicate);
    }
}