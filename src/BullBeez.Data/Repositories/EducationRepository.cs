﻿using BullBeez.Core.Entities;
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
    public class EducationRepository : Repository<Education>, IEducationRepository
    {
        public EducationRepository(BullBeezDBContext context)
            : base(context)
        { }



        private BullBeezDBContext BullBeezDBContext
        {
            get { return Context as BullBeezDBContext; }
        }

        public async Task<IEnumerable<Education>> GetAll()
        {
            return await base.Find(x => x.RowStatu == EnumRowStatusType.Active);
        }

        public async Task<IEnumerable<Education>> GetAllFilter(Expression<Func<Education, bool>> predicate)
        {
            var response = await base.Find(predicate);
            response = response.Where(x => x.RowStatu == EnumRowStatusType.Active);
            return response;
        }

        public async ValueTask<Education> GetById(int id)
        {
            return await base.SingleOrDefaultAsync(x => x.Id == id && x.RowStatu == EnumRowStatusType.Active);
        }
    }
}