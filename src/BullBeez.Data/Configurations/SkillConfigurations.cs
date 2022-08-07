using BullBeez.Core.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using System;
using System.Collections.Generic;
using System.Text;

namespace BullBeez.Data.Configurations
{
    public class SkillConfigurations : IEntityTypeConfiguration<Skill>
    {
        public void Configure(EntityTypeBuilder<Skill> builder)
        {
            builder.ToTable("Skills");


            builder
                .HasKey(m => m.Id);


            builder
                .Property(m => m.Name)
                .IsRequired()
                .HasMaxLength(100);

            
        }
    }
}
