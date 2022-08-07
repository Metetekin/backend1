using BullBeez.Core.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using System;
using System.Collections.Generic;
using System.Text;

namespace BullBeez.Data.Configurations
{
    public class CompanyTypeConfigurations : IEntityTypeConfiguration<CompanyType>
    {
        public void Configure(EntityTypeBuilder<CompanyType> builder)
        {
            builder.ToTable("CompanyType");


            builder
                .HasKey(m => m.Id);


            builder
                .Property(m => m.Name)
                .IsRequired()
                .HasMaxLength(250);


        }
    }
}