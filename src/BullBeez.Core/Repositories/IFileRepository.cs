using BullBeez.Core.Entities;

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
namespace BullBeez.Core.Repositories
{
    public interface IFileRepository : IRepository<FileData>
    {
        Task<IEnumerable<FileData>> GetAll();
        ValueTask<FileData> GetById(int id);
        Task<IEnumerable<FileData>> GetAllFilter(Expression<Func<FileData, bool>> predicate);

    }
}
