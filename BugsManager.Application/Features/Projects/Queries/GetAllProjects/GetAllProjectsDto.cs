using BugsManager.Application.Common.Mappings;
using BugsManager.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace BugsManager.Application.Features.Projects.Queries.GetAllProjects
{
    public class GetAllProjectsDto : IMapFrom<Project>
    {
        public int Id { get; init; }
        public string Name { get; init; }
        public string Description { get; init; }
    }
}
