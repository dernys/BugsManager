using BugsManager.Domain.Common;
using BugsManager.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugsManager.Application.Features.Projects.Commands.DeleteProject
{
    public class ProjectDeletedEvent: BaseEvent
    {
        public Project _project { get; }
        public ProjectDeletedEvent(Project project)
        {
            _project = project;
        }
    }
}
