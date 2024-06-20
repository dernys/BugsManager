using AutoMapper;
using BugsManager.Application.Interfaces.Repositories;
using BugsManager.Shared;
using MediatR;
using BugsManager.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace BugsManager.Application.Features.Users.Queries.GetUserById
{
    public record GetUserByIdQuery : IRequest<Result<GetUserByIdDto>>
    {
        public int Id { get; set; }

        public GetUserByIdQuery()
        {

        }

        public GetUserByIdQuery(int id)
        {
            Id = id;
        }
    }

    internal class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, Result<GetUserByIdDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetUserByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<GetUserByIdDto>> Handle(GetUserByIdQuery query, CancellationToken cancellationToken)
        {
            var entity = await _unitOfWork.Repository<User>().GetByIdAsync(query.Id);
            var User = _mapper.Map<GetUserByIdDto>(entity);
            return await Result<GetUserByIdDto>.SuccessAsync(User);
        }
    }
}
