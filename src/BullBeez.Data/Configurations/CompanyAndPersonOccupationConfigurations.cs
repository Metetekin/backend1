using BullBeez.Core.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using System;
using System.Collections.Generic;
using System.Text;

namespace BullBeez.Data.Configurations
{
    public class CompanyAndPersonOccupationConfigurations : IEntityTypeConfiguration<CompanyAndPersonOccupation>
    {
        public void Configure(EntityTypeBuilder<CompanyAndPersonOccupation> builder)
        {
            builder.ToTable("CompanyAndPersonOccupation");


            builder
                .HasKey(m => m.Id);


          
        }
    }
}
