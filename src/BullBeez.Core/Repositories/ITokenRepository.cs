using BullBeez.Core.Entities;

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BullBeez.Core.Repositories
{
    public interface ITokenRepository : IRepository<Token>
    {
        Task<IEnumerable<Token>> GetAll();
        ValueTask<Token> GetById(int id);
        Task<IEnumerable<Token>> GetAllFilter(Expression<Func<Token, bool>> predicate);
    }
}