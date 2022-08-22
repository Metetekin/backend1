using BullBeez.Core.Entities;
using BullBeez.Core.RequestDTO;

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
namespace BullBeez.Core.Repositories
{
    public interface IPackagePaymentRepository : IRepository<PackagePayments>
    {
        Task<IEnumerable<PackagePayments>> GetPostFilterList(FilterRequest request);
        Task<IEnumerable<PackagePayments>> GetUserPayPackageList(int userId);
        Task<IEnumerable<PackagePayments>> GetAll();
        ValueTask<PackagePayments> GetById(int id);
        Task<IEnumerable<PackagePayments>> GetAllFilter(Expression<Func<PackagePayments, bool>> predicate);
    }
}