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

namespace BugsManager.Application.Features.Users.Commands.UpdateUser
{
    public record UpdateUserCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
        public UserDTO user { get; set; }
    }

    internal class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, Result<int>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateUserCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<int>> Handle(UpdateUserCommand command, CancellationToken cancellationToken)
        {
            var User = await _unitOfWork.Repository<User>().GetByIdAsync(command.Id);
            var userDto = command.user;
            if (User != null)
            {
                User.Name = userDto.Name;
                User.Surname = userDto.Surname;
               

                await _unitOfWork.Repository<User>().UpdateAsync(User);
                User.AddDomainEvent(new UserUpdatedEvent(User));

                await _unitOfWork.Save(cancellationToken);

                return await Result<int>.SuccessAsync(User.Id, "User Updated.");
            }
            else
            {
                return await Result<int>.FailureAsync("User Not Found.");
            }
        }
    }
}
