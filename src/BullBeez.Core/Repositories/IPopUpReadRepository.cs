using BullBeez.Core.Entities;
using BullBeez.Core.RequestDTO;

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BullBeez.Core.Repositories
{
    public interface IPopUpReadRepository : IRepository<PopUpIsRead>
    {
        Task<IEnumerable<PopUpIsRead>> GetPostFilterList(FilterRequest request);
        Task<IEnumerable<PopUpIsRead>> GetAll();
        ValueTask<PopUpIsRead> GetById(int id);
        Task<IEnumerable<PopUpIsRead>> GetAllFilter(Expression<Func<PopUpIsRead, bool>> predicate);
    }
}