using BullBeez.Core.Entities;

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BullBeez.Core.Repositories
{
    public interface IInterestsRepository : IRepository<Interest>
    {
        Task<IEnumerable<Interest>> GetAll();
        ValueTask<Interest> GetById(int id);
        Task<IEnumerable<Interest>> GetAllFilter(Expression<Func<Interest, bool>> predicate);
    }
}