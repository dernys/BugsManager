using BugsManager.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugsManager.Domain.Entities
{
    public class Bug : BaseAuditableEntity
    {
        public int ProjectId { get; set; }
        public Project? Project { get; set; }
        public int UserId { get; set; }
        public User? User { get; set; }
        public string? Description { get; set; }
        public DateTime CreationDate { get; set; }
    }
      
}
