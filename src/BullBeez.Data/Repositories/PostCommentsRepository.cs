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
    public class PostCommentsRepository : Repository<PostComments>, IPostCommentsRepository
    {
        public PostCommentsRepository(BullBeezDBContext context)
            : base(context)
        { }

        private BullBeezDBContext BullBeezDBContext
        {
            get { return Context as BullBeezDBContext; }
        }

        public async Task<IEnumerable<PostComments>> GetAll()
        {
            return await base.Find(x => x.RowStatu == EnumRowStatusType.Active);
        }

        public async Task<IEnumerable<PostComments>> GetAllFilter(Expression<Func<PostComments, bool>> predicate)
        {
            var result = BullBeezDBContext.PostComments.
                Include(a => a.UserPosts).
                Include(a => a.CompanyAndPerson);
                
            return await result.Where(x => x.RowStatu == EnumRowStatusType.Active).
                Where(predicate).
                ToListAsync();
        }

        public async Task<IEnumerable<PostComments>> GetByOffset(int PostId, int StartIdx, int Count)
        {
            var result = BullBeezDBContext.PostComments.
                Include(a => a.UserPosts).
                Include(a => a.CompanyAndPerson);

            return await result
                .Where(x => x.UserPosts.Id == PostId)
                .Where(x => x.RowStatu == EnumRowStatusType.Active)
                .OrderBy(a => a.InsertedDate)
                .Skip(StartIdx)
                .Take(Count)
                .ToListAsync();
        }

        public async ValueTask<PostComments> GetById(int id)
        {
            return await BullBeezDBContext.PostComments.Include(a => a.CompanyAndPerson).Include(a => a.UserPosts).SingleOrDefaultAsync(x => x.Id == id && x.RowStatu == EnumRowStatusType.Active);
        }
    }
}
