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
    public class FollowsRepository : Repository<Follows>, IFollowsRepository
    {
        public FollowsRepository(BullBeezDBContext context)
            : base(context)
        { }



        private BullBeezDBContext BullBeezDBContext
        {
            get { return Context as BullBeezDBContext; }
        }

        public async Task<IEnumerable<Follows>> GetAll()
        {
            return await base.Find(x => x.RowStatu == EnumRowStatusType.Active);
        }

        public async Task<IEnumerable<Follows>> GetAllFilter(Expression<Func<Follows, bool>> predicate)
        {
            var response = await base.Find(predicate);
            response = response.Where(x=> x.RowStatu == EnumRowStatusType.Active);
            return response;
        }

        public async Task<IEnumerable<Follows>> GetAllFilterAndUserFollow(Expression<Func<Follows, bool>> predicate)
        {
            var response = await base.Find(predicate);
            response = response.Where(x => x.RowStatu == EnumRowStatusType.Active);

            var IdList = response.Select(s => s.ToUserId).ToArray();

            var responseList = await BullBeezDBContext.Follows.Where(x => IdList.Contains(x.ToUserId) && x.FollowType == EnumFollowType.Follows)
                .Include(a => a.CompanyAndPerson).ThenInclude(a => a.CompanyAndPersonOccupation).ThenInclude(a => a.Occupation)
                .Include(a => a.CompanyAndPerson).ToListAsync();
            
            return (IEnumerable<Follows>)responseList;
        }

        public async Task<IEnumerable<Follows>> GetAllFilterAndUserWorker(Expression<Func<Follows, bool>> predicate)
        {
            var response = await base.Find(predicate);
            response = response.Where(x => x.RowStatu == EnumRowStatusType.Active);

            var IdList = response.Select(s => s.ToUserId).ToArray();

            var responseList = await BullBeezDBContext.Follows.Where(x => IdList.Contains(x.ToUserId) && x.FollowType == EnumFollowType.Worker && x.WorkerFollowType == EnumWorkerFollowType.Approved && x.RowStatu == EnumRowStatusType.Active)
                .Include(a => a.CompanyAndPerson).ThenInclude(a => a.CompanyAndPersonOccupation).ThenInclude(a => a.Occupation)
                .Include(a => a.CompanyAndPerson).ToListAsync();

            return (IEnumerable<Follows>)responseList;
        }

        public async Task<IEnumerable<Follows>> GetAllFilterAndUserWorkerWaiting(Expression<Func<Follows, bool>> predicate)
        {
            var response = await base.Find(predicate);
            response = response.Where(x => x.RowStatu == EnumRowStatusType.Active);

            var IdList = response.Select(s => s.ToUserId).ToArray();

            var responseList = await BullBeezDBContext.Follows.Where(x => IdList.Contains(x.ToUserId) && x.FollowType == EnumFollowType.Worker && x.RowStatu == EnumRowStatusType.Active)
                .Include(a => a.CompanyAndPerson).ThenInclude(a => a.CompanyAndPersonOccupation).ThenInclude(a => a.Occupation)
                .Include(a => a.CompanyAndPerson).ToListAsync();

            return (IEnumerable<Follows>)responseList;
        }

        public async ValueTask<Follows> GetById(int id)
        {
            return await base.SingleOrDefaultAsync(x=> x.Id == id && x.RowStatu == EnumRowStatusType.Active);
        }

       

    }
}