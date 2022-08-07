using BullBeez.Core.Entities;

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BullBeez.Core.Repositories
{
    public interface ICompanyTypeRepository : IRepository<CompanyType>
    {
        Task<IEnumerable<CompanyType>> GetAll();
        ValueTask<CompanyType> GetById(int id);
        Task<IEnumerable<CompanyType>> GetAllFilter(Expression<Func<CompanyType, bool>> predicate);
    }
}