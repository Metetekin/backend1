using BullBeez.Core.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using System;
using System.Collections.Generic;
using System.Text;

namespace BullBeez.Data.Configurations
{
    public class ServiceAnswersConfiguration : IEntityTypeConfiguration<ServiceAnswers>
    {
        public void Configure(EntityTypeBuilder<ServiceAnswers> builder)
        {
            builder.ToTable("ServiceAnswers");


            builder
                .HasKey(m => m.Id);


            builder
                .Property(m => m.TextData)
                .IsRequired()
                .HasMaxLength(4000);


        }
    }
}
