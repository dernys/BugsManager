using BugsManager.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace BugsManager.Application.Interfaces.Repositories
{
    public interface IBugRepository
    {
        Task<List<Bug>> GetByUserIdAsync(int userId);
        Task<List<Bug>> GetByProjectIdAsync(int projectId);
    }
}
