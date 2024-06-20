using BugsManager.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugsManager.Domain.Entities
{
    public class Project: BaseAuditableEntity
    {
        public string? Name { get; set; }
        public string? Description { get; set; }

    }
}
