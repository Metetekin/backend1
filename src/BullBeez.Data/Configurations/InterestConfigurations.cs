using BullBeez.Core.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using System;
using System.Collections.Generic;
using System.Text;

namespace BullBeez.Data.Configurations
{
    public class InterestConfigurations : IEntityTypeConfiguration<Interest>
    {
        public void Configure(EntityTypeBuilder<Interest> builder)
        {
            builder.ToTable("Interest");


            builder
                .HasKey(m => m.Id);


            builder
                .Property(m => m.Name)
                .IsRequired()
                .HasMaxLength(100);

            
        }
    }
}
