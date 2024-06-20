using BugsManager.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugsManager.Domain.Entities
{
    public class User : BaseAuditableEntity
    {
        public string? Name { get; set; }
        public string? Surname { get; set; }
    }
}
