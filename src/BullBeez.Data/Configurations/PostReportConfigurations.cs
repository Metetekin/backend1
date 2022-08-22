using BullBeez.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace BullBeez.Data.Configurations
{
    public class PostReportConfigurations : IEntityTypeConfiguration<PostReport>
    {
        public void Configure(EntityTypeBuilder<PostReport> builder)
        {
            builder.ToTable("PostReport");


            builder
                .HasKey(m => m.Id);
        }
    }
}
