using BullBeez.Core.Entities;
using BullBeez.Core.Repositories;
using BullBeez.Data.Context;

using System;
using System.Collections.Generic;
using System.Text;

namespace BullBeez.Data.Repositories
{
    public class BullBeezConfigRepository : Repository<BullBeezConfig>, IBullBeezConfigRepository
    {
        public BullBeezConfigRepository(BullBeezDBContext context)
            : base(context)
        { }



        private BullBeezDBContext BullBeezDBContext
        {
            get { return Context as BullBeezDBContext; }
        }


    }
}