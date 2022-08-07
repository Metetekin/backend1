using BullBeez.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace BullBeez.Data.Configurations
{
    class PostCommentsConfiguration : IEntityTypeConfiguration<PostComments>
    {
        public void Configure(EntityTypeBuilder<PostComments> builder)
        {
            builder.ToTable("PostComments");


            builder
                .HasKey(m => m.Id);

            builder
                .Property(m => m.Text)
                .IsRequired();
        }
    }
}
