using BugsManager.Application.Interfaces.Repositories;
using BugsManager.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugsManager.Persistence.Repositories
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly IGenericRepository<Bug> _repository;
        public ProjectRepository(IGenericRepository<Bug> repository)
        {
            _repository = repository;

        }
    
    }
}
