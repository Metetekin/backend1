using BullBeez.Core.Entities;
using BullBeez.Core.Enums;
using BullBeez.Core.Repositories;
using BullBeez.Core.RequestDTO;
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
    public class ComponyAndPersonOccupationRepository : Repository<CompanyAndPersonOccupation>, IComponyAndPersonOccupationRepository
    {
        public ComponyAndPersonOccupationRepository(BullBeezDBContext context)
            : base(context)
        { }



        private BullBeezDBContext BullBeezDBContext
        {
            get { return Context as BullBeezDBContext; }
        }

        public async Task<IEnumerable<CompanyAndPersonOccupation>> GetAll()
        {
            return await base.Find(x => x.RowStatu == EnumRowStatusType.Active);
        }

        public async Task<IEnumerable<CompanyAndPersonOccupation>> GetAllFilter(Expression<Func<CompanyAndPersonOccupation, bool>> predicate)
        {
            var response = await base.Find(predicate);
            response = response.Where(x => x.RowStatu == EnumRowStatusType.Active);
            return response;
        }

        public async ValueTask<CompanyAndPersonOccupation> GetById(int id)
        {
            return await base.SingleOrDefaultAsync(x => x.ComponyAndPersonId == id && x.RowStatu == EnumRowStatusType.Active);
        }
    }
}