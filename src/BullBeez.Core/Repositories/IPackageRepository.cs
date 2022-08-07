using BullBeez.Core.Entities;
using BullBeez.Core.RequestDTO;

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
namespace BullBeez.Core.Repositories
{
    public interface IPackageRepository : IRepository<Package>
    {
        Task<IEnumerable<Package>> GetPostFilterList(FilterRequest request);
        Task<IEnumerable<Package>> GetAll();
        ValueTask<Package> GetById(int id);
        Task<IEnumerable<Package>> GetAllFilter(Expression<Func<Package, bool>> predicate);
    }
}
