using BugsManager.Application.Common.Mappings;
using BugsManager.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace BugsManager.Application.Features.Users.Queries.GetAllUsers
{
    public class GetAllUsersDto : IMapFrom<User>
    {
        public int Id { get; init; }
        public string? Name { get; init; }
        public string? Surname { get; init; }
    }
}
