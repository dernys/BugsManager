using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using BugsManager.Shared;
using BugsManager.Application.Interfaces.Repositories;
using BugsManager.Domain.Entities;

namespace BugsManager.Application.Features.Users.Queries.GetAllUsers
{
    public record GetAllUserQuery : IRequest<Result<List<GetAllUsersDto>>>;

    internal class GetAllUsersQueryHandler : IRequestHandler<GetAllUserQuery, Result<List<GetAllUsersDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllUsersQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<List<GetAllUsersDto>>> Handle(GetAllUserQuery query, CancellationToken cancellationToken)
        {
            var Users = await _unitOfWork.Repository<User>().Entities
                   .ProjectTo<GetAllUsersDto>(_mapper.ConfigurationProvider)
                   .ToListAsync(cancellationToken);

            return await Result<List<GetAllUsersDto>>.SuccessAsync(Users);
        }
    }
}
