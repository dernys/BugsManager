using BugsManager.Domain.Common;
using BugsManager.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugsManager.Application.Features.Bugs.Commands.CreateBug
{
    public class BugCreatedEvent: BaseEvent
    {
        public Bug Bug { get; }

        public BugCreatedEvent(Bug bug)
        {
            this.Bug = bug;
        }
    }
}
