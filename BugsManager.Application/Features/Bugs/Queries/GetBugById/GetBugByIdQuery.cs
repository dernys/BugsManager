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

namespace BugsManager.Application.Features.Bugs.Queries.GetBugById
{
    public record GetBugByIdQuery : IRequest<Result<GetBugByIdDto>>
    {
        public int Id { get; set; }

        public GetBugByIdQuery()
        {

        }

        public GetBugByIdQuery(int id)
        {
            Id = id;
        }
    }

    internal class GetBugByIdQueryHandler : IRequestHandler<GetBugByIdQuery, Result<GetBugByIdDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetBugByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<GetBugByIdDto>> Handle(GetBugByIdQuery query, CancellationToken cancellationToken)
        {
;
            var bugEntity = await _unitOfWork.Repository<Bug>().GetByIdAsync(query.Id);

            var userEntity = await _unitOfWork.Repository<User>().GetByIdAsync(bugEntity.UserId);
            var projectEntity = await _unitOfWork.Repository<Project>().GetByIdAsync(bugEntity.ProjectId);

            bugEntity.Project = projectEntity;
            bugEntity.User = userEntity;

            var Bug = _mapper.Map<GetBugByIdDto>(bugEntity);

            return await Result<GetBugByIdDto>.SuccessAsync(Bug);
        }
    }
}
