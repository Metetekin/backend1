using BullBeez.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace BullBeez.Data.Configurations
{
    public class UserPostsConfiguration : IEntityTypeConfiguration<UserPosts>
    {
        public void Configure(EntityTypeBuilder<UserPosts> builder)
        {
            builder.ToTable("UserPosts");


            builder
                .HasKey(m => m.Id);

            builder
                .Property(m => m.CreatedDate)
                .IsRequired();
            builder
                .Property(m => m.LikeCount)
                .IsRequired();
            builder
                .Property(m => m.CommentCount)
                .IsRequired();
        }
    }
}
