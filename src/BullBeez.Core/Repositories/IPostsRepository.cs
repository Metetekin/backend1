using BullBeez.Core.Entities;
using BullBeez.Core.RequestDTO;

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BullBeez.Core.Repositories
{
    public interface IPostsRepository : IRepository<Posts>
    {
        Task<IEnumerable<Posts>> GetPostFilterList(FilterRequest request);
        Task<IEnumerable<Posts>> GetAll();
        ValueTask<Posts> GetById(int id);
        Task<IEnumerable<Posts>> GetAllFilter(Expression<Func<Posts, bool>> predicate);
    }
}