using BugsManager.Application.Common.Mappings;
using BugsManager.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugsManager.Application.Features.Users.Queries.GetUserById
{
    public class GetUserByIdDto: IMapFrom<User>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
    }
}
