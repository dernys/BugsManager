using AutoMapper;
using BugsManager.Application.Interfaces.Repositories;
using BugsManager.Shared;
using MediatR;
using BugsManager.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace BugsManager.Application.Features.Projects.Queries.GetProjectById
{
    public record GetProjectByIdQuery : IRequest<Result<GetProjectByIdDto>>
    {
        public int Id { get; set; }

        public GetProjectByIdQuery()
        {

        }

        public GetProjectByIdQuery(int id)
        {
            Id = id;
        }
    }

    internal class GetProjectByIdQueryHandler : IRequestHandler<GetProjectByIdQuery, Result<GetProjectByIdDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetProjectByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<GetProjectByIdDto>> Handle(GetProjectByIdQuery query, CancellationToken cancellationToken)
        {
            var entity = await _unitOfWork.Repository<Project>().GetByIdAsync(query.Id);
            var Project = _mapper.Map<GetProjectByIdDto>(entity);
            return await Result<GetProjectByIdDto>.SuccessAsync(Project);
        }
    }
}
