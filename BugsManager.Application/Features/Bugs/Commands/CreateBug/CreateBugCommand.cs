using AutoMapper;
using BugsManager.Application.Common.Mappings;
using BugsManager.Application.DTOs;
using BugsManager.Application.Interfaces.Repositories;
using BugsManager.Domain.Entities;
using BugsManager.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace BugsManager.Application.Features.Bugs.Commands.CreateBug
{
    public record CreateBugCommand : IRequest<Result<int>>, IMapFrom<Bug>
    {
        public required BugDTO bug { get; set; }
    }

    internal class CreateBugCommandHandler : IRequestHandler<CreateBugCommand, Result<int>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateBugCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<int>> Handle(CreateBugCommand command, CancellationToken cancellationToken)
        {
            var bugDto = command.bug;

            var project = await _unitOfWork.Repository<Project>().GetByIdAsync(bugDto.ProjectId);
            var user = await _unitOfWork.Repository<User>().GetByIdAsync(bugDto.UserId);

            var Bug = new Bug()
            {
                ProjectId = bugDto.ProjectId,
                Project = project,
                UserId = bugDto.UserId,
                User = user,
                Description = bugDto.Description,
                CreationDate = DateTime.Now
            };

            await _unitOfWork.Repository<Bug>().AddAsync(Bug);
            Bug.AddDomainEvent(new BugCreatedEvent(Bug));
            await _unitOfWork.Save(cancellationToken);
            return await Result<int>.SuccessAsync(Bug.Id, "Bug Created.");
        }
    }
}
