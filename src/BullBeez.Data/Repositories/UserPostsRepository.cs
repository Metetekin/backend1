using BullBeez.Core.Entities;
using BullBeez.Core.Enums;
using BullBeez.Core.Helper;
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
    public class UserPostsRepository : Repository<UserPosts>, IUserPostsRepository
    {
        public UserPostsRepository(BullBeezDBContext context)
            : base(context)
        { }

        private BullBeezDBContext BullBeezDBContext
        {
            get { return Context as BullBeezDBContext; }
        }

        public async Task<IEnumerable<UserPosts>> GetAll()
        {
            return await BullBeezDBContext.UserPosts.Where(x => x.RowStatu == EnumRowStatusType.Active).
                Include(a => a.CompanyAndPerson).ToListAsync();
        }

        public async Task<IEnumerable<UserPosts>> GetAllFilter(Expression<Func<UserPosts, bool>> predicate)
        {
            var response = await base.Find(predicate);
            response = response.Where(x => x.RowStatu == EnumRowStatusType.Active);
            return response;
        }

        public async Task<IEnumerable<UserPosts>> GetByUserId(int UserId)
        {
            var fromDate = DateTime.Now.AddHours(-1 * 360);

            var response = BullBeezDBContext.UserPosts
                .Where(x => x.RowStatu == EnumRowStatusType.Active)
                .Where(x => x.CreatedDate >= fromDate)
                .Include(a => a.CompanyAndPerson);

            return await response
                .Where(x => x.CompanyAndPerson.Id == UserId                  )
                .Where(x => x.RowStatu            == EnumRowStatusType.Active)
                .ToListAsync();
        }

        public async Task<IEnumerable<UserPosts>> GetByLastHours(int hoursLimit)
        {

            var fromDate = DateTime.Now.AddHours(-1* hoursLimit);

            var response = BullBeezDBContext.UserPosts
                .Include(a => a.CompanyAndPerson)
                .Where(x => x.IsUpgradedToBoard == true)
                .Where(x => x.CreatedDate >= fromDate)
                //.Where(x => x.PostTopics.DeserializeObject<List<string>>().Intersect(RequestTopicList).Count() > 0)
                .Where(x =>    x.RowStatu == EnumRowStatusType.Active );

            return await response.ToListAsync();
        }

        public async Task<IEnumerable<UserPosts>> GetLastN(int fromId, int count, int UserId)
        {
            var response = BullBeezDBContext.UserPosts
                .Where(x => x.RowStatu == EnumRowStatusType.Active)
                .Include(a => a.CompanyAndPerson)
                .Where(x => x.CompanyAndPerson.Id == UserId)
                .OrderByDescending(x => x.CreatedDate)
                .Skip(fromId)
                .Take(count);

            return await response.ToListAsync();
        }

        public async Task<IEnumerable<UserPosts>> GetSponsored()
        {

            var response = BullBeezDBContext.UserPosts
                .Include(a => a.CompanyAndPerson)
                .Where(x => x.IsSponsoredPost == true)
                .Where(x => x.RowStatu == EnumRowStatusType.Active)
                .OrderByDescending(x => x.CreatedDate);

            return await response.ToListAsync();
        }

        public async Task<IEnumerable<UserPosts>> GetLastByUserId(int UserId)
        {
            var response = BullBeezDBContext.UserPosts
                .Where(x => x.RowStatu == EnumRowStatusType.Active)
                .Where(x => x.CompanyAndPerson.Id == UserId)
                .OrderByDescending(x => x.CreatedDate)
                .Take(1);

            return await response.ToListAsync();
        }

        public async ValueTask<UserPosts> GetById(int id)
        {
            return await BullBeezDBContext.UserPosts.Include(a => a.CompanyAndPerson).SingleOrDefaultAsync(x => x.Id == id && x.RowStatu == EnumRowStatusType.Active);
        }

        public async ValueTask<UserPosts> GetDetailedById(int id)
        {
            var response = BullBeezDBContext.UserPosts
                .Where(x => x.Id == id)
                .Where(x => x.RowStatu == EnumRowStatusType.Active)
                .Include(x => x.CompanyAndPerson)
                .Take(1);

            return await response.SingleOrDefaultAsync(); 
        }

        public async Task<IEnumerable<UserPosts>> GetPostsAndCompanyAndPersonData(string searchValue)
        {
            return await BullBeezDBContext.UserPosts.Where(x => x.PostText.Contains(searchValue == null ? "" : searchValue) && x.RowStatu == EnumRowStatusType.Active)
                .Include(a => a.CompanyAndPerson).ToListAsync();
        }
        public async Task<IEnumerable<UserPosts>> GetPostByFollowingCompanyAndPerson(int[] companyAndPersonId)
        {

            return await BullBeezDBContext.UserPosts.Where(x => companyAndPersonId.Contains(x.CompanyAndPerson.Id) && x.RowStatu == EnumRowStatusType.Active)
                .Include(a => a.CompanyAndPerson).ToListAsync();
        }
    }
}
