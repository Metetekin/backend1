using BullBeez.Core.Entities;

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BullBeez.Core.Repositories
{
    public interface IServiceAnswersRepository : IRepository<ServiceAnswers>
    {
        Task<IEnumerable<ServiceAnswers>> GetAll();
        ValueTask<ServiceAnswers> GetById(int id);
        Task<IEnumerable<ServiceAnswers>> GetAllFilter(Expression<Func<ServiceAnswers, bool>> predicate);
    }
}