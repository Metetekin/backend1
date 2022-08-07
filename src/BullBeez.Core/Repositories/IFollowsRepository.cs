using BullBeez.Core.Entities;

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BullBeez.Core.Repositories
{
    public interface IFollowsRepository : IRepository<Follows>
    {
        Task<IEnumerable<Follows>> GetAll();
        ValueTask<Follows> GetById(int id);
        Task<IEnumerable<Follows>> GetAllFilter(Expression<Func<Follows, bool>> predicate);
        Task<IEnumerable<Follows>> GetAllFilterAndUserWorker(Expression<Func<Follows, bool>> predicate);
        Task<IEnumerable<Follows>> GetAllFilterAndUserWorkerWaiting(Expression<Func<Follows, bool>> predicate);
        Task<IEnumerable<Follows>> GetAllFilterAndUserFollow(Expression<Func<Follows, bool>> predicate);
    }
}