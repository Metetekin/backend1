using BullBeez.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace BullBeez.Core.Repositories
{
    public interface IUserPostsRepository : IRepository<UserPosts>
    {
        Task<IEnumerable<UserPosts>> GetAll();
        ValueTask<UserPosts> GetById(int id);
        ValueTask<UserPosts> GetDetailedById(int id);
        Task<IEnumerable<UserPosts>> GetAllFilter(Expression<Func<UserPosts, bool>> predicate);
        Task<IEnumerable<UserPosts>> GetPostsAndCompanyAndPersonData(string searchValue);
        Task<IEnumerable<UserPosts>> GetByUserId(int UserId);
        Task<IEnumerable<UserPosts>> GetByLastHours(int hoursLimit);
        Task<IEnumerable<UserPosts>> GetLastN(int fromId, int count, int UserId);
        Task<IEnumerable<UserPosts>> GetSponsored();
        Task<IEnumerable<UserPosts>> GetLastByUserId(int UserId);
        Task<IEnumerable<UserPosts>> GetPostByFollowingCompanyAndPerson(int[] companyAndPersonId);
    }
}
