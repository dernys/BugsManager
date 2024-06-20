using AutoMapper;
using BugsManager.Application.Interfaces.Repositories;
using BugsManager.Shared;
using MediatR;
using BugsManager.Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using BugsManager.Application.DTOs;

namespace BugsManager.Application.Features.Bugs.Commands.UpdateBug
{
    public record UpdateBugCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
        public BugDTO bug { get; set; }
    }

    internal class UpdateBugCommandHandler : IRequestHandler<UpdateBugCommand, Result<int>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateBugCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<int>> Handle(UpdateBugCommand command, CancellationToken cancellationToken)
        {
            var bugDto = command.bug;

            var bugObject = await _unitOfWork.Repository<Bug>().GetByIdAsync(command.Id);
            var projectObject = await _unitOfWork.Repository<Project>().GetByIdAsync(bugDto.ProjectId);
            var userObject = await _unitOfWork.Repository<User>().GetByIdAsync(bugDto.UserId);

            if (bugObject != null)
            {
                bugObject.ProjectId = bugDto.ProjectId;
                bugObject.UserId = bugDto.UserId;
                bugObject.Description = bugDto.Description;
                bugObject.Project = projectObject;
                bugObject.User = userObject;
               

                await _unitOfWork.Repository<Bug>().UpdateAsync(bugObject);
                bugObject.AddDomainEvent(new BugUpdatedEvent(bugObject));

                await _unitOfWork.Save(cancellationToken);

                return await Result<int>.SuccessAsync(bugObject.Id, "Bug Updated.");
            }
            else
            {
                return await Result<int>.FailureAsync("Bug Not Found.");
            }
        }
    }
}
