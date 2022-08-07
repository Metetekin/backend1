using BullBeez.Core.Entities;
using BullBeez.Core.RequestDTO;

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BullBeez.Core.Repositories
{
    public interface INotificationRepository : IRepository<Notification>
    {
        Task<IEnumerable<Notification>> GetListNotificationByUserId(BaseRequest request);
        Task<IEnumerable<Notification>> GetAll();
        ValueTask<Notification> GetById(int id);
        Task<IEnumerable<Notification>> GetAllFilter(Expression<Func<Notification, bool>> predicate);
    }
}