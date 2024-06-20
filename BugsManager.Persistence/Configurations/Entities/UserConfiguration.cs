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
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            var user1 = new User { Id = 1, Name = "John", Surname = "Smith" };
            var user2 = new User { Id = 2, Name = "Emily", Surname = "Johnson" };
            var user3 = new User { Id = 3, Name = "Michael", Surname = "Brown" };
            var user4 = new User { Id = 4, Name = "Sarah", Surname = "Davis" };
            var user5 = new User { Id = 5, Name = "David", Surname = "Wilson" };

            builder.HasData(user1, user2, user3, user4, user5);
        }
    }
}
