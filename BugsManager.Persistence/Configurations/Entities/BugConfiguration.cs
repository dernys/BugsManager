using BugsManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace BugsManager.Persistence.Configurations.Entities
{
    public class BugConfiguration : IEntityTypeConfiguration<Bug>
    {
        public void Configure(EntityTypeBuilder<Bug> builder)
        {
            builder.HasOne(b => b.Project)
            .WithMany()
            .HasForeignKey(b => b.ProjectId);

            builder
                .HasOne(b => b.User)
                .WithMany()
                .HasForeignKey(b => b.UserId);
            builder.Property(b => b.Description)
                .IsRequired();
        }
    }
}
