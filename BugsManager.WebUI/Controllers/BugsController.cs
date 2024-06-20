using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BugsManager.Domain.Entities;
using BugsManager.Persistence.Contexts;
using MediatR;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using BugsManager.Application.Features.Bugs.Queries.GetAllBugs;
using BugsManager.Application.Features.Bugs.Queries.GetBugById;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using BugsManager.Application.DTOs;
using BugsManager.Application.Features.Bugs.Commands.CreateBug;
using BugsManager.Application.Features.Projects.Queries.GetAllProjects;
using BugsManager.Application.Features.Users.Queries.GetAllUsers;
using BugsManager.Application.Features.Bugs.Commands.DeleteBug;
using BugsManager.Application.Features.Bugs.Commands.UpdateBug;

namespace BugsManager.WebUI.Controllers
{
    public class BugsController : Controller
    {
        private readonly IMediator _mediator;

        public BugsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: Bugs
        public async Task<IActionResult> BugIndex()
        {
            var data = await _mediator.Send(new GetAllBugsQuery());
            return View("Index", data.Data);
        }
        
        // GET: Bugs/Details/5
        public async Task<IActionResult> Details(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bug = await _mediator.Send(new GetBugByIdQuery(id));

            if (bug.Data == null)
            {
                return NotFound();
            }

            return View(bug.Data);
        }

        // GET: Bugs/Create
        public async Task<IActionResult> CreateBugs()
        {
            var projects = await _mediator.Send(new GetAllProjectsQuery());
            var users = await _mediator.Send(new GetAllUserQuery());
            ViewData["ProjectId"] = new SelectList(projects.Data, "Id", "Name");
            ViewData["UserId"] = new SelectList(users.Data, "Id", "Name");
            return View("Create");
        }

        // POST: Bugs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateBug([Bind("ProjectId,UserId,Description,CreationDate")] BugDTO bug)
        {
            if (ModelState.IsValid)
            {
                 await _mediator.Send(new CreateBugCommand
                {
                     bug= bug   
                });
                return RedirectToAction(nameof(BugIndex));
            }
           
            return View("Index",bug);
        }
       
        // GET: Bugs/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bug = await _mediator.Send(new GetBugByIdQuery(id));
            if (bug.Data == null)
            {
                return NotFound();
            }
            var projects = await _mediator.Send(new GetAllProjectsQuery());
            var users = await _mediator.Send(new GetAllUserQuery());
            ViewData["ProjectId"] = new SelectList(projects.Data, "Id", "Name", bug.Data.ProjectId);
            ViewData["UserId"] = new SelectList(users.Data, "Id", "Name", bug.Data.UserId);

            return View(bug.Data);
        }

        // POST: Bugs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProjectId,UserId,Description,CreationDate,Id")] BugDTO bug)
        {
            if (id != bug.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _mediator.Send(new UpdateBugCommand
                    {
                        Id = id,
                        bug = bug
                    });
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await BugExists(bug.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(BugIndex));
            }
            var projects = await _mediator.Send(new GetAllProjectsQuery());
            var users = await _mediator.Send(new GetAllUserQuery());
            ViewData["ProjectId"] = new SelectList(projects.Data, "Id", "Name", bug.ProjectId);
            ViewData["UserId"] = new SelectList(users.Data, "Id", "Name", bug.UserId);

            return View(bug);
        }
        
       // GET: Bugs/Delete/5
       public async Task<IActionResult> Delete(int id)
       {
           if (id == null )
           {
               return NotFound();
           }

            var bug = await _mediator.Send(new GetBugByIdQuery(id));
            if (bug.Data == null)
           {
               return NotFound();
           }

           return View(bug.Data);
       }

       // POST: Bugs/Delete/5
       [HttpPost, ActionName("Delete")]
       [ValidateAntiForgeryToken]
       public async Task<IActionResult> DeleteConfirmed(int id)
       {

            var bug = await _mediator.Send(new GetBugByIdQuery(id));
            if (bug != null)
           {
                await _mediator.Send(new DeleteBugByIdCommand { Id = id });
           }

          
           return RedirectToAction(nameof(BugIndex));
       }
 
       private async Task<bool> BugExists(int id)
       {
            var bug = await _mediator.Send(new GetBugByIdQuery(id));
            return bug.Data != null;
       }
      
    }
}
