using BullBeez.Core.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using System;
using System.Collections.Generic;
using System.Text;

namespace BullBeez.Data.Configurations
{
    public class RequestHistoryConfigurations : IEntityTypeConfiguration<RequestHistory>
    {
        public void Configure(EntityTypeBuilder<RequestHistory> builder)
        {
            builder.ToTable("RequestHistory");


            builder
                .HasKey(m => m.Id);


            builder
                .Property(m => m.JsonRequest)
                .IsRequired()
                .HasColumnType("text");

            builder
               .Property(m => m.FunctionName)
               .IsRequired()
               .HasMaxLength(100);
        }
    }
}
