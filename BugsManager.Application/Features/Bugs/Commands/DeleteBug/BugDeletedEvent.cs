using BugsManager.Domain.Common;
using BugsManager.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugsManager.Application.Features.Bugs.Commands.DeleteBug
{
    public class BugDeletedEvent: BaseEvent
    {
        public Bug _bug { get; }
        public BugDeletedEvent(Bug bug)
        {
            _bug = bug;
        }
    }
}
