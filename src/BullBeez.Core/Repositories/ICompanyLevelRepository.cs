using BullBeez.Core.Entities;

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BullBeez.Core.Repositories
{
    public interface ICompanyLevelRepository : IRepository<CompanyLevel>
    {
        Task<IEnumerable<CompanyLevel>> GetAll();
        ValueTask<CompanyLevel> GetById(int id);
        Task<IEnumerable<CompanyLevel>> GetAllFilter(Expression<Func<CompanyLevel, bool>> predicate);
    }
}
