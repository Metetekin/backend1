using BullBeez.Core.Entities;

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BullBeez.Core.Repositories
{
    public interface IFeedbackRepository : IRepository<Feedback>
    {
        Task<IEnumerable<Feedback>> GetAll();
        ValueTask<Feedback> GetById(int id);
        Task<IEnumerable<Feedback>> GetAllFilter(Expression<Func<Feedback, bool>> predicate);
    }
}