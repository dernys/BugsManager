using AutoMapper;
using AutoMapper.QueryableExtensions;
using BugsManager.Application.Interfaces.Repositories;
using BugsManager.Domain.Entities;
using BugsManager.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace BugsManager.Application.Features.Bugs.Queries.GetAllBugs
{
    public class GetAllBugsQuery : IRequest<Result<List<GetAllBugsDto>>>
    {
        public int? project_id { get; set; } 
        public int? user_id { get; set; } 
        public DateTime? start_date { get; set; }
        public DateTime? end_date { get; set; }
    }

    internal class GetAllBugsQueryHandler : IRequestHandler<GetAllBugsQuery, Result<List<GetAllBugsDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllBugsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<List<GetAllBugsDto>>> Handle(GetAllBugsQuery query, CancellationToken cancellationToken)
        {
            var Bugs = await _unitOfWork.Repository<Bug>().Entities
                
                   .ProjectTo<GetAllBugsDto>(_mapper.ConfigurationProvider)
                   .ToListAsync(cancellationToken);
            if(query.user_id != null)
            {
                Bugs = Bugs.Where(b => b.UserId == query.user_id).ToList();
            }
            if (query.project_id != null)
            {
                Bugs = Bugs.Where(b => b.ProjectId == query.project_id).ToList();
            }
            if (!string.IsNullOrEmpty(query.start_date.ToString()) && !string.IsNullOrEmpty(query.end_date.ToString()))
            {
                Bugs = Bugs.Where(b => b.CreationDate >= query.start_date && b.CreationDate <= query.end_date).ToList();

            }else if (!string.IsNullOrEmpty(query.start_date.ToString()))
            {
                Bugs = Bugs.Where(b => b.CreationDate >= query.start_date).ToList();
            }
            else if (!string.IsNullOrEmpty(query.end_date.ToString()))
            {
                Bugs = Bugs.Where(b => b.CreationDate <= query.end_date).ToList();
            }

            return await Result<List<GetAllBugsDto>>.SuccessAsync(Bugs);
        }
    }
}
