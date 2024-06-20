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

namespace BugsManager.Application.Features.Bugs.Commands.DeleteBug
{
    public record DeleteBugByIdCommand : IRequest<Result<int>>, IMapFrom<Bug>
    {
        public int Id { get; set; }

        public DeleteBugByIdCommand()
        {

        }

        public DeleteBugByIdCommand(int id)
        {
            Id = id;
        }
    }

    internal class DeleteBugByIdCommandHandler : IRequestHandler<DeleteBugByIdCommand, Result<int>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DeleteBugByIdCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<int>> Handle(DeleteBugByIdCommand command, CancellationToken cancellationToken)
        {
            var Bug = await _unitOfWork.Repository<Bug>().GetByIdAsync(command.Id);
            if (Bug != null)
            {
                await _unitOfWork.Repository<Bug>().DeleteAsync(Bug);
                Bug.AddDomainEvent(new BugDeletedEvent(Bug));

                await _unitOfWork.Save(cancellationToken);

                return await Result<int>.SuccessAsync(Bug.Id, "Bug Deleted");
            }
            else
            {
                return await Result<int>.FailureAsync("Bug Not Found.");
            }
        }
    }
}
