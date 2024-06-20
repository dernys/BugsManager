using AutoMapper;
using BugsManager.Application.Common.Mappings;
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

namespace BugsManager.Application.Features.Projects.Commands.DeleteProject
{
    public record DeleteProjectCommand : IRequest<Result<int>>, IMapFrom<Project>
    {
        public int Id { get; set; }

        public DeleteProjectCommand()
        {

        }

        public DeleteProjectCommand(int id)
        {
            Id = id;
        }
    }

    internal class DeleteProjectCommandHandler : IRequestHandler<DeleteProjectCommand, Result<int>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DeleteProjectCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<int>> Handle(DeleteProjectCommand command, CancellationToken cancellationToken)
        {
            var Project = await _unitOfWork.Repository<Project>().GetByIdAsync(command.Id);
            if (Project != null)
            {
                await _unitOfWork.Repository<Project>().DeleteAsync(Project);
                Project.AddDomainEvent(new ProjectDeletedEvent(Project));

                await _unitOfWork.Save(cancellationToken);

                return await Result<int>.SuccessAsync(Project.Id, "Project Deleted");
            }
            else
            {
                return await Result<int>.FailureAsync("Project Not Found.");
            }
        }
    }
}
