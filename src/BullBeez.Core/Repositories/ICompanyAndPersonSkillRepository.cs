using BullBeez.Core.Entities;

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BullBeez.Core.Repositories
{
    public interface ICompanyAndPersonSkillRepository : IRepository<CompanyAndPersonSkills>
    {
        Task<IEnumerable<CompanyAndPersonSkills>> GetAll();
        ValueTask<CompanyAndPersonSkills> GetById(int id);
        Task<IEnumerable<CompanyAndPersonSkills>> GetAllFilter(Expression<Func<CompanyAndPersonSkills, bool>> predicate);
    }
}