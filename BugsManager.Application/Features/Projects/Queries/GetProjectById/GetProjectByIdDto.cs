using BugsManager.Application.Common.Mappings;
using BugsManager.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugsManager.Application.Features.Projects.Queries.GetProjectById
{
    public class GetProjectByIdDto: IMapFrom<Project>
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
    }
}
