using BullBeez.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BullBeez.Core.Repositories
{
    public interface IPostCommentsRepository : IRepository<PostComments>
    {
        Task<IEnumerable<PostComments>> GetAll();
        ValueTask<PostComments> GetById(int id);
        Task<IEnumerable<PostComments>> GetAllFilter(Expression<Func<PostComments, bool>> predicate);
    }
}
