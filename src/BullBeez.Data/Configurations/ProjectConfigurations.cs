using BullBeez.Core.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using System;
using System.Collections.Generic;
using System.Text;

namespace BullBeez.Data.Configurations
{
    public class ProjectConfigurations : IEntityTypeConfiguration<Project>
    {
        public void Configure(EntityTypeBuilder<Project> builder)
        {
            builder.ToTable("Projects");


            builder
                .HasKey(m => m.Id);


            builder
                .Property(m => m.ProjectName)
                .IsRequired()
                .HasMaxLength(500);

            builder
               .Property(m => m.ProjectRoleName)
               .IsRequired()
               .HasMaxLength(500);


            builder
               .Property(m => m.ProjectDescription)
               .IsRequired()
               .HasMaxLength(500);



        }
    }
}
