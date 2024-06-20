using BugsManager.Domain.Common;
using BugsManager.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugsManager.Application.Features.Bugs.Commands.UpdateBug
{
    public class BugUpdatedEvent: BaseEvent
    {
        public Bug _bug { get; }
        public BugUpdatedEvent(Bug bug)
        {
            _bug = bug;
        }
    }
}
