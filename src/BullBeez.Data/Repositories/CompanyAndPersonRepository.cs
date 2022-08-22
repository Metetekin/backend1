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
    public class CompanyAndPersonRepository : Repository<CompanyAndPerson>, ICompanyAndPersonRepository
    {
        public CompanyAndPersonRepository(BullBeezDBContext context)
            : base(context)
        { }

       

        private BullBeezDBContext BullBeezDBContext
        {
            get { return Context as BullBeezDBContext; }
        }


        public Task<CompanyAndPerson> GetTestMetot(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<CompanyAndPerson> GetInterestsListByUserId(BaseRequest request)
        {
            return await BullBeezDBContext.CompanyAndPersons
                .Include(a => a.CompanyAndPersonInterests).ThenInclude(cs => cs.Interest).SingleOrDefaultAsync(x=> x.Id == request.UserId && x.RowStatu == EnumRowStatusType.Active);
        }

        public async Task<IEnumerable<CompanyAndPerson>> SearchUserByFilter()
        {
            return await BullBeezDBContext.CompanyAndPersons
                .Include(a => a.CompanyType)
                .Include(a => a.Address).ThenInclude(cs => cs.County).ThenInclude(cs => cs.City)
                .Include(a => a.CompanyAndPersonOccupation).ThenInclude(cs => cs.Occupation)
                .Include(a => a.CompanyAndPersonInterests).ThenInclude(cs => cs.Interest).Where(x=> x.RowStatu == EnumRowStatusType.Active).ToListAsync();
        }

        public async Task<IEnumerable<CompanyAndPerson>> GetAll()
        {
            return await base.Find(x => x.RowStatu == EnumRowStatusType.Active);
        }

        public async Task<IEnumerable<CompanyAndPerson>> GetAllFilter(Expression<Func<CompanyAndPerson, bool>> predicate)
        {
            var response = await base.Find(predicate);
            response = response.Where(x => x.RowStatu == EnumRowStatusType.Active);


            var IdList = response.Select(s => s.Id).ToArray();

            var responseList = await BullBeezDBContext.CompanyAndPersons.Where(x => IdList.Contains(x.Id))
                .Include(a => a.CompanyAndPersonOccupation).ThenInclude(a => a.Occupation).ToListAsync();

            return (IEnumerable<CompanyAndPerson>)responseList;
        }

        public async ValueTask<CompanyAndPerson> GetById(int id)
        {
            return await BullBeezDBContext.CompanyAndPersons
                .Include(a => a.Tokens.OrderByDescending(x=> x.Id).Take(1)).FirstOrDefaultAsync(x => x.Id == id && x.RowStatu == EnumRowStatusType.Active);
        }

        public async ValueTask<CompanyAndPerson> GetByUserName(string UserName)
        {
            return await base.SingleOrDefaultAsync(x => x.UserName == UserName && x.RowStatu == EnumRowStatusType.Active);
        }

        public async Task<CompanyAndPerson> GetCompanyAndPersonAndInterestById(int id)
        {
            return await BullBeezDBContext.CompanyAndPersons
                .Include(a => a.CompanyAndPersonInterests).ThenInclude(cs => cs.Interest).Where(x => x.RowStatu == EnumRowStatusType.Active && x.Id == id).FirstOrDefaultAsync();
        }

        public async Task<CompanyAndPerson> GetCompanyAndPersonAndSkillById(int id)
        {
            return await BullBeezDBContext.CompanyAndPersons
                .Include(a => a.CompanyAndPersonSkills).ThenInclude(cs => cs.Skill).Where(x => x.RowStatu == EnumRowStatusType.Active && x.Id == id).FirstOrDefaultAsync();
        }

        public async Task<CompanyAndPerson> GetCompanyAndPersonAllDetailById(int id)
        {
            return await BullBeezDBContext.CompanyAndPersons.Where(x => x.RowStatu == EnumRowStatusType.Active && x.Id == id)
                .Include(a => a.CompanyAndPersonOccupation).ThenInclude(cs => cs.Occupation)
                .Include(a => a.Education.Where(d => d.RowStatu == EnumRowStatusType.Active))
                .Include(a=> a.Follows.Where(d => d.RowStatu == EnumRowStatusType.Active))
                .Include(a => a.CompanyType)
                .Include(a => a.CompanyLevel)
                //.Include(a => a.Files.Where(k => k.RowStatu == EnumRowStatusType.Active))
                .Include(a => a.CompanyAndPersonSkills.Where(d => d.RowStatu == EnumRowStatusType.Active)).ThenInclude(cs => cs.Skill)
                .Include(a => a.CompanyAndPersonProject.Where(d=> d.RowStatu == EnumRowStatusType.Active)).ThenInclude(cs => cs.Project).ThenInclude(s=> s.ProjectInterest).ThenInclude(v=> v.Interest)
                .Include(a => a.Address.Where(d => d.RowStatu == EnumRowStatusType.Active)).ThenInclude(cs => cs.County).ThenInclude(s => s.City)
                .Include(a => a.CompanyAndPersonInterests).ThenInclude(cs => cs.Interest).FirstOrDefaultAsync();
        }

        public async Task<CompanyAndPerson> GetCompanyAndPersonAllDetailAndFileById(int id)
        {
            return await BullBeezDBContext.CompanyAndPersons.Where(x => x.RowStatu == EnumRowStatusType.Active && x.Id == id)
                .Include(a => a.CompanyAndPersonOccupation).ThenInclude(cs => cs.Occupation)
                .Include(a => a.Education.Where(d => d.RowStatu == EnumRowStatusType.Active))
                .Include(a => a.Follows.Where(d => d.RowStatu == EnumRowStatusType.Active))
                .Include(a => a.CompanyType)
                .Include(a => a.CompanyLevel)
                .Include(a => a.Files.Where(k => k.RowStatu == EnumRowStatusType.Active))
                .Include(a => a.CompanyAndPersonSkills.Where(d => d.RowStatu == EnumRowStatusType.Active)).ThenInclude(cs => cs.Skill)
                .Include(a => a.CompanyAndPersonProject.Where(d => d.RowStatu == EnumRowStatusType.Active)).ThenInclude(cs => cs.Project).ThenInclude(s => s.ProjectInterest).ThenInclude(v => v.Interest)
                .Include(a => a.Address.Where(d => d.RowStatu == EnumRowStatusType.Active)).ThenInclude(cs => cs.County).ThenInclude(s => s.City)
                .Include(a => a.CompanyAndPersonInterests).ThenInclude(cs => cs.Interest).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<CompanyAndPerson>> GetAllFilterCompanyTypeAndCompanyLevel(List<int> toUserIdList, List<int> toUserIdList2)
        {
            return await BullBeezDBContext.CompanyAndPersons
                .Include(a => a.CompanyAndPersonOccupation).ThenInclude(cs => cs.Occupation)
                .Include(a => a.CompanyType)
                 .Include(a => a.CompanyLevel).Where(x => x.RowStatu == EnumRowStatusType.Active && toUserIdList.Contains(x.Id) || toUserIdList2.Contains(x.Id)).ToListAsync();
        }

        public async Task<CompanyAndPerson> GetCompanyAndPersonPostDetailById(int id)
        {
            return await BullBeezDBContext.CompanyAndPersons.Where(x => x.RowStatu == EnumRowStatusType.Active && x.Id == id)
                .Include(a => a.CompanyAndPersonOccupation).ThenInclude(cs => cs.Occupation)
                .Include(a => a.CompanyType).FirstOrDefaultAsync();
        }
    }
}