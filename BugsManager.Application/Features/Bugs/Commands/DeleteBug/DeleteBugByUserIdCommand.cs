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
    public record DeleteBugByUserIdCommand : IRequest<Result<int>>, IMapFrom<Bug>
    {
        public int UserId { get; set; }

        public DeleteBugByUserIdCommand()
        {

        }

        public DeleteBugByUserIdCommand(int userId)
        {
            UserId = userId;
        }
    }

    internal class DeleteBugByUserIdCommandHandler : IRequestHandler<DeleteBugByUserIdCommand, Result<int>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IBugRepository _bugRepository;
        public DeleteBugByUserIdCommandHandler(IUnitOfWork unitOfWork, IMapper mapper,IBugRepository bugRepository)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _bugRepository = bugRepository;
        }

        public async Task<Result<int>> Handle(DeleteBugByUserIdCommand command, CancellationToken cancellationToken)
        {
            var bugs = await _bugRepository.GetByUserIdAsync(command.UserId);

            if (bugs.Any())
            {
                foreach (var bug in bugs)
                {
                    await _unitOfWork.Repository<Bug>().DeleteAsync(bug);
                    bug.AddDomainEvent(new BugDeletedEvent(bug));

                    await _unitOfWork.Save(cancellationToken);

                }


                return await Result<int>.SuccessAsync(1, "Bugs Deleted");
            }
            else
            {
                return await Result<int>.FailureAsync("Bugs Not Found.");
            }
        }
    }
}
