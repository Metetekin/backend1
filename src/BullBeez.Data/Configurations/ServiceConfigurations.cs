using BullBeez.Core.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using System;
using System.Collections.Generic;
using System.Text;

namespace BullBeez.Data.Configurations
{
    public class ServiceConfigurations : IEntityTypeConfiguration<Service>
    {
        public void Configure(EntityTypeBuilder<Service> builder)
        {
            builder.ToTable("Service");


            builder
                .HasKey(m => m.Id);

            builder
                .Property(m => m.ServiceName)
                .IsRequired()
                .HasMaxLength(250);


            builder
                .Property(m => m.Amount)
                .HasColumnType("decimal(11,2)");

            builder
                .Property(m => m.Description)
                .HasMaxLength(2000);

        }
    }
}