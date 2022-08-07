using BullBeez.Core.Entities;

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BullBeez.Core.Repositories
{
    public interface IProjectRepository : IRepository<Project>
    {
        Task<IEnumerable<Project>> GetAll();
        ValueTask<Project> GetById(int id);
        Task<IEnumerable<Project>> GetAllFilter(Expression<Func<Project, bool>> predicate);
    }
}