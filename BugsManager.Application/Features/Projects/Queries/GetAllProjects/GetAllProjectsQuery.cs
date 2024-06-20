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

namespace BugsManager.Application.Features.Projects.Queries.GetAllProjects
{
    public record GetAllProjectsQuery : IRequest<Result<List<GetAllProjectsDto>>>;

    internal class GetAllProjectsQueryHandler : IRequestHandler<GetAllProjectsQuery, Result<List<GetAllProjectsDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllProjectsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<List<GetAllProjectsDto>>> Handle(GetAllProjectsQuery query, CancellationToken cancellationToken)
        {
            var Projects = await _unitOfWork.Repository<Project>().Entities
                   .ProjectTo<GetAllProjectsDto>(_mapper.ConfigurationProvider)
                   .ToListAsync(cancellationToken);

            return await Result<List<GetAllProjectsDto>>.SuccessAsync(Projects);
        }
    }
}
