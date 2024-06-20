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
    public class ProjectConfiguration : IEntityTypeConfiguration<Project>
    {
        public void Configure(EntityTypeBuilder<Project> builder)
        {
            var project1 = new Project { Id = 1, Name = "Project A", Description = "Description for Project A", CreatedDate = new DateTime(DateTime.Now.Year, 1, 15) };
            var project2 = new Project { Id = 2, Name = "Project B", Description = "Description for Project B", CreatedDate = new DateTime(DateTime.Now.Year, 2, 20) };
            var project3 = new Project { Id = 3, Name = "Project C", Description = "Description for Project C", CreatedDate = new DateTime(DateTime.Now.Year, 3, 25) };
            var project4 = new Project { Id = 4, Name = "Project D", Description = "Description for Project D", CreatedDate = new DateTime(DateTime.Now.Year, 4, 10) };
            var project5 = new Project { Id = 5, Name = "Project E", Description = "Description for Project E", CreatedDate = new DateTime(DateTime.Now.Year, 5, 5) };


            builder.HasData(project1, project2, project3, project4, project5);
        }
    }
}
