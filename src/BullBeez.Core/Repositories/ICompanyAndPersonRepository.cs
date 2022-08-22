using BullBeez.Core.Entities;
using BullBeez.Core.RequestDTO;

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BullBeez.Core.Repositories
{
    public interface ICompanyAndPersonRepository : IRepository<CompanyAndPerson>
    {
        Task<IEnumerable<CompanyAndPerson>> GetAll();
        ValueTask<CompanyAndPerson> GetById(int id);
        ValueTask<CompanyAndPerson> GetByUserName(string UserName);
        Task<IEnumerable<CompanyAndPerson>> GetAllFilter(Expression<Func<CompanyAndPerson, bool>> predicate);
        Task<CompanyAndPerson> GetTestMetot(int id);
        Task<CompanyAndPerson> GetInterestsListByUserId(BaseRequest request);
        Task<IEnumerable<CompanyAndPerson>> SearchUserByFilter();
        Task<CompanyAndPerson> GetCompanyAndPersonAndInterestById(int id);
        Task<CompanyAndPerson> GetCompanyAndPersonAndSkillById(int id);
        Task<CompanyAndPerson> GetCompanyAndPersonAllDetailById(int id);
        Task<CompanyAndPerson> GetCompanyAndPersonPostDetailById(int id);
        Task<CompanyAndPerson> GetCompanyAndPersonAllDetailAndFileById(int id);
        Task<IEnumerable<CompanyAndPerson>> GetAllFilterCompanyTypeAndCompanyLevel(List<int> toUserIdList, List<int> toUserIdList2);
    }
}
