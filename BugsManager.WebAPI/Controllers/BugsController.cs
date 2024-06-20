using BugsManager.Application.DTOs;
using BugsManager.Application.Features.Bugs.Commands.CreateBug;
using BugsManager.Application.Features.Bugs.Commands.DeleteBug;
using BugsManager.Application.Features.Bugs.Commands.UpdateBug;
using BugsManager.Application.Features.Bugs.Queries.GetAllBugs;
using BugsManager.Application.Features.Bugs.Queries.GetBugById;
using BugsManager.Application.Features.Projects.Commands.CreateProject;
using BugsManager.Application.Features.Projects.Commands.DeleteProject;
using BugsManager.Application.Features.Projects.Commands.UpdateProject;
using BugsManager.Application.Features.Projects.Queries.GetAllProjects;
using BugsManager.Application.Features.Projects.Queries.GetProjectById;
using BugsManager.Shared;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BugsManager.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BugsController : ApiControllerBase
    {
        private readonly IMediator _mediator;
        public BugsController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpGet]
        public async Task<ActionResult<Result<List<GetAllBugsDto>>>> Get([FromQuery] GetAllBugsQuery query)
        {
            /*if (query.project_id == null && query.user_id == null && query.start_date == null && query.end_date == null)
            {
                return StatusCode(405); 
            }*/

            return await _mediator.Send(query);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Result<GetBugByIdDto>>> GetBugById(int id)
        {
            return await _mediator.Send(new GetBugByIdQuery(id));
        }
        [HttpPost]
        public async Task<ActionResult<Result<int>>> Create(BugDTO bug)
        {
            return await _mediator.Send(new CreateBugCommand{ bug = bug });
          
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Result<int>>> Update(int id, BugDTO bug)
        {
            if (id != bug.Id)
            {
                return BadRequest();
            }

            return await _mediator.Send(new UpdateBugCommand { Id=id, bug=bug});
           
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Result<int>>> DeleteById(int id)
        {
             return await _mediator.Send(new DeleteBugByIdCommand(id));
        }
    }
}
