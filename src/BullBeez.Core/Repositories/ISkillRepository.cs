using BullBeez.Core.Entities;

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BullBeez.Core.Repositories
{
    public interface ISkillRepository : IRepository<Skill>
    {
        Task<IEnumerable<Skill>> GetAll();
        ValueTask<Skill> GetById(int id);
        Task<IEnumerable<Skill>> GetAllFilter(Expression<Func<Skill, bool>> predicate);
    }
}