using BullBeez.Core.Entities;

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BullBeez.Core.Repositories
{
    public interface IAddressRepository : IRepository<Address>
    {
        Task<IEnumerable<Address>> GetAll();
        ValueTask<Address> GetById(int id);
        Task<IEnumerable<Address>> GetAllFilter(Expression<Func<Address, bool>> predicate);
    }
}