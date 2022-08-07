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
            return await base.Find(x => x.RowStatu == EnumRowStatusType.Active);
        }

        public async Task<IEnumerable<UserPosts>> GetAllFilter(Expression<Func<UserPosts, bool>> predicate)
        {
            var response = await base.Find(predicate);
            response = response.Where(x => x.RowStatu == EnumRowStatusType.Active);
            return response;
        }

        public async ValueTask<UserPosts> GetById(int id)
        {
            return await base.SingleOrDefaultAsync(x => x.Id == id && x.RowStatu == EnumRowStatusType.Active);
        }


        public async Task<IEnumerable<UserPosts>> GetPostsAndCompanyAndPersonData(string searchValue)
        {
            return await BullBeezDBContext.UserPosts.Where(x => x.PostText.Contains(searchValue == null ? "" : searchValue) && x.RowStatu == EnumRowStatusType.Active)
                .Include(a => a.CompanyAndPerson).ToListAsync();
        }
    }
}
