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

namespace BugsManager.Application.Features.Users.Commands.CreateUser
{
    public record CreateUserCommand : IRequest<Result<int>>, IMapFrom<User>
    {
        public string Name { get; set; }
        public string Surname { get; set; }
    }

    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Result<int>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateUserCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<int>> Handle(CreateUserCommand command, CancellationToken cancellationToken)
        {
            var user = new User()
            {
                Name = command.Name,
                Surname = command.Surname
            };

            await _unitOfWork.Repository<User>().AddAsync(user);
            user.AddDomainEvent(new UserCreatedEvent(user));
            await _unitOfWork.Save(cancellationToken);
            return await Result<int>.SuccessAsync(user.Id, "User Created.");
        }
    }
}
