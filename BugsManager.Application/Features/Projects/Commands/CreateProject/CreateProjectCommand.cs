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

namespace BugsManager.Application.Features.Projects.Commands.CreateProject
{
    public record CreateProjectCommand : IRequest<Result<int>>, IMapFrom<Project>
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }

    internal class CreateProjectCommandHandler : IRequestHandler<CreateProjectCommand, Result<int>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateProjectCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<int>> Handle(CreateProjectCommand command, CancellationToken cancellationToken)
        {
            var Project = new Project()
            {
                Name = command.Name,
                Description = command.Description
            };

            await _unitOfWork.Repository<Project>().AddAsync(Project);
            Project.AddDomainEvent(new ProjectCreatedEvent(Project));
            await _unitOfWork.Save(cancellationToken);
            return await Result<int>.SuccessAsync(Project.Id, "Project Created.");
        }
    }
}
