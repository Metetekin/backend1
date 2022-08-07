using BullBeez.Core.Entities;

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BullBeez.Core.Repositories
{
    public interface IComponyAndPersonOccupationRepository : IRepository<CompanyAndPersonOccupation>
    {
        Task<IEnumerable<CompanyAndPersonOccupation>> GetAll();
        ValueTask<CompanyAndPersonOccupation> GetById(int id);
        Task<IEnumerable<CompanyAndPersonOccupation>> GetAllFilter(Expression<Func<CompanyAndPersonOccupation, bool>> predicate);
    }
}