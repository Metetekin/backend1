using BullBeez.Core.Entities;

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BullBeez.Core.Repositories
{
    public interface IRequestHistoryRepository : IRepository<RequestHistory>
    {
        Task<IEnumerable<RequestHistory>> GetAll();
        ValueTask<RequestHistory> GetById(int id);
        Task<IEnumerable<RequestHistory>> GetAllFilter(Expression<Func<RequestHistory, bool>> predicate);
    }
}
