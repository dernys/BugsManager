using BugsManager.Domain.Common;
using BugsManager.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugsManager.Application.Features.Projects.Commands.CreateProject
{
    public class ProjectCreatedEvent: BaseEvent
    {
        public Project _project { get; }
        public ProjectCreatedEvent(Project project)
        {
            _project = project;
        }
    }
}
