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
    public class PostsRepository : Repository<Posts>, IPostsRepository
    {
        public PostsRepository(BullBeezDBContext context)
            : base(context)
        { }



        private BullBeezDBContext BullBeezDBContext
        {
            get { return Context as BullBeezDBContext; }
        }

        public async Task<IEnumerable<Posts>> GetPostFilterList(FilterRequest request)
        {
            return await BullBeezDBContext.Posts.Where(x=> x.RowStatu == EnumRowStatusType.Active).OrderByDescending(x=> x.Id).Skip(request.Offset).Take(request.Limit).ToListAsync();
        }

        public async Task<IEnumerable<Posts>> GetAll()
        {
            return await base.Find(x => x.RowStatu == EnumRowStatusType.Active);
        }

        public async Task<IEnumerable<Posts>> GetAllFilter(Expression<Func<Posts, bool>> predicate)
        {
            var response = await base.Find(predicate);
            response = response.Where(x => x.RowStatu == EnumRowStatusType.Active);
            return response;
        }

        public async ValueTask<Posts> GetById(int id)
        {
            return await base.SingleOrDefaultAsync(x => x.Id == id && x.RowStatu == EnumRowStatusType.Active);
        }

    }
}