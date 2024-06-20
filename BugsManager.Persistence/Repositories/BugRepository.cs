using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BugsManager.Application.Interfaces.Repositories;
using BugsManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BugsManager.Persistence.Repositories
{
    public class BugRepository : IBugRepository
    {
        private readonly IGenericRepository<Bug> _repository;
        public BugRepository(IGenericRepository<Bug> repository)
        {
            _repository = repository;

        }
        public async Task<List<Bug>> GetByProjectIdAsync(int projectId)
        {
            return await _repository.Entities.Where(x => x.ProjectId == projectId).ToListAsync();
        }

        public async Task<List<Bug>> GetByUserIdAsync(int userId)
        {
            return await _repository.Entities.Where(x => x.UserId == userId).ToListAsync();
        }
    }
}
