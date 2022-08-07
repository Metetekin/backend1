using BullBeez.Core.Entities;
using BullBeez.Core.RequestDTO;

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BullBeez.Core.Repositories
{
    public interface IPopUpRepository : IRepository<PopUp>
    {
        Task<IEnumerable<PopUp>> GetPostFilterList(FilterRequest request);
        Task<IEnumerable<PopUp>> GetAll();
        ValueTask<PopUp> GetById(int id);
        Task<IEnumerable<PopUp>> GetAllFilter(Expression<Func<PopUp, bool>> predicate);
    }
}