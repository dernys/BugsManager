using BugsManager.Domain.Common;
using BugsManager.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugsManager.Application.Features.Projects.Commands.UpdateProject
{
    public class ProjectUpdatedEvent: BaseEvent
    {
        public Project _project { get; }
        public ProjectUpdatedEvent(Project project)
        {
            _project = project;
        }
    }
}
