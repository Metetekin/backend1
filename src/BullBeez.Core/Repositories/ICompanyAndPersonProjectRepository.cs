using BullBeez.Core.Entities;

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BullBeez.Core.Repositories
{
    public interface ICompanyAndPersonProjectRepository : IRepository<CompanyAndPersonProject>
    {
        Task<IEnumerable<CompanyAndPersonProject>> GetAll();
        ValueTask<CompanyAndPersonProject> GetById(int id);
        Task<IEnumerable<CompanyAndPersonProject>> GetAllFilter(Expression<Func<CompanyAndPersonProject, bool>> predicate);
    }
}