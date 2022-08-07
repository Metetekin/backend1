using BullBeez.Core.Entities;
using BullBeez.Core.Enums;
using BullBeez.Core.Repositories;
using BullBeez.Data.Context;

using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BullBeez.Data.Repositories
{
    public class ServiceRepository : Repository<Service>, IServiceRepository
    {
        public ServiceRepository(BullBeezDBContext context)
            : base(context)
        { }



        private BullBeezDBContext BullBeezDBContext
        {
            get { return Context as BullBeezDBContext; }
        }

        public async Task<IEnumerable<Service>> GetAllServiceList()
        {
            return await BullBeezDBContext.Services.Where(x=> x.RowStatu == EnumRowStatusType.Active)
                .Include(a => a.ServiceQuestions).ThenInclude(cs=> cs.Question).ThenInclude(ab=> ab.QuestionOptions)
                .ToListAsync();
        }

        public async Task<IEnumerable<Service>> GetServiceById(int id)
        {
            return await BullBeezDBContext.Services.Where(x => x.RowStatu == EnumRowStatusType.Active && x.Id == id)
                .Include(a => a.ServiceQuestions).ThenInclude(cs => cs.Question).ThenInclude(ab => ab.QuestionOptions)
                .ToListAsync();
        }

        public async Task<IEnumerable<Service>> GetAll()
        {
            return await base.Find(x => x.RowStatu == EnumRowStatusType.Active);
        }

        public async Task<IEnumerable<Service>> GetAllFilter(Expression<Func<Service, bool>> predicate)
        {
            var response = await base.Find(predicate);
            response = response.Where(x => x.RowStatu == EnumRowStatusType.Active);
            return response;
        }

        public async ValueTask<Service> GetById(int id)
        {
            return await base.SingleOrDefaultAsync(x => x.Id == id && x.RowStatu == EnumRowStatusType.Active);
        }
    }
}