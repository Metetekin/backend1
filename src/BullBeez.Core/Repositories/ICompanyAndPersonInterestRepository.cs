using BullBeez.Core.Entities;

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BullBeez.Core.Repositories
{
    public interface ICompanyAndPersonInterestRepository : IRepository<CompanyAndPersonInterest>
    {
        Task<IEnumerable<CompanyAndPersonInterest>> GetAll();
        ValueTask<CompanyAndPersonInterest> GetById(int id);
        Task<IEnumerable<CompanyAndPersonInterest>> GetAllFilter(Expression<Func<CompanyAndPersonInterest, bool>> predicate);
    }
}