using BullBeez.Core.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using System;
using System.Collections.Generic;
using System.Text;

namespace BullBeez.Data.Configurations
{
    public class CompanyAndPersonDetailConfigurations : IEntityTypeConfiguration<CompanyAndPersonDetail>
    {
        public void Configure(EntityTypeBuilder<CompanyAndPersonDetail> builder)
        {
            builder.ToTable("CompanyAndPersonDetails");


            builder
                .HasKey(m => m.Id);


           

           
        }
    }
}
