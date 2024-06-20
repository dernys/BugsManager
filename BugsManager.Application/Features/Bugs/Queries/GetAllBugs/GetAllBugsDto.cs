using BugsManager.Application.Common.Mappings;
using BugsManager.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace BugsManager.Application.Features.Bugs.Queries.GetAllBugs
{
    public class GetAllBugsDto : IMapFrom<Bug>
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public Project? Project { get; set; }
        public int UserId { get; set; }
        public User? User { get; set; }
        public string? Description { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
