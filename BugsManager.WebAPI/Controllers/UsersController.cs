using BugsManager.Application.DTOs;
using BugsManager.Application.Features.Users.Commands.CreateUser;
using BugsManager.Application.Features.Users.Commands.DeleteUser;
using BugsManager.Application.Features.Users.Commands.UpdateUser;
using BugsManager.Application.Features.Users.Queries.GetAllUsers;
using BugsManager.Application.Features.Users.Queries.GetUserById;
using BugsManager.Shared;
using MediatR;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BugsManager.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ApiControllerBase
    {
        private readonly IMediator _mediator;
        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpGet]
        public async Task<ActionResult<Result<List<GetAllUsersDto>>>> Get()
        {
            return await _mediator.Send(new GetAllUserQuery());
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Result<GetUserByIdDto>>> GetUserById(int id)
        {
            return await _mediator.Send(new GetUserByIdQuery(id));
        }
        [HttpPost]
        public async Task<ActionResult<Result<int>>> Create(CreateUserCommand command)
        {
            return await _mediator.Send(command);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Result<int>>> Update(int id, UserDTO user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }

            return await _mediator.Send(new UpdateUserCommand { Id = id, user = user});
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Result<int>>> Delete(int id)
        {
            return await _mediator.Send(new DeleteUserCommand(id));
        }
    }
}
