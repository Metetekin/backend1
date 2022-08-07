using BullBeez.Core.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using System;
using System.Collections.Generic;
using System.Text;

namespace BullBeez.Data.Configurations
{
    public class FollowsConfigurations : IEntityTypeConfiguration<Follows>
    {
        public void Configure(EntityTypeBuilder<Follows> builder)
        {
            builder.ToTable("Follows");


            builder
                .HasKey(m => m.Id);
        }
    }
}
