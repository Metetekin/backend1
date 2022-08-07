using BullBeez.Core.Entities;

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BullBeez.Core.Repositories
{
    public interface IMailApprovedCodeRepository : IRepository<MailApprovedCode>
    {
        Task<IEnumerable<MailApprovedCode>> GetAll();
        ValueTask<MailApprovedCode> GetById(int id);
        Task<IEnumerable<MailApprovedCode>> GetAllFilter(Expression<Func<MailApprovedCode, bool>> predicate);
    }
}