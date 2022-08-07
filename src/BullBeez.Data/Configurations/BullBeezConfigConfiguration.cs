using BullBeez.Core.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using System;
using System.Collections.Generic;
using System.Text;

namespace BullBeez.Data.Configurations
{
    public class BullBeezConfigConfiguration : IEntityTypeConfiguration<BullBeezConfig>
    {
        public void Configure(EntityTypeBuilder<BullBeezConfig> builder)
        {
            builder.ToTable("BullBeezConfig");


            builder
                .HasKey(m => m.Id);


            builder
                .Property(m => m.JsonData)
                .IsRequired()
                .HasMaxLength(4000);



        }
    }
}