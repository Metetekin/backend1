using BullBeez.Core.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using System;
using System.Collections.Generic;
using System.Text;

namespace BullBeez.Data.Configurations
{
    public class MailApprovedCodeConfigurations : IEntityTypeConfiguration<MailApprovedCode>
    {
        public void Configure(EntityTypeBuilder<MailApprovedCode> builder)
        {
            builder.ToTable("MailApprovedCode");


            builder
                .HasKey(m => m.Id);


            builder
                .Property(m => m.EmailAddress)
                .IsRequired()
                .HasMaxLength(100);



        }
    }
}