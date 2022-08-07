using BullBeez.Core.Entities;

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BullBeez.Core.Repositories
{
    public interface IEducationRepository : IRepository<Education>
    {
        Task<IEnumerable<Education>> GetAll();
        ValueTask<Education> GetById(int id);
        Task<IEnumerable<Education>> GetAllFilter(Expression<Func<Education, bool>> predicate);
    }
}