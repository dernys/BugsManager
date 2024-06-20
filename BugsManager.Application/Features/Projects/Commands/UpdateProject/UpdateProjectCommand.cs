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

namespace BugsManager.Application.Features.Projects.Commands.UpdateProject
{
    public record UpdateProjectCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
        public ProjectDTO projectDTO { get; set; }
    }

    internal class UpdateProjectCommandHandler : IRequestHandler<UpdateProjectCommand, Result<int>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateProjectCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<int>> Handle(UpdateProjectCommand command, CancellationToken cancellationToken)
        {
            var Project = await _unitOfWork.Repository<Project>().GetByIdAsync(command.Id);
            var projectDto = command.projectDTO;
            if (Project != null)
            {
                Project.Name = projectDto.Name;
                Project.Description = projectDto.Description;
               

                await _unitOfWork.Repository<Project>().UpdateAsync(Project);
                Project.AddDomainEvent(new ProjectUpdatedEvent(Project));

                await _unitOfWork.Save(cancellationToken);

                return await Result<int>.SuccessAsync(Project.Id, "Project Updated.");
            }
            else
            {
                return await Result<int>.FailureAsync("Project Not Found.");
            }
        }
    }
}
