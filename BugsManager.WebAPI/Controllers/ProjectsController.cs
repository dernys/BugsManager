using BugsManager.Application.DTOs;
using BugsManager.Application.Features.Bugs.Commands.DeleteBug;
using BugsManager.Application.Features.Projects.Commands.CreateProject;
using BugsManager.Application.Features.Projects.Commands.DeleteProject;
using BugsManager.Application.Features.Projects.Commands.UpdateProject;
using BugsManager.Application.Features.Projects.Queries.GetAllProjects;
using BugsManager.Application.Features.Projects.Queries.GetProjectById;
using BugsManager.Application.Features.Users.Commands.CreateUser;
using BugsManager.Application.Features.Users.Commands.DeleteUser;
using BugsManager.Application.Features.Users.Commands.UpdateUser;
using BugsManager.Application.Features.Users.Queries.GetAllUsers;
using BugsManager.Application.Features.Users.Queries.GetUserById;
using BugsManager.Shared;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BugsManager.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsController : ApiControllerBase
    {
        private readonly IMediator _mediator;
        public ProjectsController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpGet]
        public async Task<ActionResult<Result<List<GetAllProjectsDto>>>> Get()
        {
            return await _mediator.Send(new GetAllProjectsQuery());
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Result<GetProjectByIdDto>>> GetProjectById(int id)
        {
            return await _mediator.Send(new GetProjectByIdQuery(id));
        }
        [HttpPost]
        public async Task<ActionResult<Result<int>>> Create(CreateProjectCommand command)
        {
            return await _mediator.Send(command);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Result<int>>> Update(int id, ProjectDTO projectDTO)
        {
            
            return await _mediator.Send(new UpdateProjectCommand { Id = id, projectDTO = projectDTO});
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Result<int>>> Delete(int id)
        {
            return await _mediator.Send(new DeleteProjectCommand(id));
        }
    }
}
