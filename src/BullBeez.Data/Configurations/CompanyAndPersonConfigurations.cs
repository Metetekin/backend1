
using BullBeez.Core.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using System;
using System.Collections.Generic;
using System.Text;

namespace BullBeez.Data.Configurations
{
    public class CompanyAndPersonConfigurations : IEntityTypeConfiguration<CompanyAndPerson>
    {
        public void Configure(EntityTypeBuilder<CompanyAndPerson> builder)
        {
            builder.ToTable("CompanyAndPerson");


            builder
                .HasKey(m => m.Id);


            builder
                .Property(m => m.UserName)
                .IsRequired()
                .HasMaxLength(100);


            builder
                .Property(m => m.GSM)
                .HasMaxLength(20);

            builder
               .Property(m => m.EmailAddress)
               .IsRequired()
               .HasMaxLength(100);

            builder
               .Property(m => m.Status)
               .HasMaxLength(500);

            builder
                .Property(m => m.BullbeezSentence)
                .HasMaxLength(500);

            builder
                .Property(m => m.Biography)
                .HasMaxLength(500);
            
            builder
                .Property(m => m.CompanyDescription)
                .HasMaxLength(2000);

        }
    }
}
