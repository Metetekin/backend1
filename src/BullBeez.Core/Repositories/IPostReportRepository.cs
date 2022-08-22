using BullBeez.Core.Entities;

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BullBeez.Core.Repositories
{
    public interface IPostReportRepository : IRepository<PostReport>
    {
        Task<IEnumerable<PostReport>> GetAll();
        ValueTask<PostReport> GetById(int id);
        Task<IEnumerable<PostReport>> GetAllFilter(Expression<Func<PostReport, bool>> predicate);
    }
}