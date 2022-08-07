using BullBeez.Core.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using System;
using System.Collections.Generic;
using System.Text;

namespace BullBeez.Data.Configurations
{
    public class TokenConfigurations : IEntityTypeConfiguration<Token>
    {
        public void Configure(EntityTypeBuilder<Token> builder)
        {
            builder.ToTable("Token");


            builder
                .HasKey(m => m.Id);


            builder
                .Property(m => m.DeviceId)
                .IsRequired()
                .HasMaxLength(100);

            builder
                .Property(m => m.TokenId)
                .IsRequired()
                .HasMaxLength(500);


        }
    }
}
