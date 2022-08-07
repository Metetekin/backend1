using BullBeez.Core.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using System;
using System.Collections.Generic;
using System.Text;

namespace BullBeez.Data.Configurations
{
    public class PostsConfiguration : IEntityTypeConfiguration<Posts>
    {
        public void Configure(EntityTypeBuilder<Posts> builder)
        {
            builder.ToTable("Posts");


            builder
                .HasKey(m => m.Id);


            builder
                .Property(m => m.Text)
                .IsRequired()
                .HasMaxLength(1000);

            builder
                .Property(m => m.UserName)
                .IsRequired()
                .HasMaxLength(50);

            builder
                .Property(m => m.UserLink)
                .IsRequired()
                .HasMaxLength(200);

            builder
                .Property(m => m.MediaLink)
                .IsRequired()
                .HasMaxLength(200);

            builder
                .Property(m => m.PostLink)
                .IsRequired()
                .HasMaxLength(200);


        }
    }
}